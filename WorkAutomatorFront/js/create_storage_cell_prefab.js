var MODEL = {
	name : undefined,
	image_url : undefined,
	input_x : undefined,
	input_y : undefined,
	output_x : undefined,
	output_y : undefined
};

new Vue({
	el : "#storage_cell_prefab_reg_form",
	data : MODEL
});

let canvas = document.getElementById('storage_cell_prefab_canvas');
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
			output_y : MODEL.output_y
		}
	}
	
	let request = {
		type : "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Prefab/CreateStorageCellPrefab",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(reg_response) {
			document.location = "../pages/storage_cell_prefabs.html";
		},
		error : function(reg_response) {
			throw JSON.stringify({ex: reg_response.responseJSON.error.exception, source: "Storage Cell Prefab Create"});
		}
	}
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#storage_cell_prefab_registration_form').submit(save);