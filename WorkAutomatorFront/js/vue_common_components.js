Vue.component("data_type_selector", {
	props : [ "data_type_id" ],
	methods : {
		updateValue : function(e) {
			this.$props.data_type_id = e.target.value;
		}
	},
	template : 
	`<select class="input100" placeholder="Data Type" v-on:change="updateValue">
		<option value="3">float</option>
		<option value="4">float[]</option>
		<option value="1">int</option>
		<option value="2">int[]</option>
		<option value="5">bool</option>
		<option value="6">bool[]</option>
	</select>`
});
