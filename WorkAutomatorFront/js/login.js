function login(ev) {
	let form = $("#login_form");
	let data = form.serializeObject();
	
	let hashed_pass = sha256(data.pass).toUpperCase();
	let salt = Math.round(Math.random() * 100000).toString();
	let pass_salted = sha256(hashed_pass + salt);
	
	let dto = {
		login : data.login,
		password_salted : pass_salted,
		salt : salt
	}
	
	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "http://worknplay.somee.com/api/Auth/LogIn",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(log_response) {
			SESSION.createSession(log_response.data.user_id, log_response.data.session_token);
			document.location = "./pages/user_profile.html";
		},
		error : function(log_response) {
			alert("login fail: " + JSON.stringify(log_response));
		}
	});
	
	ev.preventDefault();
	ev.stopPropagation();
}

$("#login_form").submit(login);