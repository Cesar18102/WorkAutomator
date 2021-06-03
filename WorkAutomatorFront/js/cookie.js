var COOKIE = {

	getCookie: function(name) {
		let matches = document.cookie.match(new RegExp(
			"(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
		));
		return matches ? decodeURIComponent(matches[1]) : undefined;
	},

	setCookie: function(name, value, options = {}) {
		options = {
			path: '/',
			...options
		};

		if (options.expires instanceof Date) {
			options.expires = options.expires.toUTCString();
		}

		let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

		for (let optionKey in options) {
			updatedCookie += "; " + optionKey;
			let optionValue = options[optionKey];
			if (optionValue !== true) {
				updatedCookie += "=" + optionValue;
			}
		}

		document.cookie = updatedCookie;
	},
	
	eraseCookie : function(name) {
		var pathBits = location.pathname.split('/');
		var pathCurrent = ' path=';

		document.cookie = name + '=; expires=Thu, 01-Jan-1970 00:00:01 GMT;';

		for (var i = 0; i < pathBits.length; i++) {
			pathCurrent += ((pathCurrent.substr(-1) != '/') ? '/' : '') + pathBits[i];
			document.cookie = name + '=; expires=Thu, 01-Jan-1970 00:00:01 GMT;' + pathCurrent + ';';
		}
	}
}