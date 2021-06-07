var HEADER_MODEL = {
	manage_company : true,
	create_company : true,
	
	plan : true,
	workers : true,
	
	create_pipeline_item_prefab : true,
	view_pipeline_item_prefabs : true,
	create_storage_cell_prefab : true,
	view_storage_cell_prefabs : true,
	create_detector_prefab : true,
	view_detector_prefabs : true,
	
	create_pipeline_item : true,
	view_pipeline_items : true,
	create_storage_cell : true,
	view_storage_cells : true,
	create_detector : true,
	view_detectors : true,
	
	viewPipelines : true,
	createPipeline : true,
	
	viewTasks : true,
	createTask : true
};

Vue.component('custom-header', {
	data : function() {
		return HEADER_MODEL;
	},
	methods : {
		exit: function() {
			SESSION.kill();
		},
		setLang: function(lang) {
			LOCALIZE.setLang(lang);
		}
	},
	template : 
	`<ul id="header_content" class="topmenu">
		<li><a class="menu_nav" href="#">{{ 'company' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li v-if="manage_company"><a href="../pages/company_profile.html">{{ 'manageCompany' | localize }}</a></li>
				<li v-if="create_company"><a href="../pages/company_registration.html">{{ 'newCompany' | localize }}</a></li>
				<li v-if="plan"><a href="../pages/setup_plan.html">{{ 'plan' | localize }}</a></li>
				<li v-if="workers"><a href="../pages/workers.html">{{ 'workers' | localize }}</a></li>
			</ul>
		</li>
		<li><a class="menu_nav" href="#">{{ 'prefabs' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li v-if="create_pipeline_item_prefab"><a href="../pages/create_pipeline_item_prefab.html">{{ 'createPipelineItemPrefab' | localize }}</a></li>
				<li v-if="view_pipeline_item_prefabs"><a href="../pages/pipeline_item_prefabs.html">{{ 'viewPipelineItemPrefabs' | localize }}</a></li>
				<li v-if="create_storage_cell_prefab"><a href="../pages/create_storage_cell_prefab.html">{{ 'createStorageCellPrefab' | localize }}</a></li>
				<li v-if="view_storage_cell_prefabs"><a href="../pages/storage_cell_prefabs.html">{{ 'viewStorageCellPrefabs' | localize }}</a></li>
				<li v-if="create_detector_prefab"><a href="../pages/create_detector_prefab.html">{{ 'createDetectorPrefab' | localize }}</a></li>
				<li v-if="view_detector_prefabs"><a href="../pages/detector_prefabs.html">{{ 'viewDetectorPrefabs' | localize }}</a></li>
			</ul>
		</li>
		<li><a class="menu_nav" href="#">{{ 'pipeline' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li v-if="viewPipelines"><a href="../pages/pipelines.html">{{ 'pipelinesList' | localize }}</a></li>
				<li v-if="createPipeline"><a href="../pages/create_pipeline.html">{{ 'newPipeline' | localize }}</a></li>
			</ul>
		</li>
		<li><a class="menu_nav" href="#">{{ 'task' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li v-if="viewTasks"><a href="../pages/my_tasks.html">{{ 'taskList' | localize }}</a></li>
				<li v-if="createTask"><a href="../pages/create_task.html">{{ 'newTask' | localize }}</a></li>
			</ul>
		</li>
		<li style="float: right"><a class="menu_nav" href="#">{{ 'lang' | localize }}</a>
			<ul class="submenu">
				<li v-on:click="setLang('eng')"><a>English</a></li>
				<li v-on:click="setLang('ukr')"><a>Ukrainian</a></li>
			</ul>
		</li>
		<li style="float: right"><a class="menu_nav" href="#">{{ 'account' | localize }}</a>
			<ul class="submenu">
				<li><a href="../pages/user_profile.html">{{ 'settings' | localize }}</a></li>
				<li><a v-on:click="exit()" href="../index.html">{{ 'leave' | localize }}</a></li>
			</ul>
		</li>
	</ul>`
});

new Vue({ el : "#header" });

ROLE.getRoles(role => {
	
});