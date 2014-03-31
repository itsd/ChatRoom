app.factory('sessionService', function ($http, $cookieStore) {

	var COOKIEUSER_KEY = "CURRENT_USER";

	var API_LOGIN_URL = "account/login";
	var API_LOGOUT_URL = "";

	var defaultUser = {
		userID: 0,
		token: '',
		username: ''
	};

	var currentUser = $cookieStore.get(COOKIEUSER_KEY);

	var session = {
		user: currentUser ? currentUser : defaultUser,
		isAuthenticated: currentUser ? true : false
	};

	function parseUser(data) {
		return {
			userID: data.id,
			token: data.token,
			username: data.alias
		}
	}

	session.login = function (username, password, successHandler, failureHandler) {
		session.user = defaultUser;
		$cookieStore.remove(COOKIEUSER_KEY);


		$http.post(api(API_LOGIN_URL), { username: username, password: password })
			.success(function (data, status, headers, config) {
				session.user = parseUser(data);
				session.isAuthenticated = true;

				$cookieStore.put(COOKIEUSER_KEY, session.user);

				if (successHandler) { successHandler(); }

			})
			.error(function (data, status, headers, config) {

				if (failureHandler) { failureHandler(); }

			});
	};

	return session;
});