Vue.filter(
	'localize',
	function(id) {
		return LOCALIZE.getString(id);
	}
);

Vue.filter(
	'data_type_name_from_id',
	function(id) {
		switch(id) {
			case 1: return "int";
			case 2: return "int[]";
			case 3: return "float";
			case 4: return "float[]";
			case 5: return "bool";
			case 6: return "bool[]";
		}
	}
);

Vue.filter(
	'chunk',
	function(arr, size) {
		let res = [];
		for (let i = 0; i < arr.length; i += size) {
			const chunk = arr.slice(i, i + size);
			res.push(chunk);
		}
		return res;
	}
);