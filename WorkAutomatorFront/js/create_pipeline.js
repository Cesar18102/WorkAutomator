const PIPELINE_ITEM_SIZE = 50;
const STORAGE_CELL_SIZE = 50;
const DETECTOR_SIZE = 40;
const FAULT_SIZE = 30;

var MODEL = {
	pipeline_id: new URLSearchParams(window.location.search).get('id'),
	image_url: undefined,
	
	pipeline_item_prefabs : [],
	pipeline_items : [],
	
	storage_cell_prefabs : [],
	storage_cells : [],
	
	detector_prefabs : [],
	detectors : [],
	
	pipeline_item_pipeline_item_connections : [],
	pipeline_item_storage_cell_connections : [],
	
	selected_pipeline_item : undefined,
	selected_storage_cell : undefined,
	
	is_setting_outgoing_connection : false,
	is_setting_incoming_connection : false,
	
	pipeline_item_setting_connection_source : undefined,
	storage_cell_setting_connection_source : undefined,
	
	remove_connection_item : undefined,
	is_removing_connection : false,
	
	is_mouse_pressed : false,
	
	creating_detector_prefab : undefined,
	is_tracked_detector_fault_prefabs_window_shown : false,
	
	closeTrackedFaultPrefabsWindow : function() {
		MODEL.is_tracked_detector_fault_prefabs_window_shown = false;
	},
	
	createDetectorPrefabConfirm : function() {
		let newDetectorDto = {
			data : {
				prefab_id : MODEL.creating_detector_prefab.id,
				tracked_detector_fault_ids : MODEL.creating_detector_prefab.detector_fault_prefabs
					.filter(fault_prefab => fault_prefab.is_tracked)
					.map(fault_prefab => { 
						return { id : fault_prefab.id };
					})
			}
		}
		
		let newDetectorRequest = {
			type :  "POST",
			contentType : "application/json",
			url : "https://workautomatorback.azurewebsites.net/api/Detector/Create",
			data : JSON.stringify(newDetectorDto),
			dataType : "json",
			success : function(response) {
				MODEL.detectors.push(response.data);
			},
			error : function(response) {
				throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Create Detector"});
			}
		};
		
		SESSION.putToAjaxRequest(newDetectorRequest);
		$.ajax(newDetectorRequest);
	},
	
	resetSelection : function() {
		if(MODEL.selected_pipeline_item) {
			MODEL.selected_pipeline_item.is_moving = false;
			MODEL.selected_pipeline_item = undefined;
		}
		
		if(MODEL.selected_storage_cell) {
			MODEL.selected_storage_cell.is_moving = false;
			MODEL.selected_storage_cell = undefined;
		}
		
		MODEL.pipeline_item_setting_connection_source = undefined;
		MODEL.storage_cell_setting_connection_source = undefined;
		
		MODEL.is_setting_incoming_connection = false;
		MODEL.is_setting_outgoing_connection = false;
		
		MODEL.remove_connection_item = undefined;
		MODEL.is_removing_connection = false;
	},
	
	setOutgoingConnection : function() {
		if(MODEL.is_setting_incoming_connection || MODEL.is_setting_outgoing_connection) {
			return;
		}
		
		if(MODEL.selected_pipeline_item) {
			MODEL.pipeline_item_setting_connection_source = MODEL.selected_pipeline_item;
			MODEL.selected_pipeline_item = undefined;
			MODEL.is_setting_outgoing_connection = true;
		} else if(MODEL.selected_storage_cell) {
			MODEL.storage_cell_setting_connection_source = MODEL.selected_storage_cell;
			MODEL.selected_storage_cell = undefined;
			MODEL.is_setting_outgoing_connection = true;
		}
	},
	
	setIncomingConnection : function() {
		if(MODEL.is_setting_incoming_connection || MODEL.is_setting_outgoing_connection) {
			return;
		}
		
		if(MODEL.selected_pipeline_item) {
			MODEL.pipeline_item_setting_connection_source = MODEL.selected_pipeline_item;
			MODEL.selected_pipeline_item = undefined;
			MODEL.is_setting_incoming_connection = true;
		} else if(MODEL.selected_storage_cell) {
			MODEL.storage_cell_setting_connection_source = MODEL.selected_storage_cell;
			MODEL.selected_storage_cell = undefined;
			MODEL.is_setting_incoming_connection = true;
		}
	},
	
	startRemoveConnection : function() {
		if(MODEL.selected_pipeline_item) {
			MODEL.remove_connection_item = MODEL.selected_pipeline_item;
			MODEL.selected_pipeline_item = undefined;
			MODEL.is_removing_connection = true;
		} else if(MODEL.selected_storage_cell) {
			MODEL.remove_connection_item = MODEL.selected_storage_cell;
			MODEL.selected_storage_cell = undefined;
			MODEL.is_removing_connection = true;
		}
	},
	
	tryRemoveConnection : function(item) {
		if(!MODEL.remove_connection_item || !MODEL.is_removing_connection) {
			return false;
		}
		
		let pipeline_item_connections_to_remove = MODEL.pipeline_item_pipeline_item_connections.filter(
			conn => (conn.from == item || conn.to == item) && 
					(conn.from == MODEL.remove_connection_item || conn.to == MODEL.remove_connection_item)
		);
		
		for(let connection of pipeline_item_connections_to_remove) {
			MODEL.pipeline_item_pipeline_item_connections.splice(
				MODEL.pipeline_item_pipeline_item_connections.indexOf(connection), 1
			);
		}
		
		let storage_cell_connections_to_remove = MODEL.pipeline_item_storage_cell_connections.filter(
			conn => (conn.from == item || conn.to == item) && 
					(conn.from == MODEL.remove_connection_item || conn.to == MODEL.remove_connection_item)
		);
		
		for(let connection of storage_cell_connections_to_remove) {
			MODEL.pipeline_item_storage_cell_connections.splice(
				MODEL.pipeline_item_storage_cell_connections.indexOf(connection), 1
			);
		}
	},
	
	tryFinishConnection : function(pipeline_item, storage_cell) {
		if(!MODEL.is_setting_incoming_connection && !MODEL.is_setting_outgoing_connection) {
			return false;
		}
		
		let source = {
			pipeline_item : MODEL.pipeline_item_setting_connection_source,
			storage_cell : MODEL.storage_cell_setting_connection_source
		}
		
		let target = {
			pipeline_item : pipeline_item,
			storage_cell : storage_cell
		}
		
		if((source.pipeline_item && source.pipeline_item == target.pipeline_item) || (source.storage_cell && source.storage_cell == target.storage_cell)) {
			throw JSON.stringify({ex: { message : "Item cannot be connected with itself" }, source: "Create Connection"});
		}
		
		if(MODEL.is_setting_incoming_connection) {
			let temp = target;
			target = source;
			source = temp;
		}
		
		if(source.storage_cell && target.storage_cell) {
			throw JSON.stringify({ex: { message : "Storage Cells Cannot Be Connected" }, source: "Create Connection"});
		}
		
		if(source.pipeline_item && target.pipeline_item) {
			if(MODEL.pipeline_item_pipeline_item_connections.find(conn => conn.from == source.pipeline_item && conn.to == target.pipeline_item)) {
				throw JSON.stringify({ex: { message : "Connection already exists" }, source: "Create Connection"});
			}
			
			MODEL.pipeline_item_pipeline_item_connections.push({
				from : source.pipeline_item,
				to : target.pipeline_item
			});
			
			return true;
		} else {
			let connection = {};
			
			if(source.pipeline_item) {
				connection.from = source.pipeline_item;
			} else {
				connection.from = source.storage_cell;
			}
			
			if(target.pipeline_item) {
				connection.to = target.pipeline_item;
			} else {
				connection.to = target.storage_cell;
			}
			
			if(MODEL.pipeline_item_storage_cell_connections.find(conn => conn.from == connection.from && conn.to == connection.to)) {
				throw JSON.stringify({ex: { message : "Connection already exists" }, source: "Create Connection"});
			}
			
			MODEL.pipeline_item_storage_cell_connections.push(connection);
			
			return true;
		}
	},
	
	remove : function() {
		let target = undefined;
		
		if(MODEL.selected_pipeline_item) {
			MODEL.selected_pipeline_item.x = undefined;
			MODEL.selected_pipeline_item.y = undefined;
			MODEL.selected_pipeline_item.control = undefined;
			
			let pipeline_item_pipeline_item_connections_to_remove = MODEL.pipeline_item_pipeline_item_connections.filter(
				connection => connection.from == MODEL.selected_pipeline_item || connection.to == MODEL.selected_pipeline_item
			);
			
			for(let connection of pipeline_item_pipeline_item_connections_to_remove) {
				MODEL.pipeline_item_pipeline_item_connections.splice(
					MODEL.pipeline_item_pipeline_item_connections.indexOf(connection), 1
				);
			}
			
			target = MODEL.selected_pipeline_item;
			MODEL.selected_pipeline_item = undefined;
		} else if(MODEL.selected_storage_cell) {
			MODEL.selected_storage_cell.x = undefined;
			MODEL.selected_storage_cell.y = undefined;
			MODEL.selected_storage_cell.control = undefined;
			
			target = MODEL.selected_storage_cell;
			MODEL.selected_storage_cell = undefined;
		}
		
		let pipeline_item_storage_cell_connections_to_remove = MODEL.pipeline_item_storage_cell_connections.filter(
			connection => connection.from == target || connection.to == target
		);
		
		for(let connection of pipeline_item_storage_cell_connections_to_remove) {
			MODEL.pipeline_item_storage_cell_connections.splice(
				MODEL.pipeline_item_storage_cell_connections.indexOf(connection), 1
			);
		}
	},
	
	save : function() {
		let dto = {
			data : {
				pipeline_item_placemetns : MODEL.pipeline_items.filter(pipeline_item => pipeline_item.x).map(pipeline_item => {
					return {
						id : pipeline_item.id,
						x : pipeline_item.x,
						y : pipeline_item.y
					}
				}),
				storage_cell_placemetns : MODEL.storage_cells.filter(storage_cell => storage_cell.x).map(storage_cell => {
					return {
						id : storage_cell.id,
						x : storage_cell.x,
						y : storage_cell.y
					}
				}),
				connections : MODEL.pipeline_items.filter(pipeline_item => pipeline_item.x).map(pipeline_item => {
					return {
						pipeline_item_id : pipeline_item.id,
						input_pipeline_items : MODEL.pipeline_item_pipeline_item_connections.filter(conn => conn.to == pipeline_item).map(conn => { return { id : conn.from.id } }),
						output_pipeline_items : MODEL.pipeline_item_pipeline_item_connections.filter(conn => conn.from == pipeline_item).map(conn => { return { id : conn.to.id } }),
						input_storage_cells : MODEL.pipeline_item_storage_cell_connections.filter(conn => conn.to == pipeline_item).map(conn => { return { id : conn.from.id } }),
						output_storage_cells : MODEL.pipeline_item_storage_cell_connections.filter(conn => conn.from == pipeline_item).map(conn => { return { id : conn.to.id } })
					}
				})
			}
		}
		
		if(MODEL.pipeline_id) {
			dto.data.id = MODEL.pipeline_id;
		} else {
			dto.data.company_id = SESSION.getCompanyId();
		}
		
		let createPipelineRequest = {
			type :  "POST",
			contentType : "application/json",
			url : "https://workautomatorback.azurewebsites.net/api/Pipeline/" + (MODEL.pipeline_id ? "Update" : "Create"),
			data : JSON.stringify(dto),
			dataType : "json",
			success : function(response) {
				let countSetup = 0;
				let hasError = false;
				
				let placed_detectors = MODEL.detectors.filter(detector => detector.pipeline_item);
				for(let placed_detector of placed_detectors) {
					let setupDetectorDto = {
						data : {
							pipeline_item_id : placed_detector.pipeline_item.id,
							detector_id : placed_detector.id
						}
					};
					
					let setupDetectorRequest = {
						type :  "POST",
						contentType : "application/json",
						url : "https://workautomatorback.azurewebsites.net/api/PipelineItem/SetupDetector",
						data : JSON.stringify(setupDetectorDto),
						dataType : "json",
						success : function(response) {
							countSetup++;
							
							if(countSetup == placed_detectors) {
								document.location = "../pages/pipelines.html";
							}
						},
						error : function(response) {
							if(!hasError) {
								hasError = true;
								throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Setup Detector"});
							}
						}
					};
					
					SESSION.putToAjaxRequest(setupDetectorRequest);
					$.ajax(setupDetectorRequest);
				}
			},
			error : function(response) {
				throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Save Pipeline"});
			}
		};

		SESSION.putToAjaxRequest(createPipelineRequest);
		$.ajax(createPipelineRequest);
	}
};

Vue.component("pipeline_item_prefab_list_item", {
	props : [ "pipeline_item_prefab" ],
	methods : {
		newPipelineItem : function() {
			let newPipelineItemRequest = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/PipelineItem/Create",
				data : JSON.stringify({ data : { prefab_id : this.$props.pipeline_item_prefab.id } }),
				dataType : "json",
				success : function(response) {
					MODEL.pipeline_items.push(response.data);
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Create Pipeline Item"});
				}
			};
			
			SESSION.putToAjaxRequest(newPipelineItemRequest);
			$.ajax(newPipelineItemRequest);
		}
	},
	template : 
	`<div> 
		<img v-bind:src="pipeline_item_prefab.image_url" width="40" height="40"></img>
		<span> {{ pipeline_item_prefab.name }} </span>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button" v-on:click="newPipelineItem">New {{ pipeline_item_prefab.name }}</button>
		</div>
	</div>`
});

Vue.component("storage_cell_prefab_list_item", {
	props : [ "storage_cell_prefab" ],
	methods : {
		newStorageCell : function() {
			let newStorageCellRequest = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/StorageCell/Create",
				data : JSON.stringify({ data : { prefab_id : this.$props.storage_cell_prefab.id } }),
				dataType : "json",
				success : function(response) {
					MODEL.storage_cells.push(response.data);
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Create Storage Cell"});
				}
			};
			
			SESSION.putToAjaxRequest(newStorageCellRequest);
			$.ajax(newStorageCellRequest);
		}
	},
	template : 
	`<div> 
		<img v-bind:src="storage_cell_prefab.image_url" width="40" height="40"></img>
		<span> {{ storage_cell_prefab.name }} </span>
		<div class="common-button-container" style="display: inline-block;">
			<button class="common-button" v-on:click="newStorageCell">New {{ storage_cell_prefab.name }}</button>
		</div>
	</div>`
});

Vue.component("detector_prefab_list_item", {
	props : [ "detector_prefab" ],
	methods : {
		newDetector : function() {
			MODEL.creating_detector_prefab = this.$props.detector_prefab;
			
			if(MODEL.creating_detector_prefab.detector_fault_prefabs.length != 0) {
				MODEL.is_tracked_detector_fault_prefabs_window_shown = true;
			}
		}
	},
	template : 
	`<div>
		<img v-bind:src="detector_prefab.image_url" width="30" height="30"></img>
		<span> {{ detector_prefab.name }} </span>
		<div class="common-button-container" style="display: inline-block">
			<button class="common-button" v-on:click="newDetector">New {{ detector_prefab.name }}</button>
		</div>
	</div>`
});

Vue.component("pipeline_item_list_item", {
	props : [ "pipeline_item" ],
	methods : {
		place : function() {
			this.$props.pipeline_item.x = 0.5;
			this.$props.pipeline_item.y = 0.5;
		}
	},
	computed : {
		isSelected : function() {
			return MODEL.selected_pipeline_item == this.$props.pipeline_item;
		}
	},
	template : 
	`<div> 
		<img width="40" height="40" v-bind:src="pipeline_item.prefab.image_url" v-bind:class="{ selectedItem : isSelected }"></img>
		<span> {{ pipeline_item.prefab.name }} #{{ pipeline_item.id }} </span>
		<div class="common-button-container" style="display: inline-block;"  v-if="pipeline_item.x == undefined">
			<button class="common-button" v-on:click="place">Place</button>
		</div>
	</div>`
});

Vue.component("storage_cell_list_item", {
	props : [ "storage_cell" ],
	methods : {
		place : function() {
			this.$props.storage_cell.x = 0.5;
			this.$props.storage_cell.y = 0.5;
		}
	},
	computed : {
		isSelected : function() {
			return MODEL.selected_storage_cell == this.$props.storage_cell;
		}
	},
	template : 
	`<div> 
		<img v-bind:src="storage_cell.prefab.image_url" width="40" height="40" v-bind:class="{ selectedItem : isSelected }"></img>
		<span> {{ storage_cell.prefab.name }} #{{ storage_cell.id }} </span>
		<div class="common-button-container" style="display: inline-block;" v-if="storage_cell.x == undefined">
			<button class="common-button" v-on:click="place">Place</button>
		</div>
	</div>`
});

Vue.component("detector_list_item", {
	props : [ "detector" ],
	methods : {
		place : function() {
			if(this.$props.detector.pipeline_item) {
				return;
			}
			
			MODEL.selected_pipeline_item.detectors.push(this.$props.detector);
			this.$props.detector.pipeline_item = MODEL.selected_pipeline_item;
			
			this.$props.detector.pipeline_item.updateConnections();
		},
		unplace : function() {
			this.$props.detector.pipeline_item.detectors.splice(
				this.$props.detector.pipeline_item.detectors.indexOf(this.$props.detector), 1
			);
			
			this.$props.detector.pipeline_item.updateConnections();
			this.$props.detector.pipeline_item = undefined;
		}
	},
	template : 
	`<div>
		<img v-bind:src="detector.prefab.image_url" width="30" height="30"></img>
		<span> {{ detector.prefab.name }} #{{ detector.id }} </span>
		<div class="common-button-container" style="display: inline-block;" v-if="$root.selected_pipeline_item && !detector.pipeline_item">
			<button class="common-button" v-on:click="place">Place on {{ $root.selected_pipeline_item.prefab.name }} #{{ $root.selected_pipeline_item.id }}</button>
		</div>
		<div class="common-button-container" style="display: inline-block;" v-if="detector.pipeline_item">
			<button class="common-button" v-on:click="unplace">Unplace from {{ $root.selected_pipeline_item.prefab.name }} #{{ $root.selected_pipeline_item.id }}</button>
		</div>
	</div>`
});

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
		select : function() {
			if(MODEL.tryFinishConnection(this.$props.pipeline_item, undefined) || MODEL.tryRemoveConnection(this.$props.pipeline_item)) {
				MODEL.resetSelection();
				return;
			}
			
			MODEL.resetSelection();
			MODEL.selected_pipeline_item = this.$props.pipeline_item;
		},
		startMoving : function() {
			this.$props.pipeline_item.is_moving = true;
			this.$props.pipeline_item.control.style.zIndex = 999;
		},
		endMoving : function() {
			this.$props.pipeline_item.is_moving = false;
			this.$props.pipeline_item.control.style.zIndex = 1;
		},
		move : function(e) {
			if(!this.$props.pipeline_item.is_moving) {
				return;
			}
			
			let rect = plan_image.getBoundingClientRect();
			
			let newX = parseInt(MODEL.selected_pipeline_item.control.style.left.replace("px", "")) + e.movementX;
			let newY = parseInt(MODEL.selected_pipeline_item.control.style.top.replace("px", "")) + e.movementY;
 			
			this.$props.pipeline_item.control.style.left = newX;
			this.$props.pipeline_item.control.style.top = newY;
			
			this.$props.pipeline_item.x = (newX - rect.left + PIPELINE_ITEM_SIZE / 2) / plan_image.width;
			this.$props.pipeline_item.y = (newY - rect.top + PIPELINE_ITEM_SIZE / 2) / plan_image.height;
			
			this.$props.pipeline_item.updateConnections();
		}
	},
	computed : {
		isSelected : function() {
			return MODEL.selected_pipeline_item == this.$props.pipeline_item;
		},
		isSettingOutgoingConnection : function() {
			return MODEL.pipeline_item_setting_connection_source == this.$props.pipeline_item && MODEL.is_setting_outgoing_connection;
		},
		isSettingIncomingConnection : function() {
			return MODEL.pipeline_item_setting_connection_source == this.$props.pipeline_item && MODEL.is_setting_incoming_connection;
		},
		isRemovingConnection : function() {
			return MODEL.remove_connection_item == this.$props.pipeline_item;
		}
	},
	template : `<div>
					<div>
						<img v-for="detector of pipeline_item.detectors" v-bind:src="detector.prefab.image_url" width="30" height="30" style="display: inline-block"
							 v-bind:class="{ selectedItem : isSelected, settingOutgoingConnection : isSettingOutgoingConnection, settingIncomingConnection : isSettingIncomingConnection, removeConnection : isRemovingConnection }"></img>
					</div>
					<canvas ref="canvas" width="50" height="50" 
							v-on:mousedown="startMoving" v-on:mouseup="endMoving" v-on:mousemove="move" v-on:click="select" 
							v-bind:class="{ selectedItem : isSelected, settingOutgoingConnection : isSettingOutgoingConnection, settingIncomingConnection : isSettingIncomingConnection, removeConnection : isRemovingConnection }">
					</canvas>
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
	methods : {
		select : function() {
			if(MODEL.tryFinishConnection(undefined, this.$props.storage_cell) || MODEL.tryRemoveConnection(this.$props.storage_cell)) {
				MODEL.resetSelection();
				return;
			}
			
			MODEL.resetSelection();
			MODEL.selected_storage_cell = this.$props.storage_cell;
		},
		startMoving : function() {
			this.$props.storage_cell.is_moving = true;
			this.$props.storage_cell.control.style.zIndex = 999;
		},
		endMoving : function() {
			this.$props.storage_cell.is_moving = false;
			this.$props.storage_cell.control.style.zIndex = 1;
			
		},
		move : function(e) {
			if(!this.$props.storage_cell.is_moving) {
				return;
			}
			
			let rect = plan_image.getBoundingClientRect();
			
			let newX = parseInt(MODEL.selected_storage_cell.control.style.left.replace("px", "")) + e.movementX;
			let newY = parseInt(MODEL.selected_storage_cell.control.style.top.replace("px", "")) + e.movementY;
 			
			this.$props.storage_cell.control.style.left = newX;
			this.$props.storage_cell.control.style.top = newY;
			
			this.$props.storage_cell.x = (newX - rect.left + STORAGE_CELL_SIZE / 2) / plan_image.width;
			this.$props.storage_cell.y = (newY - rect.top + STORAGE_CELL_SIZE / 2) / plan_image.height;
			
			this.$props.storage_cell.updateConnections();
		}
	},
	computed : {
		isSelected : function() {
			return MODEL.selected_storage_cell == this.$props.storage_cell;
		},
		isSettingOutgoingConnection : function() {
			return MODEL.storage_cell_setting_connection_source == this.$props.storage_cell && MODEL.is_setting_outgoing_connection;
		},
		isSettingIncomingConnection : function() {
			return MODEL.storage_cell_setting_connection_source == this.$props.storage_cell && MODEL.is_setting_incoming_connection;
		},
		isRemovingConnection : function() {
			return MODEL.remove_connection_item == this.$props.storage_cell;
		}
	},
	template : `<canvas ref="canvas" width="50" height="50" 
						v-on:mousedown="startMoving" v-on:mouseup="endMoving" v-on:mousemove="move" v-on:click="select"
						v-bind:class="{ selectedItem : isSelected, settingOutgoingConnection : isSettingOutgoingConnection, settingIncomingConnection : isSettingIncomingConnection, removeConnection : isRemovingConnection }">
				</canvas>`
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
		MODEL.image_url = response.data.plan_image_url,
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


async function planLoaded() {
	let pipelineItemsRequest = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/PipelineItem/Get",
		data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
		dataType : "json",
		success : function(response) {
			MODEL.pipeline_items = response.data.filter(pi => !pi.pipeline_id || pi.pipeline_id == MODEL.pipeline_id);
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Pipeline Items"});
		}
	};

	SESSION.putToAjaxRequest(pipelineItemsRequest);
	await $.ajax(pipelineItemsRequest);
	
	let storageCellsRequest = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/StorageCell/Get",
		data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
		dataType : "json",
		success : function(response) {
			MODEL.storage_cells = response.data.filter(pi => !pi.pipeline_id || pi.pipeline_id == MODEL.pipeline_id);
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Storage Cells"});
		}
	};

	SESSION.putToAjaxRequest(storageCellsRequest);
	await $.ajax(storageCellsRequest);
	
	let detectorsRequest = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Detector/Get",
		data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
		dataType : "json",
		success : function(response) {
			MODEL.detectors = response.data.filter(
				d => !d.pipeline_item_id || MODEL.pipeline_items.find(pi => d.pipeline_item_id == pi.id)
			).map(d => { 
				d.pipeline_item = MODEL.pipeline_items.find(pi => d.pipeline_item_id == pi.id); 
				return d; 
			});
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detectors"});
		}
	};

	SESSION.putToAjaxRequest(detectorsRequest);
	$.ajax(detectorsRequest);
	
	if(MODEL.pipeline_id) {
		let getPipelineRequest = {
			type :  "POST",
			contentType : "application/json",
			url : "https://workautomatorback.azurewebsites.net/api/Pipeline/Get",
			data : JSON.stringify({ data : { id : MODEL.pipeline_id } }),
			dataType : "json",
			success : function(response) {
				for(let connection of response.data.connections) {
					for(let input_pipeline_item of connection.input_pipeline_items) {
						if(MODEL.pipeline_item_pipeline_item_connections.find(conn => conn.from.id == input_pipeline_item.id && conn.to.id == connection.pipeline_item.id)) {
							return;
						}
						
						MODEL.pipeline_item_pipeline_item_connections.push({
							from : MODEL.pipeline_items.find(pi => pi.id == input_pipeline_item.id),
							to : MODEL.pipeline_items.find(pi => pi.id == connection.pipeline_item.id)
						});
					}
					
					for(let output_pipeline_item of connection.output_pipeline_items) {
						if(MODEL.pipeline_item_pipeline_item_connections.find(conn => conn.to.id == output_pipeline_item.id && conn.from.id == connection.pipeline_item.id)) {
							return;
						}
						
						MODEL.pipeline_item_pipeline_item_connections.push({
							from : MODEL.pipeline_items.find(pi => pi.id == connection.pipeline_item.id),
							to : MODEL.pipeline_items.find(pi => pi.id == output_pipeline_item.id)
						});
					}
					
					for(let input_storage_cell of connection.input_storage_cells) {
						if(MODEL.pipeline_item_storage_cell_connections.find(conn => conn.from.id == input_storage_cell.id && conn.to.id == connection.pipeline_item.id)) {
							return;
						}
						
						MODEL.pipeline_item_storage_cell_connections.push({
							from : MODEL.storage_cells.find(sc => sc.id == input_storage_cell.id),
							to : MODEL.pipeline_items.find(pi => pi.id == connection.pipeline_item.id)
						});
					}
					
					for(let output_storage_cell of connection.output_storage_cells) {
						if(MODEL.pipeline_item_storage_cell_connections.find(conn => conn.to == output_storage_cell && conn.from == connection.pipeline_item)) {
							return;
						}
						
						MODEL.pipeline_item_storage_cell_connections.push({
							from : MODEL.pipeline_items.find(pi => pi.id == connection.pipeline_item.id),
							to : MODEL.storage_cells.find(sc => sc.id == output_storage_cell.id)
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
}