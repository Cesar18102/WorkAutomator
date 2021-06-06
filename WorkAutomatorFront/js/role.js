var ROLE = {
	getRoles: function(callback){
		let request = {
			type : "POST",
			contentType : "application/json",
			url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
			data : JSON.stringify({ data : { id : SESSION.getUserId() } }),
			dataType : "json",
			success : function(response) {
				callback(response.data.roles);
			},
			error : function(response) {
				callback(undefined);
			}
		};
		
		SESSION.putToAjaxRequest(request);
		$.ajax(request);
	}
}

var ACCOUNT = {
	get: function(callback){
		let request = {
			type : "POST",
			contentType : "application/json",
			url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
			data : JSON.stringify({ data : { id : SESSION.getUserId() } }),
			dataType : "json",
			success : function(response) {
				callback(response.data)
			},
			error : function(response) {
				callback(undefined);
			}
		};
		
		SESSION.putToAjaxRequest(request);
		$.ajax(request);
	}
}