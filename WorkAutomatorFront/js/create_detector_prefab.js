var MODEL = {
	name : undefined,
	image_url : undefined,
	setting_prefabs : [],
	data_prefabs : [],
	fault_prefabs : []
};

Vue.component("detector_setting_prefab", {
	props : [ "setting_prefab" ],
	methods : {
		remove : function() {
			MODEL.setting_prefabs.splice(
				MODEL.setting_prefabs.indexOf(
					this.$props.setting_prefab
				), 1
			);
		}
	},
	template : 
	`<table style="width: 100%">
		<tr>
			<td>
				<div class="wrap-input100 validate-input">
					<input class="input100" type="text" name="name" v-model="setting_prefab.option_name" placeholder="Name">
				</div>
			</td>
			<td rowspan="2">
				<div class="common-button-container">
					<button type="button" class="common-button" v-on:click="remove" style="height: 8em;">
						X
					</button>
				</div>
			</td>
		</tr>
		</tr>
			<td>
				<div class="wrap-input100 validate-input">
					<select class="input100" v-model="setting_prefab.data_type_id" placeholder="Data Type">
						<option value="3">float</option>
						<option value="4">float[]</option>
						<option value="1">int</option>
						<option value="2">int[]</option>
						<option value="5">bool</option>
						<option value="6">bool[]</option>
					</select>
				</div>
			</td>
		</tr>
	</table>`
});

Vue.component("detector_data_prefab", {
	props : [ "data_prefab" ],
	methods : {
		remove : function() {
			MODEL.data_prefabs.splice(
				MODEL.data_prefabs.indexOf(
					this.$props.data_prefab
				), 1
			);
		}
	},
	template : 
	`<table style="width: 100%">
		<tr>
			<td>
				<div class="wrap-input100 validate-input">
					<input class="input100" type="text" name="name" v-model="data_prefab.field_name" placeholder="Name">
				</div>
			</td>
			<td rowspan="4">
				<div class="common-button-container">
					<button type="button" class="common-button" v-on:click="remove" style="height: 16em;">
						X
					</button>
				</div>
			</td>
		</tr>
		</tr>
			<td>
				<div class="wrap-input100 validate-input">
					<select class="input100" v-model="data_prefab.field_data_type_id" placeholder="Data Type">
						<option value="3">float</option>
						<option value="4">float[]</option>
						<option value="1">int</option>
						<option value="2">int[]</option>
						<option value="5">bool</option>
						<option value="6">bool[]</option>
					</select>
				</div>
			</td>
		</tr>
		<tr>
			<div class="wrap-input100 validate-input">
				<input class="input100" type="text" name="name" v-model="data_prefab.argument_name" placeholder="Argument name (e.g. Time)">
			</div>
		</tr>
		</tr>
			<td>
				<div class="wrap-input100 validate-input">
					<select class="input100" v-model="data_prefab.visualizer_type_id" placeholder="Visualizer Type">
						<option value="1">SIGNAL</option>
						<option value="2">VALUE</option>
					</select>
				</div>
			</td>
		</tr>
	</table>`
});

Vue.component("detector_fault_prefab", {
	props : [ "fault_prefab" ],
	methods : {
		remove : function() {
			MODEL.fault_prefabs.splice(
				MODEL.fault_prefabs.indexOf(
					this.$props.fault_prefab
				), 1
			);
		},
		addPitem : function() {
			this.$props.fault_prefab.fault_condition += "PITEM.";
		},
		addSetting : function(setting) {
			this.$props.fault_prefab.fault_condition += "DETECTOR." + setting.option_name;
		},
		addData : function(data) {
			this.$props.fault_prefab.fault_condition += "DATA." + data.field_name;
		},
		conditionChanged : function(e) {
			this.$props.fault_prefab.fault_condition = e.target.value;
		}
	},
	template : 
	`<table style="width: 100%">
		<tr>
			<td>
				<div class="wrap-input100 validate-input">
					<input class="input100" type="text" name="name" v-model="fault_prefab.name" placeholder="Name">
				</div>
			</td>
			<td rowspan="4">
				<div class="common-button-container">
					<button type="button" class="common-button" v-on:click="remove" style="height: 16em;">
						X
					</button>
				</div>
			</td>
		</tr>
		</tr>
			<td>
				<div class="wrap-input100 validate-input">
					<input class="input100" type="text" name="image_url" v-model="fault_prefab.image_url" placeholder="Image URL">
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<center v-if="fault_prefab.image_url != undefined">
					<img v-bind:src="fault_prefab.image_url" width="50" height="50" style="border-radius: 35px;"></img>
				</center>
			</td>
		</tr>
		<tr>
			<td>
				<div class="wrap-input100 validate-input">
					<textarea class="input100" type="text" name="name" style="min-height: 150px;" placeholder="Condition" 
						      v-bind:value="fault_prefab.fault_condition" v-on:input="conditionChanged">
					</textarea>
				</div>
				
				<span class="common-label" style="text-align: center;">
					Fault condition constructor helper
				</span>
				<div style="max-width: 50vw">
					<div class="common-button-container" style="width: 100%;">
						<button type="button" class="common-button" v-on:click="addPitem()">
							PIPELINE_ITEM.
						</button>
					</div>
					<div class="common-button-container" v-for="setting in $root.setting_prefabs" 
						 style="width: auto; display: inline-block; margin-left: 1em 1em 1em 1em;">
						<button type="button" class="common-button" v-if="setting.option_name" v-on:click="addSetting(setting)">
							DETECTOR.{{setting.option_name}}
						</button>
					</div>
					<div class="common-button-container" v-for="data in $root.data_prefabs" 
						 style="width: auto; display: inline-block; margin-left: 1em 1em 1em 1em;">
						<button type="button" class="common-button" v-if="data.field_name" v-on:click="addData(data)">
							DATA.{{data.field_name}}
						</button>
					</div>
				</div>
			</td>
		</tr>
	</table>`
});

new Vue({
	el : "#detector_prefab_reg_form",
	data : MODEL
});

function addSettingPrefab() {
	MODEL.setting_prefabs.push({ });
}

function addDataPrefab() {
	MODEL.data_prefabs.push({ });
}

function addFaultPrefab() {
	MODEL.fault_prefabs.push({ fault_condition : "" });
}

function save(e) {
	let dto = {
		data : {
			company_id : SESSION.getCompanyId(),
			name : MODEL.name,
			image_url : MODEL.image_url,
			detector_settings_prefabs : MODEL.setting_prefabs,
			detector_data_prefabs : MODEL.data_prefabs,
			detector_fault_prefabs : MODEL.fault_prefabs
		}
	}
	
	let request = {
		type : "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Prefab/CreateDetectorPrefab",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(reg_response) {
			document.location = "../pages/detector_prefabs.html";
		},
		error : function(reg_response) {
			throw JSON.stringify({ex: reg_response.responseJSON.error.exception, source: "Detector Prefab Create"});
		}
	}
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#detector_prefab_registration_form').submit(save);