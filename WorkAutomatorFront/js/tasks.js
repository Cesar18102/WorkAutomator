var MODEL = {
	tasks : [],
	user_id : SESSION.getUserId()
};

Vue.component('task_item', {
	props : ['task'],
	methods : {
		done : function() {
			let task = this.$props.task;
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Task/NotifyDone",
				data : JSON.stringify({ data : { id : task.id }}),
				dataType : "json",
				success : function(response) {
					task.is_done = true;
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Mark Task Done"});
				}
			};
			
			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		},
		review : function(state) {
			let task = this.$props.task;
			
			let request = {
				type :  "POST",
				contentType : "application/json",
				url : "https://workautomatorback.azurewebsites.net/api/Task/NotifyReviewed",
				data : JSON.stringify({ data : { id : task.id, review_result : state }}),
				dataType : "json",
				success : function(response) {
					task.is_reviewed = true;
				},
				error : function(response) {
					throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Task Review"});
				}
			};
			
			SESSION.putToAjaxRequest(request);
			$.ajax(request);
		}
	},
	template :
	`<div class="listItemContainer" style="width: auto; height: auto; display: block;">
		<span>Task #{{ task.id }}</span>
		<center><b> {{ task.name }} </b></center>
		<i v-if="task.description">{{ task.description }}</i>
		
		<div v-if="task.associated_fault_prefab">
			<span> Associated with fault "{{ task.associated_fault_prefab.name }}" #{{ task.associated_fault_id }} </span>
			<img v-if="task.associated_fault_prefab.image_url" v-bind:src="task.associated_fault_prefab.image_url" width="50" height="50"></img>
		</div>
		
		<div>
			<span v-if="task.is_done">Done</span>
			<span v-if="task.is_reviewed">Reviewed</span>
		</div>
		
		<div class="common-button-container" style="display: inline-block" v-if="task.assignee && task.assignee.id == $root.user_id && !task.is_done">
			<button class="common-button" v-on:click="done">Done</button>
		</div>
		<div class="common-button-container" style="display: inline-block" v-if="task.reviewer && task.reviewer.id == $root.user_id && !task.is_reviewed">
			<button class="common-button" v-on:click="review(true)">Review Pass</button>
		</div>
		<div class="common-button-container" style="display: inline-block" v-if="task.reviewer && task.reviewer.id == $root.user_id && !task.is_reviewed">
			<button class="common-button" v-on:click="review(false)">Review Fail</button>
		</div>
	</div>`
});

new Vue({
	el : "#tasks",
	data : MODEL
});

let request = {
	type :  "POST",
	contentType : "application/json",
	url : "https://workautomatorback.azurewebsites.net/api/Task/GetMy",
	data : JSON.stringify({ data : { id : 0 }}),
	dataType : "json",
	success : function(response) {
		MODEL.tasks = response.data;
	},
	error : function(response) {
		throw JSON.stringify({ex: response.responseJSON.error.exception, source: "Get Tasks List"});
	}
};

SESSION.putToAjaxRequest(request);
$.ajax(request);