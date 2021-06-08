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
	
	faults : [],
	is_fault_window_opened : false,
	
	setting_pipeline_item : undefined,
	setting_detector : undefined,
	
	getDetectorDataTimer : undefined,
	
	closeFaultsWindow : function() {
		MODEL.is_fault_window_opened = false;
	},
	
	getValue : function(setting_prefab, pipeline_item) {
		let setting_value = pipeline_item.settings_values.find(
			v => v.prefab.id == setting_prefab.id
		)
		
		if(!setting_value) {
			return setting_value;
		}
		
		let data_type = setting_prefab.data_type ? setting_prefab.data_type : setting_prefab.option_data_type_id;
		
		if(data_type.name == 'bool') {
			return atob(setting_value.value_base64) == "true";
		}
		
		return atob(setting_value.value_base64);
	},
	
	setValue : function(e, setting_prefab, pipeline_item) {
		let setting_value = pipeline_item.settings_values.find(
			v => v.prefab.id == setting_prefab.id
		);
		
		let data_type = setting_prefab.data_type ? setting_prefab.data_type : setting_prefab.option_data_type_id;
		let value = data_type.name == 'bool' ? (e.target.checked ? "true" : "false") : e.target.value;
		
		if(setting_value) {
			setting_value.value_base64 = btoa(value);
		} else {
			pipeline_item.settings_values.push({
				prefab : setting_prefab,
				value_base64 : btoa(value)
			});
		}
	}
};

Vue.component("pipeline_item_visualizer", {
	props : [ "pipeline_item", "timer" ],
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
		
		this.$props.timer = setInterval(function() {
			for(let detector of pipeline_item.detectors) {
				let request = {
					type :  "POST",
					contentType : "application/json",
					url : "https://workautomatorback.azurewebsites.net/api/Detector/GetActualFaults",
					data : JSON.stringify({ data : { id : detector.id } }),
					dataType : "json",
					success : function(response) {
						let modelFaults = MODEL.faults.filter(f => f.detector.id == detector.id);
						let toAdd = [], toRemove = [];
						
						for(let fault of response.data) {
							if(!modelFaults.find(f => f.id == fault.id)) {
								toAdd.push(fault);
							}
						}
						
						for(let fault of modelFaults) {
							if(!response.data.find(f => f.id == fault.id)) {
								toRemove.push(fault);
							}
						}
						
						for(let f of toAdd) {
							MODEL.faults.push(f);
						}
						
						for(let f of toRemove) {
							MODEL.faults.splice(
								MODEL.faults.indexOf(f), 1
							);
						}
						
						if(toAdd.length != 0) {
							MODEL.is_fault_window_opened = true;
						}
					},
					error : function(response) {
						throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Actual Detector Faults"});
					}
				};

				SESSION.putToAjaxRequest(request);
				$.ajax(request);
			}
		}, 5000);
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
						<img v-for="detector of pipeline_item.detectors" style="display: inline-block" v-bind:src="detector.prefab.image_url" 
							v-on:click="editDetectorSettings(detector)" class="selectedItem" width="30" height="30"></img>
					</div>
					<canvas ref="canvas" width="50" height="50" v-on:click="editPipelineItemSettings" class="selectedItem"></canvas>
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

Vue.component("pipeline_item_settings_window", {
	props : [ "pipeline_item" ],
	methods : {
		savePipelineItemSettings : function() {
			let dto = {
				data : {
					id : this.$props.pipeline_item.id,
					seetings_values : this.$props.pipeline_item.settings_values.map(v => {
						return {
							prefab_id : v.prefab.id,
							value_base64 : v.value_base64
						}
					})
				}
			};
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/PipelineItem/SetupSettings",
				data : JSON.stringify(dto),
				dataType : "json",
				success : function(response) {
					MODEL.setting_pipeline_item = undefined;
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Set Pipeline Item Settings"});
				}
			};

			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		},
		
		cancelPipelineItemSettings : function() {
			MODEL.setting_pipeline_item = undefined;
		}
	},
	template : 
	`<div class="modal" v-if="pipeline_item">
	  <div class="modal-content">
		<center><span>Edit {{ pipeline_item.prefab.name }} #{{ pipeline_item.id }} settings</span></center>
		
		<div v-for="setting_prefab of pipeline_item.prefab.settings_prefabs" style="margin-bottom: 1em;">
			<label>
				<b>{{ setting_prefab.option_name }}</b>
				<span v-if="setting_prefab.description"><i>({{ setting_prefab.description }})</i></span>
			</label>
			<div class="wrap-input100 validate-input">
				<input class="input100" type="checkbox" v-if="setting_prefab.data_type.name == 'bool'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:checked="$root.getValue(setting_prefab, pipeline_item)" 
					   v-on:change="$root.setValue($event, setting_prefab, pipeline_item)"></input>
					   
				<input class="input100" type="number" v-if="setting_prefab.data_type.name == 'int'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:value="$root.getValue(setting_prefab, pipeline_item)" 
					   v-on:input="$root.setValue($event, setting_prefab, pipeline_item)"></input>
					   
				<input class="input100" type="number" step="0.001" v-if="setting_prefab.data_type.name == 'float'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:value="$root.getValue(setting_prefab, pipeline_item)" 
					   v-on:input="$root.setValue($event, setting_prefab, pipeline_item)"> </input>
			</div>
		</div>
		
		<center>
			<div class="common-button-container" style="display: inline-block; width: 40%;">
				<button v-on:click="savePipelineItemSettings" class="common-button">
					Save
				</button>
			</div>
			
			<div class="common-button-container" style="display: inline-block; width: 40%;">
				<button v-on:click="cancelPipelineItemSettings" class="common-button">
					Cancel
				</button>
			</div>
		</center>
		</div>
	</div>`
});

Vue.component("detector_window", {
	props : [ "detector", "timer" ],
	watch: {
		detector: function (newDetector, oldDetector) {
			if(!newDetector) {
				return;
			}
			
			let self = this;
			let dateEnd = new Date(new Date() - 10800000);
			let dateStart = new Date(dateEnd - 86400000);
			
			let getDataThisDayDto = { 
				data : { 
					id : newDetector.id,
					date_since : formatDate(dateStart),
					date_until : formatDate(dateEnd)
				}
			}
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Detector/GetData",
				data : JSON.stringify(getDataThisDayDto),
				dataType : "json",
				success : function(response) {
					for(let data_prefab of newDetector.prefab.detector_data_prefabs) {
						let lineChart = undefined;
						let database64 = response.data.data.filter(d => d.data_prefab.id == data_prefab.id);
						
						let data = data_prefab.field_data_type.name == "bool" ? database64.map(d => { 
							return {
								x : new Date(d.timespan) - dateStart,
								y : atob(d.data_base64) == "true" ? 1 : 0
							}
						}) : database64.map(d => {
							return {
								x : new Date(d.timespan) - dateStart,
								y : parseFloat(atob(d.data_base64))
							}
						})
						
						if(data_prefab.visualizer_type.name == "SIGNAL") {
							let canvas = document.getElementById("data_canvas_" + data_prefab.id);
							
							lineChart = new Chart(canvas, {
								type: 'line',
								data: {
									labels: data.map(d => undefined),
									datasets: [{
										label: "#" + data_prefab.id + " " + data_prefab.field_name,
										data: data
									}]
								},
								options: {
									responsive: true,
									legend: {
										display: true,
										position: 'top',
										labels: {
											boxWidth: 80,
											fontColor: 'black'
										}
									},
									scales: {
										xAxis: {
											title: data_prefab.argument_name
										}
									}
								}
							});
						} else {
							let data_field = document.getElementById("data_field_" + data_prefab.id);
							data_field.innerHTML = data[data.length - 1].y;
						}
						
						self.$props.timer = setInterval(function() {
							let getTimedDataDto = { 
								data : { 
									id : newDetector.id,
									date_since : getDataThisDayDto.data.date_until,
									date_until : formatDate(new Date(new Date() - 10800000))
								}
							}
							
							let timerRequest = {
								type :  "POST",
								contentType : "application/json",
								url : "https://workautomatorback.azurewebsites.net/api/Detector/GetData",
								data : JSON.stringify(getTimedDataDto),
								dataType : "json",
								success : function(response) {
									getDataThisDayDto = getTimedDataDto;
									for(let data_prefab of newDetector.prefab.detector_data_prefabs) {
										let database64 = response.data.data.filter(d => d.data_prefab.id == data_prefab.id);
										
										let data = data_prefab.field_data_type.name == "bool" ? database64.map(d => { 
											return {
												x : new Date(d.timespan) - dateStart,
												y : atob(d.data_base64) == "true" ? 1 : 0
											}
										}) : database64.map(d => {
											return {
												x : new Date(d.timespan) - dateStart,
												y : parseFloat(atob(d.data_base64))
											}
										})
										
										if(data_prefab.visualizer_type.name == "SIGNAL") {
											lineChart.data.labels.push(...data.map(d => undefined));
											lineChart.data.datasets[0].data.push(...data);
											lineChart.update('active');
										} else {
											let data_field = document.getElementById("data_field_" + data_prefab.id);
											data_field.innerHTML = data[data.length - 1].y;
										}
									}
								},
								error : function(response) {
									throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detector Data"});
								}
							};
							
							SESSION.putToAjaxRequest(timerRequest);
							$.ajax(timerRequest);
						}, 5000);
					}		
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Detector Data"});
				}
			};

			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		}
	},
	methods : {
		saveDetectorSettings : function() {
			let dto = {
				data : {
					id : this.$props.detector.id,
					settings_values : this.$props.detector.settings_values.map(v => {
						return {
							prefab_id : v.prefab.id,
							value_base64 : v.value_base64
						}
					})
				}
			};
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Detector/SetupSettings",
				data : JSON.stringify(dto),
				dataType : "json",
				success : function(response) {
					MODEL.setting_detector = undefined;
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Set Detector Settings"});
				}
			};

			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		},
		
		cancelDetectorSettings : function() {
			MODEL.setting_detector = undefined;
		},
		
		getDataCanvasId : function(data_prefab) {
			return "data_canvas_" + data_prefab.id;
		},
		
		getDataFieldId : function(data_prefab) {
			return "data_field_" + data_prefab.id;
		}
	},
	template : 
	`<div class="modal" v-if="detector">
	  <div class="modal-content">
		<center><span>Edit {{ detector.prefab.name }} #{{ detector.id }} settings</span></center>
		
		<div v-for="setting_prefab of detector.prefab.detector_settings_prefabs" style="margin-bottom: 1em;">
			<label>
				<b>{{ setting_prefab.option_name }}</b>
				<span v-if="setting_prefab.description"><i>({{ setting_prefab.description }})</i></span>
			</label>
			<div class="wrap-input100 validate-input">
				<input class="input100" type="checkbox" v-if="setting_prefab.option_data_type_id.name == 'bool'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:checked="$.root.getValue(setting_prefab, detector)" 
					   v-on:input="$root.setValue($event, setting_prefab, detector)"></input>
					   
				<input class="input100" type="number" v-if="setting_prefab.option_data_type_id.name == 'int'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:value="$root.getValue(setting_prefab, detector)" 
					   v-on:input="$root.setValue($event, setting_prefab, detector)"></input>
					   
				<input class="input100" type="number" step="0.001" v-if="setting_prefab.option_data_type_id.name == 'float'" 
					   v-bind:placeholder="setting_prefab.option_name" v-bind:value="$root.getValue(setting_prefab, detector)" 
					   v-on:input="$root.setValue($event, setting_prefab, detector)"></input>
			</div>
		</div>
		
		<center>
			<div class="common-button-container" style="display: inline-block; width: 40%;">
				<button v-on:click="saveDetectorSettings" class="common-button">
					Save
				</button>
			</div>
			
			<div class="common-button-container" style="display: inline-block; width: 40%;">
				<button v-on:click="cancelDetectorSettings" class="common-button">
					Cancel
				</button>
			</div>
		</center>
		
		<div class="listItemContainer" style="width: 95%; height: 20em;" v-for="data_prefab of detector.prefab.detector_data_prefabs">
			<canvas v-if="data_prefab.visualizer_type.name == 'SIGNAL'" v-bind:id="getDataCanvasId(data_prefab)"></canvas>
			
			<span v-if="data_prefab.visualizer_type.name == 'VALUE'">
				<b>{{ data_prefab.option_name }}</b>: <i v-bind:id="getDataFieldId(data_prefab)"></i>
			</span>
		</div>
		
	  </div>
	</div>`
});

Vue.component("fault_window", {
	props : [ "faults" ],
	methods : {
		chunk : function(items, length) {
			let res = [];
			for (let i = 0; i < items.length; i += length) {
				const chunk = items.slice(i, i + length);
				res.push(chunk);
			}
			return res;
		}
	},
	template : 
	`<div class="modal" v-if="$root.is_fault_window_opened">
	  <div class="modal-content">
		<center><span>FAULTS!!!</span></center>
		
		<center>
			<table>
				<tr v-for="faults_chunk of chunk(faults, 5)">
					<td v-for="fault of faults_chunk">
						
						<span><b>{{ fault.fault.name }}</b> at <b>{{ fault.detector.prefab.name }} #{{ fault.detector.id }}</b></span>
						<img v-bind:src="fault.fault.image_url" width="50" height="50"></img>
						
					</td>
				</tr>
			</table>
		</center>
		
		<center>
			<div class="common-button-container" style="display: inline-block; width: 40%;">
				<button v-on:click="$root.closeFaultsWindow" class="common-button">
					OK
				</button>
			</div>
		</center>
		</div>
	</div>`
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

function formatDate(date) {
	let month = date.getMonth() + 1;
	let day = date.getDate();
	let hour = date.getHours();
	let minute = date.getMinutes();
	let seconds = date.getSeconds();
	
	return (date.getYear() + 1900) + "-" + 
			(month < 10 ? "0" + month : month)+ "-" + 
			(day < 10 ? "0" + day : day)+ "T" + 
			(hour < 10 ? "0" + hour : hour) + ":" + 
			(minute < 10 ? "0" + minute : minute) + ":" +
			(seconds < 10 ? "0" + seconds : seconds) + "Z";
}

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