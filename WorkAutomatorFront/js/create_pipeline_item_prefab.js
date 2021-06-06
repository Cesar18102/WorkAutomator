var MODEL = {
	name : undefined,
	image_url : undefined,
	input_x : undefined,
	input_y : undefined,
	output_x : undefined,
	output_y : undefined,
	setting_prefabs : []
};

new Vue({
	el : "#pipeline_item_prefab_reg_form",
	data : MODEL
});

Vue.component("pipeline_item_setting_prefab", {
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
	`<table>
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

function addSetting() {
	MODEL.setting_prefabs.push({ });
}

let canvas = document.getElementById('pipeline_item_prefab_canvas');
let ctx = canvas.getContext('2d');
let image = new Image();

function updateCanvas() {
	image.src = MODEL.image_url;
	
	image.onload = () => {
		ctx.clearRect(0, 0, canvas.width, canvas.height);
		ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
		
		if(MODEL.input_x && MODEL.input_y) {
			ctx.fillStyle = "green";
			ctx.beginPath();
			ctx.arc(MODEL.input_x * canvas.width, MODEL.input_y * canvas.height, 6, 0, 2 * Math.PI, false);
			ctx.fill();
		}
		
		if(MODEL.output_x && MODEL.output_y) {
			ctx.fillStyle = "red";
			ctx.beginPath();
			ctx.arc(MODEL.output_x * canvas.width, MODEL.output_y * canvas.height, 6, 0, 2 * Math.PI, false);
			ctx.fill();
		}
	};
}

let addPointFunction = undefined;

function setInputPoint(position) {
	MODEL.input_x = Math.round(position.x * 10000 / canvas.width) / 10000;
	MODEL.input_y = Math.round(position.y * 10000 / canvas.height) / 10000;
}

function setOuputPoint(position) {
	MODEL.output_x = Math.round(position.x * 10000 / canvas.width) / 10000;
	MODEL.output_y = Math.round(position.y * 10000 / canvas.height) / 10000;
}

function setInputMode() {
	addPointFunction = setInputPoint;
}

function setOutputMode() {
	addPointFunction = setOuputPoint;
}

function getMousePosition(canvas, event) {
	let rect = canvas.getBoundingClientRect();
	let x = event.clientX - rect.left;
	let y = event.clientY - rect.top;
	return { x: x, y: y };
}

canvas.addEventListener("mousedown", e => {
	let position = getMousePosition(canvas, e);
	
	if(addPointFunction) {
		addPointFunction(position);
		updateCanvas();
	}
});

function save(e) {
	let dto = {
		data : {
			company_id : SESSION.getCompanyId(),
			name : MODEL.name,
			image_url : MODEL.image_url,
			input_x : MODEL.input_x,
			input_y : MODEL.input_y,
			output_x : MODEL.output_x,
			output_y : MODEL.output_y,
			settings_prefabs : MODEL.setting_prefabs.map(sp => {
				return {
					data_type_id : sp.data_type_id,
					option_name : sp.option_name
				}
			})
		}
	}
	
	let request = {
		type : "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Prefab/CreatePipelineItemPrefab",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(reg_response) {
			document.location = "../pages/pipeline_item_prefabs.html";
		},
		error : function(reg_response) {
			throw JSON.stringify({ex: reg_response.responseJSON.error.exception, source: "Pipeline Item Prefab Create"});
		}
	}
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#pipeline_item_prefab_registration_form').submit(save);