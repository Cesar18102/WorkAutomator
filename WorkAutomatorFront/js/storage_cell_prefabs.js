var MODEL = {
	storage_cell_prefabs : []
};

Vue.component('storage_cell_prefab_list_item', {
	props : ['storage_cell_prefab'],
	mounted : function () {
		let prefab = this.$props.storage_cell_prefab;
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
	`<div class="listItemContainer" style="width: auto; height: auto; display: block;">
		<canvas ref="canvas" width="100" height="100"></canvas>
		
		<div v-if="storage_cell_prefab.name">
			<br/>
			<span class="common-label">Name</span>
			<br/>
			<span>{{ storage_cell_prefab.name }}</span>
		</div>
		
		<div v-if="storage_cell_prefab.description">
			<br/>
			<span class="common-label">Description</span>
			<br/>
			<span>{{ storage_cell_prefab.description }}</span>
		</div>
	</div>`
});

new Vue({
	el : "#storage_cell_prefabs",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() }}),
	dataType : "json",
	success : function(response) {
		MODEL.storage_cell_prefabs = response.data.storage_cell_prefabs;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipeline Item Prefabs List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);