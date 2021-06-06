var LOCALIZE = {
	eng : [{
		key : "companyProfile",
		value : "Company Profile"
	}, {
		key : "company",
		value : "Company"
	}, {
		key : "manageCompany",
		value : "Manage company"
	}, {
		key : "newCompany",
		value : "New company"
	}, {
		key : "name",
		value : "Name"
	}, {
		key : "plan",
		value : "Plan"
	}, {
		key : "workers",
		value : "Workers"
	}, {
		key : "prefabs",
		value : "Prefabs"
	}, {
		key : "createPipelineItemPrefab",
		value : "Create Pipeline Item Prefab"
	}, {
		key : "viewPipelineItemPrefabs",
		value : "Pipeline Item Prefabs List"
	}, {
		key : "createStorageCellPrefab",
		value : "Create Storage Cell Prefab"
	}, {
		key : "viewStorageCellPrefabs",
		value : "Storage Cell Prefabs List"
	}, {
		key : "createDetectorPrefab",
		value : "Create Detector Prefab"
	}, {
		key : "viewDetectorPrefabs",
		value : "Detector Prefabs List"
	}, {
		key : "items",
		value : "Items"
	}, {
		key : "createPipelineItem",
		value : "Create Pipeline Item"
	}, {
		key : "viewPipelineItems",
		value : "Pipeline Items List"
	}, {
		key : "createStorageCell",
		value : "Create Storage Cell"
	}, {
		key : "viewStorageCells",
		value : "Storage Cells List"
	}, {
		key : "createDetector",
		value : "Create Detector"
	}, {
		key : "viewDetectors",
		value : "Detectors List"
	}, {
		key : "pipeline",
		value : "Pipeline"
	}, {
		key : "pipelinesList",
		value : "Pipelines List"
	}, {
		key : "newPipeline",
		value : "New Pipeline"
	}, {
		key : "task",
		value : "Task"
	}, {
		key : "taskList",
		value : "Task List"
	}, {
		key : "newTask",
		value : "New Task"
	},  {
		key : "delete",
		value : "Delete"
	}, {
		key : "edit",
		value : "Edit"
	}, {
		key : "lang",
		value : "Lang"
	}, {
		key : "account",
		value : "Account"
	}, {
		key : "settings",
		value : "Settings"
	}, {
		key : "leave",
		value : "Leave"
	}, {
		key : "save",
		value : "Save"
	}, {
		key : "user_profile",
		value : "My profile"
	}, {
		key : "login",
		value : "Login"
	}, {
		key : "first_name",
		value : "First name"
	}, {
		key : "last_name",
		value : "Last name"
	}, {
		key : "email",
		value : "Email"
	}],
	ukr : [{
		key : "company",
		value : "Company"
	}, {
		key : "manageCompany",
		value : "Manage company"
	}, {
		key : "newCompany",
		value : "New company"
	}, {
		key : "plan",
		value : "Plan"
	}, {
		key : "viewPlan",
		value : "View plan"
	}, {
		key : "managePlan",
		value : "Manage plan"
	}, {
		key : "prefabs",
		value : "Prefabs"
	}, {
		key : "createPipelineItemPrefab",
		value : "Create Pipeline Item Prefab"
	}, {
		key : "viewPipelineItemPrefabs",
		value : "Pipeline Item Prefabs List"
	}, {
		key : "createStorageCellPrefab",
		value : "Create Storage Cell Prefab"
	}, {
		key : "viewStorageCellPrefabs",
		value : "Storage Cell Prefabs List"
	}, {
		key : "createDetectorPrefab",
		value : "Create Detector Prefab"
	}, {
		key : "viewDetectorPrefabs",
		value : "Detector Prefabs List"
	}, {
		key : "items",
		value : "Items"
	}, {
		key : "createPipelineItem",
		value : "Create Pipeline Item"
	}, {
		key : "viewPipelineItems",
		value : "Pipeline Items List"
	}, {
		key : "createStorageCell",
		value : "Create Storage Cell"
	}, {
		key : "viewStorageCells",
		value : "Storage Cells List"
	}, {
		key : "createDetector",
		value : "Create Detector"
	}, {
		key : "viewDetectors",
		value : "Detectors List"
	}, {
		key : "pipeline",
		value : "Pipeline"
	}, {
		key : "pipelinesList",
		value : "Pipelines List"
	}, {
		key : "newPipeline",
		value : "New Pipeline"
	}, {
		key : "task",
		value : "Task"
	}, {
		key : "taskList",
		value : "Task List"
	}, {
		key : "newTask",
		value : "New Task"
	},  {
		key : "delete",
		value : "Delete"
	}, {
		key : "edit",
		value : "Edit"
	}, {
		key : "lang",
		value : "Lang"
	}, {
		key : "account",
		value : "Account"
	}, {
		key : "settings",
		value : "Settings"
	}, {
		key : "leave",
		value : "Leave"
	}, {
		key : "save",
		value : "Save"
	}, {
		key : "user_profile",
		value : "User's profile"
	}, {
		key : "login",
		value : "Login"
	}, {
		key : "first_name",
		value : "First name"
	}, {
		key : "last_name",
		value : "Last name"
	}, {
		key : "email",
		value : "Email"
	}],
	getString: function(id) {
		let lang = COOKIE.getCookie("LANG");
		let data = this.eng;
		
		if(lang == "ukr") {
			data = this.ukr;
		}

		let kvp = data.find(dt => dt.key == id);
		
		return kvp ? kvp.value : "NOLOC<" + id + ">";
	},
	setLang: function(lang) {
		COOKIE.setCookie("LANG", lang);
	}
}