Vue.component("err", {
	props : ["is_shown", "message"],
	methods : {
		closeModal : function (){
			this.$props.is_shown = false;
		}
	},
	template : 
	`<div class="modal" v-if="is_shown">
	  <div class="modal-content">
		<span>{{ message }}</span>
		
		<div class="common-button-container">
			<button v-on:click="closeModal" class="common-button">
				OK
			</button>
		</div>
	  </div>
	</div>`
});

let vue = new Vue({
	el: "#err"
});

window.onerror = e => {
	err = JSON.parse(e.replace("Uncaught ", ""));
	
	if(err.nofail)
		vue.$children[0].message = err.source + ": " + err.message;
	else
	{
		let message = err.source + " Failed: " + err.ex.message;
		
		if(err.ex.invalid_messages) {
			message += ": " + err.ex.invalid_messages.join(", ");
		}
		
		if(err.ex.required_permissions) {
			message += ". Required permissions: " + err.ex.required_permissions.join(", ");
		}
		
		vue.$children[0].message = message;
	}
	
	vue.$children[0].is_shown = true;
};

Vue.config.errorHandler = e => {
	err = JSON.parse(e.replace("Uncaught ", ""));
	
	if(err.nofail)
		vue.$children[0].message = err.source + ": " + err.message;
	else
	{
		vue.$children[0].message = err.source + " Failed: " + err.ex.message + 
			(err.ex.invalid_messages ? ": " + err.ex.invalid_messages.join(", ") : "");
	}
	
	vue.$children[0].is_shown = true;
}