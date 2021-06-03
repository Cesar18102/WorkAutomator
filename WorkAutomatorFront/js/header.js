var HEADER_MODEL = {
	statistics : false,
	company : false,
	manage_company : false,
	create_company : false,
	manage_project : false
};

Vue.component('custom-header', {
	data : function() {
		return HEADER_MODEL;
	},
	methods : {
		exit: function() {
			SESSION.kill();
		},
		setLang: function(lang) {
			LOCALIZE.setLang(lang);
		}
	},
	template : 
	`<ul id="header_content" class="topmenu">
		<li><a class="menu_nav logo" href="#" id="logo">Work&Play</a></li>
		<li style="margin-left:10vw;"><a class="menu_nav" href="#" v-if="company">{{ 'company' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li v-if="manage_company"><a href="../pages/company_profile.html">{{ 'manageCompany' | localize }}</a></li>
				<li v-if="create_company"><a href="../pages/company_registration.html">{{ 'newCompany' | localize }}</a></li>
			</ul>
		</li>
		<li><a class="menu_nav" href="#">{{ 'project' | localize }}<i class="fa fa-angle-down"></i></a>
			<ul class="submenu">
				<li><a href="../pages/my_projects.html">{{ 'myProjectList' | localize }}</a></li>
				<li v-if="manage_project"><a href="../pages/projects.html">{{ 'allProjectList' | localize }}</a></li>
				<li v-if="manage_project"><a href="../pages/project_registration.html">{{ 'newProject' | localize }}</a></li>
			</ul>
		</li>
		<li><a class="menu_nav" href="#">{{ 'gamification' | localize }}</a>
			<ul class="submenu">
				<li><a href="../pages/quizs.html">{{ 'quiz' | localize }}</a></li>
				<li><a href="../pages/presents.html">{{ 'presents' | localize }}</a></li>
				<li><a href="../pages/quiz_registration.html">{{ 'newQuiz' | localize }}</a></li>
				<li><a href="../pages/present_registration.html">{{ 'newPresent' | localize }}</a></li>
			</ul>
		</li>
		<!--<li>
			<a v-bind:class="[ { invisible: !statistics }, menu_nav ]" href="#">
				{{ 'statistics' | localize }}<i class="fa fa-angle-down"></i>
			</a>
			<ul class="submenu" >
				<li><a href="">Enter events</a></li>
				<li><a href="">Leave events</a></li>
			</ul>
		</li>-->
		<li style="margin-left:20vw;"><a class="menu_nav" href="#">{{ 'lang' | localize }}</a>
			<ul class="submenu">
				<li v-on:click="setLang('eng')"><a>English</a></li>
				<li v-on:click="setLang('ukr')"><a>Ukrainian</a></li>
			</ul>
		</li>
		<li class="account"><a class="menu_nav" href="#">{{ 'account' | localize }}</a>
			<ul class="submenu">
				<li><a href="../pages/user_profile.html">{{ 'settings' | localize }}</a></li>
				<li><a v-on:click="exit()" href="../index.html">{{ 'leave' | localize }}</a></li>
			</ul>
		</li>
	</ul>`
});

new Vue({ el : "#header" });

ROLE.getRole(role => {
	HEADER_MODEL.manage_company = role == "superadmin" || role == "owner";
	HEADER_MODEL.create_company = role == "none";
	HEADER_MODEL.company = HEADER_MODEL.manage_company || HEADER_MODEL.create_company;
	HEADER_MODEL.statistics = role == "superadmin" || role == "owner";
	HEADER_MODEL.manage_project = role == "superadmin" || role == "owner" || role == "manager";
});