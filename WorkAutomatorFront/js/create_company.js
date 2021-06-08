var MODEL = {
	name : undefined,
	plan_image_url : undefined
};

new Vue({
	el : "#company-info",
	data : MODEL
});

function saveUserProfile(e) {
	let dto = {
		data : {
			name : MODEL.name,
			plan_image_url : MODEL.plan_image_url
		}
	}
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Company/Create",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			document.location = "../pages/company_profile.html";
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Create Company"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#company_profile_form').submit(saveUserProfile);