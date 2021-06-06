var MODEL = {
	pipeline_item_prefabs : []
};

Vue.component('pipeline_item_prefab_list_item', {
	props : ['pipeline_item_prefab'],
	mounted : function () {
		let prefab = this.$props.pipeline_item_prefab;
		let canvas = this.$refs.canvas;
		
		let ctx = canvas.getContext('2d');
		let image = new Image();
		
		image.onload = () => {
			ctx.clearRect(0, 0, canvas.width, canvas.height);
			ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
			
			if(prefab.input_x && prefab.input_y) {
				ctx.fillStyle = "green";
				ctx.beginPath();
				ctx.arc(prefab.input_x * canvas.width, prefab.input_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
			
			if(prefab.output_x && prefab.output_y) {
				ctx.fillStyle = "red";
				ctx.beginPath();
				ctx.arc(prefab.output_x * canvas.width, prefab.output_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
		};
		
		image.src = prefab.image_url;
	},
	template :
	`<div class="listItemContainer">
		<table style="width: 100%;">
			<tr>
				<td rowspan="2" style="padding-right: 3em; width: 100px;">
					<canvas ref="canvas" width="100" height="100"></canvas>
				</td>
				<td style="width: 10%;">
					<span class="common-label">Name </span>
					<br/>
					<span>{{ pipeline_item_prefab.name }}</span>
				</td>
				<td style="width: 100%;">
					<center><span class="common-label">Settings</span></center>
				</td>
			</tr>
			<tr>
				<td style="width: 10%;">
					<span class="common-label" v-if="pipeline_item_prefab.description">Description: </span>
					<br/>
					<span>{{ pipeline_item_prefab.description }}</span>
				</td>
				<td style="width: 100%;">
					<center>
						<div v-for="settings_prefab of pipeline_item_prefab.settings_prefabs" v-if="pipeline_item_prefab.settings_prefabs.length != 0">
							<span class="common-label" style="font-size: 1em;">
								{{ settings_prefab.option_name }}
							</span>
							<span> - </span>
							<span><i> {{ settings_prefab.data_type.name }} </i></span>
						</div>
						<span v-if="pipeline_item_prefab.settings_prefabs.length == 0"><i> EMPTY </i></span>
					</center>
				</td>
			</tr>
		</table>
	</div>`
});

new Vue({
	el : "#pipeline_item_prefabs",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() }}),
	dataType : "json",
	success : function(response) {
		MODEL.pipeline_item_prefabs = response.data.pipeline_item_prefabs;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipeline Item Prefabs List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);