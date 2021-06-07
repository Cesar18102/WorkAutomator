var MODEL = {
	pipelines : []
};

Vue.component('pipeline_list_item', {
	props : ['pipeline'],
	methods : {
		edit : function() {
			document.location = "../pages/create_pipeline.html?id=" + this.$props.pipeline.id;
		},
		details : function() {
			document.location = "../pages/pipeline.html?id=" + this.$props.pipeline.id;
		},
		scrap : function() {
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Pipeline/Scrap",
				data : JSON.stringify({ data : { id : this.$props.pipeline.id }}),
				dataType : "json",
				success : function(response) {
					MODEL.pipelines.splice(
						MODEL.pipelines.indexOf(this.$props.pipeline), 1
					);
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Scrap Pipeline"});
				}
			};

			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		}
	},
	template :
	`<div class="listItemContainer" style="width: auto; height: auto; display: block;">
		<span>Pipeline #{{ pipeline.id }}</span>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button" v-on:click="details">Details</button>
		</div>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button" v-on:click="edit">Edit</button>
		</div>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button" v-on:click="scrap">Scrap</button>
		</div>
	</div>`
});

new Vue({
	el : "#pipelines",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Pipeline/GetAll",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() }}),
	dataType : "json",
	success : function(response) {
		MODEL.pipelines = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipelines List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);