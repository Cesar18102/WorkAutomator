var MODEL = {
	company : {
		name : undefined,
		plan_image_url : undefined,
		members : [],
		workers : []
	},
	permissions_account : undefined,
	freeAccounts : [],
	
	db_permissions : [],
	pipeline_item_permissions : [],
	storage_cell_permissions : [],
	detector_permissions : [],
	manufactory_permissions : []
};

function cancelPermissions(){
	MODEL.permissions_account = undefined;
}

function savePermissions() {
	let workerAccount = MODEL.permissions_account;
	let role = workerAccount.roles.find(
		r => r.name == ("COMPANY #" + SESSION.getCompanyId() + " OWNER") || 
			 r.name == ("COMPANY #" + SESSION.getCompanyId() + " MEMBER #" + SESSION.getUserId())
	);
	
	let dto = {
		data : {
			id : role.id,
			name : role.name,
			db_permission_ids : MODEL.db_permissions.reduce((acc, p) => acc.concat(p.permissions), []).filter(p => p.granted).map(p => p.id),
			manufactory_ids : MODEL.manufactory_permissions.filter(p => p.granted).map(p => p.id),
			storage_cell_ids : MODEL.storage_cell_permissions.filter(p => p.granted).map(p => p.id),
			pipeline_item_ids : MODEL.pipeline_item_permissions.filter(p => p.granted).map(p => p.id),
			detector_ids : MODEL.detector_permissions.filter(p => p.granted).map(p => p.id)
		}
	}
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Role/Update",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			MODEL.permissions_account = undefined;
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Update Permissions"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request)
}

Vue.component('worker_list_item', {
	props : ['worker'],
	methods : {
		fire : function() {
			let worker = this.$props.worker;
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Company/FireMember",
				data : JSON.stringify({ data : { company_id : SESSION.getCompanyId(), account_id : worker.id } }),
				dataType : "json",
				success : function(response) {
					MODEL.company.members.splice(
						MODEL.company.members.indexOf(
							worker
						), 1
					);
					
					MODEL.freeAccounts.push(worker);
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Fire worker"});
				}
			};
			
			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		},
		editPermissions : function() {
			let worker = this.$props.worker;
			
			let workerRequest = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
				data : JSON.stringify({ data : { id : worker.id } }),
				dataType : "json",
				success : function(response) {
					MODEL.permissions_account = response.data;
					let workerAccount = MODEL.permissions_account;
			
					let dbPermissions = workerAccount.roles.reduce(
						(acc, role) => acc.concat(role.db_permissions), []
					);
					
					for(let db_permission of MODEL.db_permissions) {
						for(let db_permission_typed of db_permission.permissions) {
							if(dbPermissions.find(p => p.id == db_permission_typed.id)) {
								db_permission_typed.granted = true;
							} else {
								db_permission_typed.granted = false;
							}
						}
					}
					
					let manufactoryPermissions = workerAccount.roles.reduce(
						(acc, role) => acc.concat(role.manufactory_permissions), []
					);
					
					for(let manufactory_permission of MODEL.manufactory_permissions) {
						if(manufactoryPermissions.find(p => p.id == manufactory_permission.id)) {
							manufactory_permission.granted = true;
						} else {
							manufactory_permission.granted = false;
						}
					}
					
					let pipelineItemPermissions = workerAccount.roles.reduce(
						(acc, role) => acc.concat(role.pipeline_item_permissions), []
					);
					
					for(let pipeline_item_permission of MODEL.pipeline_item_permissions) {
						if(pipelineItemPermissions.find(p => p.id == pipeline_item_permission.id)) {
							pipeline_item_permission.granted = true;
						} else {
							pipeline_item_permission.granted = false;
						}
					}
					
					let storageCellPermissions = workerAccount.roles.reduce(
						(acc, role) => acc.concat(role.storage_cell_permissions), []
					);
					
					for(let storage_cell_permission of MODEL.storage_cell_permissions) {
						if(storageCellPermissions.find(p => p.id == storage_cell_permission.id)) {
							storage_cell_permission.granted = true;
						} else {
							storage_cell_permission.granted = false;
						}
					}
					
					let detectorPermissions = workerAccount.roles.reduce(
						(acc, role) => acc.concat(role.detector_permissions), []
					);
					
					for(let detector_permission of MODEL.detector_permissions) {
						if(detectorPermissions.find(p => p.id == detector_permission.id)) {
							detector_permission.granted = true;
						} else {
							detector_permission.granted = false;
						}
					}
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Workers"});
				}
			};

			SESSION.putToAjaxRequest(workerRequest);
			$.ajax(workerRequest);
		}
	},
	template :
	`<div class="listItemContainer" style="width: auto; height: auto; display: block;">
		<span> {{ worker.login }} </span>
		<div>
			<div class="common-button-container" style="display: inline-block; width: 40%">
				<button class="common-button" v-on:click="fire" type="button">Fire</button>
			</div>
			<div class="common-button-container" style="display: inline-block; width: 40%">
				<button class="common-button" v-on:click="editPermissions" type="button">Edit Permissions</button>
			</div>
		</div>
	</div>`
});

new Vue({ el : "#company-info", data : MODEL });

function hire() {
	let account_id = document.getElementById("hire_worker_select").value;
	
	let hireRequest = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Company/HireMember",
		data : JSON.stringify({ data : { company_id : SESSION.getCompanyId(), account_id : account_id } }),
		dataType : "json",
		success : function(response) {
			MODEL.freeAccounts.splice(
				MODEL.freeAccounts.indexOf(
					acc => acc.id == account_id
				), 1
			);
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
				data : JSON.stringify({ data : { id : account_id } }),
				dataType : "json",
				success : function(response) {
					MODEL.company.members.push(response.data);
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Hired Worker"});
				}
			};
			
			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Hire Worker"});
		}
	};
	
	SESSION.putToAjaxRequest(hireRequest);
	$.ajax(hireRequest);
}

let freeAccountsRequest = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Account/GetFreeAccounts",
	data : JSON.stringify({ data : { id : 0 } }),
	dataType : "json",
	success : function(response) {
		MODEL.freeAccounts = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Free Accounts"});
	}
};

SESSION.putToAjaxRequest(freeAccountsRequest);
$.ajax(freeAccountsRequest);

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		Object.assign(MODEL.company, response.data);
		MODEL.manufactory_permissions = response.data.manufactories;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Company Profile"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

let pipelineItemsRequest = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/PipelineItem/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.pipeline_item_permissions = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipeline Items"});
	}
};

SESSION.putToAjaxRequest(pipelineItemsRequest);
$.ajax(pipelineItemsRequest);

let storageCellsRequest = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/StorageCell/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.storage_cell_permissions = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Storage Cells"});
	}
};

SESSION.putToAjaxRequest(storageCellsRequest);
$.ajax(storageCellsRequest);

let detectorsRequest = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Detector/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.detector_permissions = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detectors"});
	}
};

SESSION.putToAjaxRequest(detectorsRequest);
$.ajax(detectorsRequest);

let dbPermissionsRequest = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/GetDbPermissions",
	data : JSON.stringify({ data : { id : 0 } }),
	dataType : "json",
	success : function(response) {
		let res = [];
		for(let dbp of response.data) {
			let permissionsChunk = res.find(p => p.table == dbp.table);
			
			if(!permissionsChunk) {
				res.push({
					table : dbp.table,
					permissions : [ dbp ]
				})
			} else {
				permissionsChunk.permissions.push(dbp);
			}
		}
		MODEL.db_permissions = res;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Db Permissions"});
	}
};

SESSION.putToAjaxRequest(dbPermissionsRequest);
$.ajax(dbPermissionsRequest);

function saveCompanyProfile(e) {
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

$('#company_profile_form').submit(saveCompanyProfile);