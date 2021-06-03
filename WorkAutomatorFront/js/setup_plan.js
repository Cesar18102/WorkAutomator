const COMPANY_PLAN_POINT_RADIUS = 8;
const MANUFACTORY_PLAN_POINT_RADIUS = 6;
const CHECK_POINT_RADIUS = 4;

let COMPANY_PLAN_POINT_COUNTER = 1;

var MODEL = {
	image_url: 'https://ce-na.ru/upload/iblock/afb/afb01ea6b753ae90a6ccf14126d36bc6.jpg',
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
			( {{ company_plan_point.x }} ; {{ company_plan_point.y }} ) 
			<button v-on:click="remove">X</button>
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
			<button v-on:click="removeManufactory">X</button>
		</h2>
		<ul>
			<li v-for="manufactory_plan_point of manufactory.manufactory_plan_points">
				( {{ manufactory_plan_point.company_plan_point.x }} ; {{ manufactory_plan_point.company_plan_point.y }} ) 
				<button v-on:click="removePoint(manufactory_plan_point)">X</button>
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
			<span v-if="check_point.company_plan_point2"> #2: ( {{ check_point.company_plan_point2.x }} ; {{ check_point.company_plan_point2.y }} ) </span>
			<button v-on:click="remove">X</button>
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
			
			MODEL.enter_leave_point.splice(
				MODEL.enter_leave_point.indexOf(
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
			<span v-if="enter_leave_point.company_plan_point2"> #2: ( {{ enter_leave_point.company_plan_point2.x }} ; {{ enter_leave_point.company_plan_point2.y }} ) </span>
			<button v-on:click="remove">X</button>
		</span>
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
	
	for(let company_plan_point of MODEL.company_plan_points) {
		ctx.fillStyle = "red";
		ctx.beginPath();
		ctx.arc(company_plan_point.click_x, company_plan_point.click_y, COMPANY_PLAN_POINT_RADIUS, 0, 2 * Math.PI, false);
		ctx.fill();
	}
	
	for(let manufactory of MODEL.manufactories) {
		for(let manufactory_plan_point of manufactory.manufactory_plan_points) {
			ctx.fillStyle = "#" + manufactory.color;
			ctx.beginPath();
			
			ctx.arc(
				manufactory_plan_point.company_plan_point.click_x, 
				manufactory_plan_point.company_plan_point.click_y, 
				MANUFACTORY_PLAN_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
				
			ctx.fill();
		}
	}
	
	for(let check_point of MODEL.check_points) {
		ctx.fillStyle = "green";
		
		if(check_point.company_plan_point1) {
			ctx.beginPath();
			ctx.arc(
				check_point.company_plan_point1.click_x, 
				check_point.company_plan_point1.click_y, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
		
		if(check_point.company_plan_point2) {
			ctx.beginPath();
			ctx.arc(
				check_point.company_plan_point2.click_x, 
				check_point.company_plan_point2.click_y, 
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
				enter_leave_point.company_plan_point1.click_x, 
				enter_leave_point.company_plan_point1.click_y, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
		
		if(enter_leave_point.company_plan_point2) {
			ctx.beginPath();
			ctx.arc(
				enter_leave_point.company_plan_point2.click_x, 
				enter_leave_point.company_plan_point2.click_y, 
				CHECK_POINT_RADIUS, 
				0, 2 * Math.PI, false
			);
			ctx.fill();
		}
	}
}

function addCompanyPlanPoint(position) {
	MODEL.company_plan_points.push({ 
		click_x: position.x,
		click_y: position.y,
		fake_id: COMPANY_PLAN_POINT_COUNTER++,
		x: Math.round(position.x * 10000 / canvas.width) / 10000, 
		y: Math.round(position.y * 10000 / canvas.height) / 10000
	});
}

function findCompanyPlanPointAtPosition(position) {
	return MODEL.company_plan_points.find(
		point => Math.pow(position.x - point.click_x, 2.0) + 
				 Math.pow(position.y - point.click_y, 2.0) <=
				 COMPANY_PLAN_POINT_RADIUS * COMPANY_PLAN_POINT_RADIUS
	);
}

function addManufactory() {
	MODEL.current_check_point = undefined;
	MODEL.current_enter_leave_point = undefined;
	
	MODEL.current_manufactory = {
		color : Math.floor(Math.random() * 16777215).toString(16),
		manufactory_plan_points : []
	};
	
	MODEL.manufactories.push(MODEL.current_manufactory);
	addPointFunction = addManufactoryPlanPoint;
}

function addManufactoryPlanPoint(position) {
	let company_plan_point = findCompanyPlanPointAtPosition(position);
	
	if(!company_plan_point) {
		return;
	}
	
	MODEL.current_manufactory.manufactory_plan_points.push({ 
		company_plan_point: company_plan_point
	});
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

canvas.addEventListener("mousedown", e => {
	let position = getMousePosition(canvas, e);
	addPointFunction(position);
	updateCanvas();
});