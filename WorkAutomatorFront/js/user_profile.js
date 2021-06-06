var MODEL = {
	user : {
		login : undefined,
		first_name : undefined,
		last_name : undefined
	}
};

new Vue({ el : "#user-info", data : MODEL });

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
	data : JSON.stringify({ data : { id : SESSION.getUserId() } }),
	dataType : "json",
	success : function(response) {4
		Object.assign(MODEL.user, response.data);
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get User Profile"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function saveUserProfile(e) {
	let data = $('#user_profile_form').serializeObject();
	
	let dto = {
		data : {
			id : SESSION.getUserId(),
			first_name : data.first_name,
			last_name : data.last_name
		}
	}
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Account/Update",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			MODEL.user = response.data;
			throw JSON.stringify({nofail: true, source: "Update User Profile", message: "Success"});
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Update User Profile"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#user_profile_form').submit(saveUserProfile);