var MODEL = {
	detector_prefabs : []
};

Vue.component('detector_prefab_list_item', {
	props : ['detector_prefab'],
	template :
	`<div class="listItemContainer">
		<table style="width: 100%;">
			<tr>
				<td rowspan="2" style="padding-right: 3em; width: 100px;">
					<img v-bind:src="detector_prefab.image_url" width="100" height="100"></img>
				</td>
				<td style="width: 10%;">
					<span class="common-label">Name</span>
					<br/>
					<span>{{ detector_prefab.name }}</span>
				</td>
				<td style="width: 50%;">
					<center><span class="common-label">Settings</span></center>
				</td>
				<td style="width: 50%;">
					<center><span class="common-label">Data Format</span></center>
				</td>
			</tr>
			<tr>
				<td style="width: 10%;">
					<span class="common-label" v-if="detector_prefab.description">Description</span>
					<br/>
					<span>{{ detector_prefab.description }}</span>
				</td>
				<td style="width: 50%;">
					<center>
						<div v-for="settings_prefab of detector_prefab.detector_settings_prefabs" v-if="detector_prefab.detector_settings_prefabs.length != 0">
							<span class="common-label" style="font-size: 1em;">
								{{ settings_prefab.option_name }}
							</span>
							<span> - </span>
							<span><i> {{ settings_prefab.option_data_type_id.name }} </i></span>
						</div>
						<span v-if="detector_prefab.detector_settings_prefabs.length == 0"><i> EMPTY </i></span>
					</center>
				</td>
				<td style="width: 50%;">
					<center>
						<div v-for="data_prefab of detector_prefab.detector_data_prefabs" v-if="detector_prefab.detector_data_prefabs.length != 0">
							<span class="common-label" style="font-size: 1em;">{{ data_prefab.field_name }}</span>
							<span><i>({{ data_prefab.field_data_type.name }})</i></span>
							<span> visualized as <b>{{ data_prefab.visualizer_type.name }}</b></span>
							<span v-if="data_prefab.argument_name"> depending on <b>{{ data_prefab.argument_name }}</b></span>
						</div>
						<span v-if="detector_prefab.detector_data_prefabs.length == 0"><i> EMPTY </i></span>
					</center>
				</td>
			</tr>
			<tr>
				<td colspan="4" style="width: 100%; padding-top: 3em">
					<center>
						<span class="common-label">Fault Prefabs</span>
						<br/>
						<table v-if="detector_prefab.detector_fault_prefabs.length != 0">
							<tr v-for="fault_prefab_chunk of $options.filters.chunk(detector_prefab.detector_fault_prefabs, 5)">
								<td v-for="fault_prefab of fault_prefab_chunk">
									<img v-if="fault_prefab.image_url" v-bind:src="fault_prefab.image_url" width="30px" height="30px"></img>
									<span><b>{{ fault_prefab.name }}<b/>: </span>
									<span><i>{{ fault_prefab.fault_condition }}</i></span>
								</td>
							</tr>
						</table>
						<span v-if="detector_prefab.detector_fault_prefabs.length == 0"><i> EMPTY </i></span>
					</center>
				</td>
			</tr>
		</table>
	</div>`
});

new Vue({
	el : "#detector_prefabs",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() }}),
	dataType : "json",
	success : function(response) {
		MODEL.detector_prefabs = response.data.detector_prefabs;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detector Prefabs List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);