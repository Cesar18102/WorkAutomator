var MODEL = {
	name : undefined,
	members : []
};

new Vue({
	el : "#task_reg_form",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
	data : JSON.stringify({ data : { id : SESSION.getUserId() }}),
	dataType : "json",
	success : function(response) {
		MODEL.members = response.data.subs;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Subs List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function save(e) {
	let request = {
		type : "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Task/Create",
		data : JSON.stringify({ data : { company_id : SESSION.getCompanyId(), name : MODEL.name } }),
		dataType : "json",
		success : function(reg_response) {
			let dto = {
				data : {
					id : reg_response.data.id,
					assignee_account_id : document.getElementById("assignee_select").value,
					reviewer_account_id : document.getElementById("reviewer_select").value
				}
			}
			
			if(dto.data.assignee_account_id == "null") {
				dto.data.assignee_account_id = null;
			}
			
			if(dto.data.reviewer_account_id == "null") {
				dto.data.reviewer_account_id = null;
			}
			
			let setAssigneeRequest = {
				type : "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Task/Assign",
				data : JSON.stringify(dto),
				dataType : "json",
				success : function(reg_response) {
					document.location = "../pages/tasks.html";
				},
				error : function(reg_response) {
					throw JSON.stringify({ex: reg_response.responseJSON.error.exception, source: "Assign Task"});
				}
			}
			
			SESSION.putToAjaxRequest(setAssigneeRequest);
			$.ajax(setAssigneeRequest);
			
		},
		error : function(reg_response) {
			throw JSON.stringify({ex: reg_response.responseJSON.error.exception, source: "Storage Cell Prefab Create"});
		}
	}
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#task_registration_form').submit(save);