var MODEL = {
	image_url: 'https://ce-na.ru/upload/iblock/afb/afb01ea6b753ae90a6ccf14126d36bc6.jpg',
	
	pipeline_item_prefabs : [],
	pipeline_items : [],
	
	storage_cell_prefabs : [],
	storage_cells : [],
	
	detector_prefabs : [],
	detectors : []
};

new Vue({
	el : "#pipeline",
	data : MODEL
});

Vue.component("pipeline_item_prefab_list_item", {
	props : [ "pipeline_item_prefab" ],
	template : 
	`<div> 
		<span> {{ pipeline_item_prefab.name }} </span>
		<img v-bind:src="pipeline_item_prefab.image_url" width="40" height="40"></img>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button">New {{ pipeline_item_prefab.name }}</button>
		</div>
	</div>`
});

Vue.component("storage_cell_prefab_list_item", {
	props : [ "storage_cell_prefab" ],
	template : 
	`<div> 
		<img v-bind:src="storage_cell_prefab.image_url" width="40" height="40"></img>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button">New</button>
		</div>
	</div>`
});

Vue.component("detector_prefab_list_item", {
	props : [ "detector_prefab" ],
	template : 
	`<div>
		<span> {{ detector_prefab.name }} </span>
		<img v-bind:src="detector_prefab.image_url" width="30" height="30"></img>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button">New {{ detector_prefab.name }}</button>
		</div>
	</div>`
});

Vue.component("pipeline_item_list_item", {
	props : [ "pipeline_item" ],
	template : 
	`<div> 
		<span> {{ pipeline_item.prefab.name }} #{{ pipeline_item.id }} </span>
		<img v-bind:src="pipeline_item.prefab.image_url" width="40" height="40"></img>
	</div>`
});

Vue.component("storage_cell_list_item", {
	props : [ "storage_cell" ],
	template : 
	`<div> 
		<img v-bind:src="storage_cell.prefab.image_url" width="40" height="40"></img>
		<span> #{{ storage_cell.id }} </span>
	</div>`
});

Vue.component("detector_list_item", {
	props : [ "detector" ],
	template : 
	`<div>
		<span> {{ detector.prefab.name }} #{{ detector.id }} </span>
		<img v-bind:src="detector.prefab.image_url" width="30" height="30"></img>
	</div>`
});

let canvas = document.getElementById('plan');
let ctx = canvas.getContext('2d');

let image = new Image();
image.onload = drawImageActualSize;
image.src = MODEL.image_url;

function drawImageActualSize() {
	canvas.width = this.width;
	canvas.height = this.height;
	ctx.drawImage(this, 0, 0, this.width, this.height);
}

function getMousePosition(canvas, event) {
	let rect = canvas.getBoundingClientRect();
	let x = event.clientX - rect.left;
	let y = event.clientY - rect.top;
	return { x: x, y: y };
}

function updateCanvas() {
	ctx.clearRect(0, 0, canvas.width, canvas.height);
	ctx.drawImage(image, 0, 0, image.width, image.height);
}

canvas.addEventListener("mousedown", e => {
	let position = getMousePosition(canvas, e);
	updateCanvas();
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.pipeline_item_prefabs = response.data.pipeline_item_prefabs;
		MODEL.storage_cell_prefabs = response.data.storage_cell_prefabs;
		MODEL.detector_prefabs = response.data.detector_prefabs;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Company"});
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
		MODEL.pipeline_items = response.data;
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
		MODEL.storage_cells = response.data;
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
		MODEL.detectors = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detectors"});
	}
};

SESSION.putToAjaxRequest(detectorsRequest);
$.ajax(detectorsRequest);