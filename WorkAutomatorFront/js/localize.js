var LOCALIZE = {
	eng : [{
		key : "members",
		value : "Members"
	}, {
		key : "project",
		value : "Project"
	}, {
		key : "myProjectList",
		value : "My Projects"
	}, {
		key : "allProjectList",
		value : "All Projects"
	}, {
		key : "newProject",
		value : "New Project"
	}, {
		key : "gamification",
		value : "Gamification"
	}, {
		key : "quiz",
		value : "Quiz"
	}, {
		key : "presents",
		value : "Presents"
	}, {
		key : "newQuiz",
		value : "New quiz"
	}, {
		key : "newPresent",
		value : "New present"
	}, {
		key : "myTasks",
		value : "My Tasks"
	}, {
		key : "delete",
		value : "Delete"
	}, {
		key : "edit",
		value : "Edit"
	}, {
		key : "company",
		value : "Company"
	}, {
		key : "workersList",
		value : "Workers list"
	}, {
		key : "manageCompany",
		value : "Manage company"
	}, {
		key : "manageWorkers",
		value : "Manage workers"
	}, {
		key : "newCompany",
		value : "New company"
	}, {
		key : "allTasks",
		value : "All Tasks"
	}, {
		key : "lang",
		value : "Lang"
	}, {
		key : "account",
		value : "Account"
	}, {
		key : "settings",
		value : "Settings"
	}, {
		key : "leave",
		value : "Leave"
	}, {
		key : "fire",
		value : "Fire"
	}, {
		key : "bind",
		value : "Bind"
	}, {
		key : "save",
		value : "Save"
	}, {
		key : "buy",
		value : "Buy"
	}, {
		key : "start",
		value : "Start"
	}, {
		key : "stop",
		value : "Stop"
	}, {
		key : "play",
		value : "Play"
	}, {
		key : "answer",
		value : "Submit"
	}, {
		key : "results",
		value : "Results"
	}, {
		key : "all_results",
		value : "All results"
	}, {
		key : "user_profile",
		value : "User's profile"
	}, {
		key : "login",
		value : "Login"
	}, {
		key : "first_name",
		value : "First name"
	}, {
		key : "last_name",
		value : "Last name"
	}, {
		key : "email",
		value : "Email"
	}, {
		key : "points",
		value : "Points"
	}, {
		key : "trello_username",
		value : "Trello username"
	}, {
		key : "trello_api_key",
		value : "Trello API-key"
	}, {
		key : "save",
		value : "Save"
	}, {
		key : "presents",
		value : "Presents"
	}
	],
	ukr : [ {
		key : "members",
		value : "Робітники"
	}, {
		key : "project",
		value : "Проєкт"
	}, {
		key : "myProjectList",
		value : "Мої проєкти"
	}, {
		key : "allProjectList",
		value : "Усі проєкти"
	}, {
		key : "newProject",
		value : "Новий проєкт"
	}, {
		key : "gamification",
		value : "Гейміфікація"
	}, {
		key : "quiz",
		value : "Вікторина"
	}, {
		key : "presents",
		value : "Подарунки"
	}, {
		key : "newQuiz",
		value : "Створити вікторину"
	}, {
		key : "newPresent",
		value : "Створити подарунок"
	}, {
		key : "myTasks",
		value : "Мої завдання"
	},{
		key : "delete",
		value : "Видалити"
	}, {
		key : "edit",
		value : "Редагувати"
	}, {
		key : "company",
		value : "Компанія"
	}, {
		key : "workersList",
		value : "Список робітників"
	}, {
		key : "manageCompany",
		value : "Управління компанією"
	}, {
		key : "manageWorkers",
		value : "Управління робітниками"
	}, {
		key : "allTasks",
		value : "Усі Завдання"
	}, {
		key : "newCompany",
		value : "Створити компанію"
	}, {
		key : "lang",
		value : "Мова"
	}, {
		key : "account",
		value : "Акаунт"
	}, {
		key : "settings",
		value : "Налаштування"
	}, {
		key : "leave",
		value : "Вийти"
	}, {
		key : "fire",
		value : "Звільнити"
	}, {
		key : "bind",
		value : "Прив'язати"
	}, {
		key : "save",
		value : "Зберегти"
	}, {
		key : "buy",
		value : "Придбати"
	}, {
		key : "start",
		value : "Розпочати"
	}, {
		key : "stop",
		value : "Закінчити"
	}, {
		key : "play",
		value : "Грати"
	}, {
		key : "answer",
		value : "Завершити"
	}, {
		key : "results",
		value : "Результати"
	}, {
		key : "all_results",
		value : "Усі результати"
	}, {
		key : "user_profile",
		value : "Профіль користувача"
	}, {
		key : "login",
		value : "Логін"
	}, {
		key : "first_name",
		value : "Ім'я"
	}, {
		key : "last_name",
		value : "Прізвище"
	}, {
		key : "email",
		value : "Електронна пошта"
	}, {
		key : "points",
		value : "Бали"
	}, {
		key : "trello_username",
		value : "Логін Trello"
	}, {
		key : "trello_api_key",
		value : "Trello API-ключ"
	}, {
		key : "save",
		value : "Зберегти"
	}, {
		key : "presents",
		value : "Подарунки"
	}
	],
	getString: function(id) {
		let lang = COOKIE.getCookie("LANG");
		let data = this.eng;
		
		if(lang == "ukr") {
			data = this.ukr;
		}

		let kvp = data.find(dt => dt.key == id);
		
		return kvp ? kvp.value : "NOLOC<" + id + ">";
	},
	setLang: function(lang) {
		COOKIE.setCookie("LANG", lang);
	}
}