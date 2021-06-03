Vue.filter(
	'localize',
	function(id) {
		return LOCALIZE.getString(id);
	}
);
