const COMPANY_PLAN_POINT_RADIUS = 8;
const MANUFACTORY_PLAN_POINT_RADIUS = 6;
const CHECK_POINT_RADIUS = 4;

let COMPANY_PLAN_POINT_COUNTER = 1;

var MODEL = {
	company_id: undefined,
	company_name: undefined,
	
	image_url: undefined,
	company_plan_points: [],
	manufactories: [],
	check_points: [],
	enter_leave_points: [],
	
	current_manufactory: undefined,
	current_check_point: undefined,
	current_enter_leave_point: undefined
};

new Vue({
	el : "#savedData",
	data : MODEL
});

Vue.component("company_plan_point_list_item", {
	props : [ "company_plan_point" ],
	methods : {
		remove : function() {
			MODEL.company_plan_points.splice(
				MODEL.company_plan_points.indexOf(
					this.$props.company_plan_point
				), 1
			);
			
			updateCanvas();
		}
	},
	template : 
	`<div>
		<span> 
			<span>( {{ company_plan_point.x }} ; {{ company_plan_point.y }} )</span>
			<div class="commonButtonContainer" style="width: 1em; display: inline-block">
				<button class="commonButton" v-on:click="remove">X</button>
			</div>
		</span>
	</div>`
});

Vue.component("manufactory_list_item", {
	props : [ "manufactory" ],
	methods : {
		removeManufactory : function() {
			if(this.$props.manufactory == MODEL.current_manufactory) {
				addPointFunction = addCompanyPlanPoint;
				MODEL.current_manufactory = undefined;
			}
			
			MODEL.manufactories.splice(
				MODEL.manufactories.indexOf(
					this.$props.manufactory
				), 1
			);
			
			updateCanvas();
		},
		setCurrent : function() {
			addPointFunction = addManufactoryPlanPoint;
			MODEL.current_manufactory = this.$props.manufactory;
			MODEL.current_check_point = undefined;
			MODEL.current_enter_leave_point = undefined;
		},
		removePoint : function(manufactory_plan_point) {
			this.$props.manufactory.manufactory_plan_points.splice(
				this.$props.manufactory.manufactory_plan_points.indexOf(
					manufactory_plan_point
				), 1
			);
			
			updateCanvas();
		}
	},
	computed : {
		isActive : function() {
			return this.$props.manufactory == MODEL.current_manufactory;
		}
	},
	template : 
	`<div>
		<h2>
			<div class="manufactory-plan-point-sample" v-bind:class="{ 'current-manufactory-plan-point-sample' : isActive }"
				 style="display: inline-block" v-bind:style="{ backgroundColor : manufactory.color }" v-on:click="setCurrent"></div>
			<span>Manufactory Plan Points</span>
			<div class="commonButtonContainer" style="width: 1em; display: inline-block">
				<button class="commonButton" v-on:click="removeManufactory">X</button>
			</div>
		</h2>
		<ul>
			<li v-for="manufactory_plan_point of manufactory.manufactory_plan_points">
				( {{ manufactory_plan_point.company_plan_point.x }} ; {{ manufactory_plan_point.company_plan_point.y }} ) 
				<div class="commonButtonContainer" style="width: 1em; display: inline-block">
					<button class="commonButton" v-on:click="removePoint(manufactory_plan_point)">X</button>
				</div>
			</li>
		</ul>
	</div>`
});

Vue.component("check_point_list_item", {
	props : [ "check_point" ],
	methods : {
		remove : function() {
			if(this.$props.check_point == MODEL.current_check_point) {
				addPointFunction = addCompanyPlanPoint;
				MODEL.current_check_point = undefined;
			}
			
			MODEL.check_points.splice(
				MODEL.check_points.indexOf(
					this.$props.check_point
				), 1
			);
			
			updateCanvas();
		},
		setCurrent : function() {
			addPointFunction = addCheckPointPlanPoint;
			MODEL.current_check_point = this.$props.check_point;
			
			MODEL.current_manufactory = undefined;
			MODEL.current_enter_leave_point = undefined;
		},
	},
	computed : {
		isActive : function() {
			return this.$props.check_point == MODEL.current_check_point;
		}
	},
	template : 
	`<div>
		<span v-on:click="setCurrent" v-bind:class="{ 'current-check-point-sample' : isActive }"> 
			<span v-if="check_point.company_plan_point1"> #1: ( {{ check_point.company_plan_point1.x }} ; {{ check_point.company_plan_point1.y }} ) </span>
			<br/>
			<span v-if="check_point.company_plan_point2"> #2: ( {{ check_point.company_plan_point2.x }} ; {{ check_point.company_plan_point2.y }} ) </span>
			<div class="commonButtonContainer" style="width: 1em; display: inline-block">
				<button class="commonButton" v-on:click="remove">X</button>
			</div>
		</span>
	</div>`
});

Vue.component("enter_leave_point_list_item", {
	props : [ "enter_leave_point" ],
	methods : {
		remove : function() {
			if(this.$props.enter_leave_point == MODEL.current_enter_leave_point) {
				addPointFunction = addCompanyPlanPoint;
				MODEL.current_enter_leave_point = undefined;
			}
			
			MODEL.enter_leave_points.splice(
				MODEL.enter_leave_points.indexOf(
					this.$props.enter_leave_point
				), 1
			);
			
			updateCanvas();
		},
		setCurrent : function() {
			addPointFunction = addEnterLeavePointPlanPoint;
			MODEL.current_enter_leave_point = this.$props.enter_leave_point;
			
			MODEL.current_manufactory = undefined;
			MODEL.current_check_point = undefined;
		},
	},
	computed : {
		isActive : function() {
			return this.$props.enter_leave_point == MODEL.current_enter_leave_point;
		}
	},
	template : 
	`<div>
		<span v-on:click="setCurrent" v-bind:class="{ 'current-enter-leave-point-sample' : isActive }"> 
			<span v-if="enter_leave_point.company_plan_point1"> #1: ( {{ enter_leave_point.company_plan_point1.x }} ; {{ enter_leave_point.company_plan_point1.y }} ) </span>
			<br/>
			<span v-if="enter_leave_point.company_plan_point2"> #2: ( {{ enter_leave_point.company_plan_point2.x }} ; {{ enter_leave_point.company_plan_point2.y }} ) </span>
			<div class="commonButtonContainer" style="width: 1em; display: inline-block">
				<button class="commonButton" v-on:click="remove">X</button>
			</div>
		</span>
	</div>`
});

function updateCanvas() {
	ctx.clearRect(0, 0, canvas.width, canvas.height);
	ctx.drawImage(image, 0, 0, image.width, image.height);
	
	for(let company_plan_point of MODEL.company_plan_points) {
		ctx.fillStyle = "red";
		ctx.beginPath();
		ctx.arc(company_plan_point.x * canvas.width, company_plan_point.y * canvas.height, COMPANY_PLAN_POINT_RADIUS, 0, 2 * Math.PI, false);
		ctx.fill();
	}
	
	for(let manufactory of MODEL.manufactories) {
		ctx.fillStyle = manufactory.color;
		for(let manufactory_plan_point of manufactory.manufactory_plan_points) {
			ctx.beginPath();
			
			ctx.arc(
				manufactory_plan_point.company_plan_point.x * canvas.width, 
				manufactory_plan_point.company_plan_point.y * canvas.height, 
				MANUFACTORY_PLAN_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
				
			ctx.fill();
		}
		
		ctx.moveTo(
			manufactory.manufactory_plan_points[0].company_plan_point.x * canvas.width,
			manufactory.manufactory_plan_points[0].company_plan_point.y * canvas.height
		);
		
		ctx.beginPath();
		
		for(let manufactory_plan_point of manufactory.manufactory_plan_points) {
			ctx.lineTo(
				manufactory_plan_point.company_plan_point.x * canvas.width, 
				manufactory_plan_point.company_plan_point.y * canvas.height
			);
		}
		
		ctx.lineTo(
			manufactory.manufactory_plan_points[0].company_plan_point.x * canvas.width,
			manufactory.manufactory_plan_points[0].company_plan_point.y * canvas.height
		);
		
		ctx.fill();
	}
	
	for(let check_point of MODEL.check_points) {
		ctx.fillStyle = "green";
		
		if(check_point.company_plan_point1) {
			ctx.beginPath();
			ctx.arc(
				check_point.company_plan_point1.x * canvas.width, 
				check_point.company_plan_point1.y * canvas.height, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
		
		if(check_point.company_plan_point2) {
			ctx.beginPath();
			ctx.arc(
				check_point.company_plan_point2.x * canvas.width, 
				check_point.company_plan_point2.y * canvas.height, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
	}
	
	for(let enter_leave_point of MODEL.enter_leave_points) {
		ctx.fillStyle = "yellow";
		
		if(enter_leave_point.company_plan_point1) {
			ctx.beginPath();
			ctx.arc(
				enter_leave_point.company_plan_point1.x * canvas.width, 
				enter_leave_point.company_plan_point1.y * canvas.height, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
		
		if(enter_leave_point.company_plan_point2) {
			ctx.beginPath();
			ctx.arc(
				enter_leave_point.company_plan_point2.x * canvas.width, 
				enter_leave_point.company_plan_point2.y * canvas.height, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
	}
}

function addCompanyPlanPoint(position) {
	MODEL.company_plan_points.push({ 
		fake_id: COMPANY_PLAN_POINT_COUNTER++,
		x: Math.round(position.x * 10000 / canvas.width) / 10000, 
		y: Math.round(position.y * 10000 / canvas.height) / 10000
	});
}

function findCompanyPlanPointAtPosition(position) {
	return MODEL.company_plan_points.find(
		point => Math.pow(position.x - point.x * canvas.width, 2.0) + 
				 Math.pow(position.y - point.y * canvas.height, 2.0) <=
				 COMPANY_PLAN_POINT_RADIUS * COMPANY_PLAN_POINT_RADIUS
	);
}

function addManufactory() {
	MODEL.current_check_point = undefined;
	MODEL.current_enter_leave_point = undefined;
	
	MODEL.current_manufactory = {
		color : "#" + Math.floor(Math.random() * 16777215).toString(16) + "AA",
		manufactory_plan_points : []
	};
	
	MODEL.manufactories.push(MODEL.current_manufactory);
	addPointFunction = addManufactoryPlanPoint;
}

function addManufactoryPlanPoint(position) {
	let company_plan_point = findCompanyPlanPointAtPosition(position);
	
	if(!company_plan_point || MODEL.current_manufactory.manufactory_plan_points.find(point => point.company_plan_point == company_plan_point)) {
		return;
	}
	
	MODEL.current_manufactory.manufactory_plan_points.push({ company_plan_point: company_plan_point });
}

function addCheckPoint() {
	MODEL.current_manufactory = undefined;
	MODEL.current_enter_leave_point = undefined;
	
	MODEL.current_check_point = {
		company_plan_point1 : undefined,
		company_plan_point2 : undefined
	};
	
	MODEL.check_points.push(MODEL.current_check_point);
	addPointFunction = addCheckPointPlanPoint;
}

function addCheckPointPlanPoint(position) {
	let company_plan_point = findCompanyPlanPointAtPosition(position);
	
	if(!company_plan_point || (MODEL.current_check_point.company_plan_point1 && MODEL.current_check_point.company_plan_point2)) {
		return;
	}
	
	if(!MODEL.current_check_point.company_plan_point1) {
		MODEL.current_check_point.company_plan_point1 = company_plan_point;
	} else if(!MODEL.current_check_point.company_plan_point2) {
		MODEL.current_check_point.company_plan_point2 = company_plan_point;
	}
	
	if(MODEL.current_check_point.company_plan_point1 && MODEL.current_check_point.company_plan_point2) {
		MODEL.current_check_point = undefined;
	}
}

function addEnterLeavePoint() {
	MODEL.current_manufactory = undefined;
	MODEL.current_check_point = undefined;
	
	MODEL.current_enter_leave_point = {
		company_plan_point1 : undefined,
		company_plan_point2 : undefined
	};
	
	MODEL.enter_leave_points.push(MODEL.current_enter_leave_point);
	addPointFunction = addEnterLeavePointPlanPoint;
}

function addEnterLeavePointPlanPoint(position) {
	let company_plan_point = findCompanyPlanPointAtPosition(position);
	
	if(!company_plan_point || (MODEL.current_enter_leave_point.company_plan_point1 && MODEL.current_enter_leave_point.company_plan_point2)) {
		return;
	}
	
	if(!MODEL.current_enter_leave_point.company_plan_point1) {
		MODEL.current_enter_leave_point.company_plan_point1 = company_plan_point;
	} else if(!MODEL.current_enter_leave_point.company_plan_point2) {
		MODEL.current_enter_leave_point.company_plan_point2 = company_plan_point;
	}
	
	if(MODEL.current_enter_leave_point.company_plan_point1 && MODEL.current_enter_leave_point.company_plan_point2) {
		MODEL.current_enter_leave_point = undefined;
	}
}

function resetAddPointFunction() {
	addPointFunction = addCompanyPlanPoint;
	
	MODEL.current_manufactory = undefined;
	MODEL.current_check_point = undefined;
	MODEL.current_enter_leave_point = undefined;
}

let addPointFunction = addCompanyPlanPoint;

function getMousePosition(canvas, event) {
	let rect = canvas.getBoundingClientRect();
	let x = event.clientX - rect.left;
	let y = event.clientY - rect.top;
	return { x: x, y: y };
}

let canvas = document.getElementById('plan');
let ctx = canvas.getContext('2d');
let image = new Image();

canvas.addEventListener("mousedown", e => {
	let position = getMousePosition(canvas, e);
	addPointFunction(position);
	updateCanvas();
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Company/Get",
	data : JSON.stringify({ data : { id : SESSION.getCompanyId() } }),
	dataType : "json",
	success : function(response) {
		MODEL.image_url = response.data.plan_image_url;
		
		MODEL.company_plan_points = response.data.company_plan_unique_points.map(point => {
			return {
				id : point.id,
				x: point.x, 
				y: point.y
			}
		});
		
		MODEL.manufactories = response.data.manufactories.map(manufactory => {
			return {
				id : manufactory.id,
				color : "#" + Math.floor(Math.random() * 16777215).toString(16) + "AA",
				manufactory_plan_points : manufactory.manufactory_plan_points.map(point => { 
					return {
						id : point.id,
						company_plan_point : MODEL.company_plan_points.find(p => p.id == point.company_plan_point_id)
					}
				})
			}
		});
		
		MODEL.check_points = response.data.check_points.map(check_point => {
			return {
				id : check_point.id,
				company_plan_point1 : MODEL.company_plan_points.find(p => p.id == check_point.company_plan_unique_point1_id),
				company_plan_point2 : MODEL.company_plan_points.find(p => p.id == check_point.company_plan_unique_point2_id)
			}
		});
		
		MODEL.enter_leave_points = response.data.enter_leave_points.map(enter_leave_point => {
			return {
				id : enter_leave_point.id,
				company_plan_point1 : MODEL.company_plan_points.find(p => p.id == enter_leave_point.company_plan_unique_point1_id),
				company_plan_point2 : MODEL.company_plan_points.find(p => p.id == enter_leave_point.company_plan_unique_point2_id)
			}
		});
		
		image.src = response.data.plan_image_url;
		
		image.onload = () => {
			canvas.width = image.width;
			canvas.height = image.height;	
			updateCanvas();
		};
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Company Plan"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function save() {
	let dto = {
		data : {
			company_id : SESSION.getCompanyId(),
			company_plan_points : MODEL.company_plan_points,
			manufactories : MODEL.manufactories.map(m => {
				return {
					id : m.id,
					manufactory_plan_points : m.manufactory_plan_points.map(mp => {
						return {
							company_plan_point_id : mp.company_plan_point.id,
							fake_company_plan_point_id : mp.company_plan_point.fake_id
						}
					})
				}
			}),
			check_points : MODEL.check_points.map(cp => {
				return {
					id : cp.id,
					company_plan_unique_point1_id : cp.company_plan_point1.id,
					fake_company_plan_unique_point1_id : cp.company_plan_point1.fake_id,
					company_plan_unique_point2_id : cp.company_plan_point2.id,
					fake_company_plan_unique_point2_id : cp.company_plan_point2.fake_id
				}
			}),
			enter_leave_points : MODEL.enter_leave_points.map(elp => {
				return {
					id : elp.id,
					company_plan_unique_point1_id : elp.company_plan_point1.id,
					fake_company_plan_unique_point1_id : elp.company_plan_point1.fake_id,
					company_plan_unique_point2_id : elp.company_plan_point2.id,
					fake_company_plan_unique_point2_id : elp.company_plan_point2.fake_id
				}
			})
		}
	}
	
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Company/SetupPlan",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			throw JSON.stringify({nofail: true, source: "Update Company Plan", message: "Success"});
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Update Company Plan"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
}