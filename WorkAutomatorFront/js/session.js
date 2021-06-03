var SESSION = {
	getUserId : function() {
		return COOKIE.getCookie("USER_ID");
	},
	
	getSessionDto: function() {
		let user_id = COOKIE.getCookie("USER_ID");
		let token = COOKIE.getCookie("SESSION_TOKEN");
		
		if(user_id == undefined || token == undefined) {
			return null;
		}
		
		let salt = Math.round(Math.random() * 100000).toString();
		let token_salted = sha256(token + salt);
			
		let sessionDto = {
			user_id : user_id,
			session_token_salted : token_salted,
			salt : salt
		};
		return sessionDto;
	},
	
	putToAjaxRequest: function(request) {
		sessionDto = this.getSessionDto();
		request.headers = {
			UID : sessionDto.user_id,
			SALT : sessionDto.salt,
			SESSION_TOKEN_SALTED : sessionDto.session_token_salted
		};
	},

	createSession: function(user_id, token) {
		COOKIE.setCookie("USER_ID", user_id, { 'max-age': 8640000000000 } ); 
		COOKIE.setCookie("SESSION_TOKEN", token, { 'max-age': 8640000000000 } ); 
	},
	
	kill: function() {
		COOKIE.eraseCookie("USER_ID"); 
		COOKIE.eraseCookie("SESSION_TOKEN"); 
	}
}