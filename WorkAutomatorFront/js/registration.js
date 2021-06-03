function register(ev) {
	let form = $("#registration_form");
	let data = form.serializeObject();
	
	if(data.pass != data.pass_confirm)
		alert("Passwords do not match");
	
	$.ajax({
		type : "GET",
		url : "http://worknplay.somee.com/api/Key/GetPublicAsymmetricKey",
		success : response => {
			let publicKey = new RSAPublicKey(
				Base64.decode(response.data.modulus), 
				Base64.decode(response.data.exponent)
			);
			let encryptedPass = RSA.encrypt(
				data.pass, 
				publicKey
			);
			
			let dto = {
				login : data.login,
				first_name : data.first_name,
				last_name : data.last_name,
				email : data.email,
				password_encrypted : encryptedPass,
				public_key : {
					modulus : response.data.modulus,
					exponent : response.data.exponent
				}
			}
			
			$.ajax({
				type : "POST",
				contentType : "application/json",
				url : "http://worknplay.somee.com/api/Auth/SignUp",
				data : JSON.stringify(dto),
				dataType : "json",
				success : function(reg_response) {
					document.location = "../index.html";
				},
				error : function(reg_response) {
					alert("registration fail: " + JSON.stringify(reg_response));
				}
			});
		}
	});
	
	ev.preventDefault();
	ev.stopPropagation();
}

$("#registration_form").submit(register);