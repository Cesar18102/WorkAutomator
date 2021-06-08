var MODEL = {
	user : {
		login : undefined,
		first_name : undefined,
		last_name : undefined
	},
	events : []
};

Vue.component("event_list_item", {
	props : [ "event" ],
	template : 
	`<div class="listItemContainer" style="width: auto; height: auto; display: block;">
		<span v-if="event.type == 'enter_leave'"> 
			{{ event.is_enter ? "Enter" : "Leave" }} at {{ event.timespan }} 
			to manufactory #{{ event.enter_leave_point.manufactory_id }} via enter/leave point #{{ event.enter_leave_point.id }}
		</span>
		<span v-if="event.type == 'checkout'"> 
			Checkout from {{ event.is_direct ? event.check_point.manufactory1_id : event.check_point.manufactory2_id }}
			to {{ event.is_direct ? event.check_point.manufactory2_id : event.check_point.manufactory1_id }}
			at {{ event.timespan }} via check point #{{ event.check_point.id }}
		</span>
		<span v-if="event.type == 'pipeline'"> 
			Interaction with pipeline item "{{ event.pipeline_item.prefab.name }}" #{{ event.pipeline_item.id }} at {{ event.timespan }};
			Log: {{ event.log }}
		</span>
		<span v-if="event.type == 'detector'"> 
			Interaction with detector "{{ event.detector.prefab.name }}" #{{ event.detector.id }} at {{ event.timespan }};
			Log: {{ event.log }}
		</span>
		<span v-if="event.type == 'storage_cell'"> 
			Interaction with storage cell "{{ event.storage_cell.prefab.name }}" #{{ event.storage_cell.id }} at {{ event.timespan }};
			Log: {{ event.log }}
		</span>
	</div>`
});

new Vue({ el : "#user-info", data : MODEL });

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Account/Get",
	data : JSON.stringify({ data : { id : SESSION.getUserId() } }),
	dataType : "json",
	success : function(response) {4
		Object.assign(MODEL.user, response.data);
		
		let events = []
		
		for(let enter_leave_event of response.data.enter_leave_point_events) {
			enter_leave_event.type = 'enter_leave';
			events.push(enter_leave_event);
		}
		
		for(let checkout_event of response.data.check_point_events) {
			checkout_event.type = 'checkout';
			events.push(checkout_event);
		}
		
		for(let pipeline_item_event of response.data.pipeline_item_interaction_events) {
			pipeline_item_event.type = 'pipeline';
			events.push(pipeline_item_event);
		}
		
		for(let detector_event of response.data.detector_interaction_events) {
			detector_event.type = 'detector';
			events.push(detector_event);
		}
		
		for(let storage_cell_event of response.data.storage_cell_events) {
			storage_cell_event.type = 'storage_cell';
			events.push(storage_cell_event);
		}
		
		events.sort(function(a, b){
		    return new Date(b.timespan) - new Date(a.timespan);
		});
		
		MODEL.events = events;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get User Profile"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);

function saveUserProfile(e) {
	let data = $('#user_profile_form').serializeObject();
	
	let dto = {
		data : {
			id : SESSION.getUserId(),
			first_name : data.first_name,
			last_name : data.last_name
		}
	}
	
	let request = {
		type :  "POST",
		contentType : "application/json",
		url : "https://workautomatorback.azurewebsites.net/api/Account/Update",
		data : JSON.stringify(dto),
		dataType : "json",
		success : function(response) {
			MODEL.user = response.data;
			throw JSON.stringify({nofail: true, source: "Update User Profile", message: "Success"});
		},
		error : function(response) {
			throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Update User Profile"});
		}
	};
	
	SESSION.putToAjaxRequest(request);
	$.ajax(request);
	
	e.originalEvent.preventDefault();
	e.originalEvent.stopPropagation();
}

$('#user_profile_form').submit(saveUserProfile);