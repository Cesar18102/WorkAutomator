const PIPELINE_ITEM_SIZE = 50;
const STORAGE_CELL_SIZE = 50;
const DETECTOR_SIZE = 30;
const FAULT_SIZE = 20;

var MODEL = {
	pipeline_id: new URLSearchParams(window.location.search).get('id'),
	image_url: undefined,
	
	pipeline_items : [],
	storage_cells : [],
	detectors : [],
	
	pipeline_item_pipeline_item_connections : [],
	pipeline_item_storage_cell_connections : [],
	
	setting_pipeline_item : undefined,
	setting_detector : undefined,
	
	savePipelineItemSettings : function() {
		//TODO
	},
	
	cancelPipelineItemSettings : function() {
		MODEL.setting_pipeline_item = undefined;
	},
	
	saveDetectorSettings : function() {
		//TODO
	},
	
	cancelDetectorSettings : function() {
		MODEL.setting_detector = undefined;
	}
};

Vue.component("pipeline_item_visualizer", {
	props : [ "pipeline_item" ],
	mounted : function () {
		let pipeline_item = this.$props.pipeline_item;
		
		let canvas = this.$refs.canvas;
		let ctx = canvas.getContext('2d');	
		
		let image = new Image();
		
		image.onload = () => {
			ctx.clearRect(0, 0, canvas.width, canvas.height);
			ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
			
			if(pipeline_item.prefab.input_x && pipeline_item.prefab.input_y) {
				ctx.fillStyle = "green";
				ctx.beginPath();
				ctx.arc(pipeline_item.prefab.input_x * canvas.width, pipeline_item.prefab.input_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
			
			if(pipeline_item.prefab.output_x && pipeline_item.prefab.output_y) {
				ctx.fillStyle = "red";
				ctx.beginPath();
				ctx.arc(pipeline_item.prefab.output_x * canvas.width, pipeline_item.prefab.output_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
		};
		
		image.src = pipeline_item.prefab.image_url;
		
		let rect = plan_image.getBoundingClientRect();
		
		canvas.parentNode.parentNode.style.left = pipeline_item.x * plan_image.width - PIPELINE_ITEM_SIZE / 2 + rect.left;
		canvas.parentNode.parentNode.style.top = pipeline_item.y * plan_image.height - PIPELINE_ITEM_SIZE / 2 + rect.top + window.scrollY;
		
		pipeline_item.control = canvas.parentNode.parentNode;
		pipeline_item.canvas = canvas;
		
		pipeline_item.updateConnections = function() {
			let connections_to_update = MODEL.pipeline_item_pipeline_item_connections.concat(MODEL.pipeline_item_storage_cell_connections).filter(
				connection => connection.from == pipeline_item || connection.to == pipeline_item
			);
			
			for(let conn of connections_to_update) {
				conn.update();
			}
		};
		
		pipeline_item.updateConnections();
	},
	methods : {
		editPipelineItemSettings : function() {
			MODEL.setting_pipeline_item = this.$props.pipeline_item;
		},
		editDetectorSettings : function(detector) {
			MODEL.setting_detector = detector;
		}
	},
	template : `<div>
					<div>
						<img v-for="detector of pipeline_item.detectors" v-bind:src="detector.prefab.image_url" v-on:click="editDetectorSettings(detector)"
							width="30" height="30" style="display: inline-block"></img>
					</div>
					<canvas ref="canvas" width="50" height="50" v-on:click="editPipelineItemSettings"></canvas>
				</div>`
});

Vue.component("storage_cell_visualizer", {
	props : [ "storage_cell" ],
	mounted : function () {
		let storage_cell = this.$props.storage_cell;
		
		let canvas = this.$refs.canvas;
		let ctx = canvas.getContext('2d');
		let image = new Image();
		
		image.onload = () => {
			ctx.clearRect(0, 0, canvas.width, canvas.height);
			ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
			
			if(storage_cell.prefab.input_x && storage_cell.prefab.input_y) {
				ctx.fillStyle = "green";
				ctx.beginPath();
				ctx.arc(storage_cell.prefab.input_x * canvas.width, storage_cell.prefab.input_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
			
			if(storage_cell.prefab.output_x && storage_cell.prefab.output_y) {
				ctx.fillStyle = "red";
				ctx.beginPath();
				ctx.arc(storage_cell.prefab.output_x * canvas.width, storage_cell.prefab.output_y * canvas.height, 6, 0, 2 * Math.PI, false);
				ctx.fill();
			}
		};
		
		image.src = storage_cell.prefab.image_url;
		
		let rect = plan_image.getBoundingClientRect();
		
		canvas.parentNode.style.left = storage_cell.x * plan_image.width - STORAGE_CELL_SIZE / 2 + rect.left;
		canvas.parentNode.style.top = storage_cell.y * plan_image.height - STORAGE_CELL_SIZE / 2 + rect.top + window.scrollY;
		
		storage_cell.control = canvas.parentNode;
		storage_cell.canvas = canvas;
		
		storage_cell.updateConnections = function() {
			let connections_to_update = MODEL.pipeline_item_storage_cell_connections.filter(
				connection => connection.from == storage_cell || connection.to == storage_cell
			);
			
			for(let conn of connections_to_update) {
				conn.update();
			}
		}
		
		storage_cell.updateConnections();
	},
	template : `<canvas ref="canvas" width="50" height="50" ></canvas>`
});

Vue.component("connection_visualizer", {
	props : [ "connection" ],
	mounted : function () {
		let connection = this.$props.connection;
		
		connection.update = () => {
			let canvas = this.$refs.canvas;
			
			let fromControlBounds = connection.from.canvas.getBoundingClientRect();
			let toControlBounds = connection.to.canvas.getBoundingClientRect();
			
			let globalFromX = fromControlBounds.width * connection.from.prefab.output_x + fromControlBounds.left;
			let globalFromY = fromControlBounds.height * connection.from.prefab.output_y + fromControlBounds.top + window.scrollY;
			
			let globalToX = toControlBounds.width * connection.to.prefab.input_x + toControlBounds.left;
			let globalToY = toControlBounds.height * connection.to.prefab.input_y + toControlBounds.top + window.scrollY;
			
			canvas.width = Math.abs(globalToX - globalFromX) + 10;
			canvas.height = Math.abs(globalToY - globalFromY) + 10;
			
			canvas.parentNode.style.left = Math.min(globalFromX, globalToX) - 5;
			canvas.parentNode.style.top = Math.min(globalFromY, globalToY) - 5;
			
			let fromx = globalFromX > globalToX ? canvas.width - 5 : 5;
			let fromy = globalFromY > globalToY ? canvas.height - 5 : 5;
			
			let tox = globalFromX > globalToX ? 5 : canvas.width - 5;
			let toy = globalFromY > globalToY ? 5 : canvas.height - 5;
			
			let ctx = canvas.getContext('2d');
		
			let headlen = 10;
			let angle = Math.atan2(toy - fromy, tox - fromx);
			
			ctx.strokeStyle = "red";
			ctx.lineWidth = 3;
			
			ctx.beginPath();
			ctx.moveTo(fromx, fromy);
			ctx.lineTo(tox, toy);
			ctx.stroke();
			ctx.closePath();
			
			ctx.beginPath();
			ctx.moveTo(tox, toy);
			ctx.lineTo(tox - headlen * Math.cos(angle - Math.PI / 7), toy - headlen * Math.sin(angle - Math.PI / 7));
			ctx.stroke();
			ctx.closePath();
			
			ctx.beginPath();
			ctx.moveTo(tox, toy);
			ctx.lineTo(tox - headlen * Math.cos(angle + Math.PI / 7), toy - headlen * Math.sin(angle + Math.PI / 7));
			ctx.stroke();
			ctx.closePath();
		};
		
		connection.update.bind(this);
		connection.update();
	},
	template : `<canvas ref="canvas"></canvas>`
});

new Vue({
	el : "#pipeline",
	data : MODEL
});

let plan_image = document.getElementById("plan");

function getMousePosition(canvas, event) {
	let rect = canvas.getBoundingClientRect();
	let x = event.clientX - rect.left;
	let y = event.clientY - rect.top;
	return { x: x, y: y };
}

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.image_url = response.data.plan_image_url;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Company"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function planLoad() {
	let getPipelineRequest = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Pipeline/Get",
		data : JSON.stringify({ data : { id : MODEL.pipeline_id } }),
		dataType : "json",
		success : function(response) {
			MODEL.pipeline_items = response.data.connections.map(conn => conn.pipeline_item);
			MODEL.storage_cells = response.data.connections.reduce((acc, conn) => acc.concat(conn.input_storage_cells), []).concat(
				response.data.connections.reduce((acc, conn) => acc.concat(conn.output_storage_cells), [])
			).filter((v, i, a) => a.indexOf(v) === i);
			
			for(let connection of response.data.connections) {
				for(let input_pipeline_item of connection.input_pipeline_items) {
					if(MODEL.pipeline_item_pipeline_item_connections.find(conn => conn.from == input_pipeline_item && conn.to == connection.pipeline_item)) {
						return;
					}
					
					MODEL.pipeline_item_pipeline_item_connections.push({
						from : input_pipeline_item,
						to : connection.pipeline_item
					});
				}
				
				for(let output_pipeline_item of connection.output_pipeline_items) {
					if(MODEL.pipeline_item_pipeline_item_connections.find(conn => conn.to == output_pipeline_item && conn.from == connection.pipeline_item)) {
						return;
					}
					
					MODEL.pipeline_item_pipeline_item_connections.push({
						from : connection.pipeline_item,
						to : output_pipeline_item
					});
				}
				
				for(let input_storage_cell of connection.input_storage_cells) {
					if(MODEL.pipeline_item_storage_cell_connections.find(conn => conn.from == input_storage_cell && conn.to == connection.pipeline_item)) {
						return;
					}
					
					MODEL.pipeline_item_storage_cell_connections.push({
						from : input_storage_cell,
						to : connection.pipeline_item
					});
				}
				
				for(let output_storage_cell of connection.output_storage_cells) {
					if(MODEL.pipeline_item_storage_cell_connections.find(conn => conn.to == output_storage_cell && conn.from == connection.pipeline_item)) {
						return;
					}
					
					MODEL.pipeline_item_storage_cell_connections.push({
						from : connection.pipeline_item,
						to : output_storage_cell
					});
				}
			}
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipeline"});
		}
	};

	SESSION.putToAjaxRequest(getPipelineRequest);
	$.ajax(getPipelineRequest);
}