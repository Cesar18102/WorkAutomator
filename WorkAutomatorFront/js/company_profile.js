var MODEL = {
	company : {
		name : undefined,
		plan_image_url : undefined
	}
};

new Vue({ el : "#company-info", data : MODEL });

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {4
		Object.assign(MODEL.company, response.data);
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Company Profile"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function saveUserProfile(e) {
	let dto = {
		data : {
			id : SESSION.getCompanyId(),
			name : MODEL.company.name,
			plan_image_url : MODEL.company.plan_image_url
		}
	}
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Company/Update",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			MODEL.company = response.data;
			throw JSON.stringify({nofail: true, source: "Update Company Profile", message: "Success"});
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Update Company Profile"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#company_profile_form').submit(saveUserProfile);