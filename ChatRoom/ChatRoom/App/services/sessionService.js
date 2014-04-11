app.factory('sessionService', function ($http, $cookieStore) {

	var COOKIEUSER_KEY = "CURRENT_USER";

	var API_LOGIN_URL = "account/login";
	var API_LOGOUT_URL = "account/logout";

	var HTTP_HEADER_KEY = 'Api-Auth-Token';

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

	$http.defaults.headers.common[HTTP_HEADER_KEY] = session.user.token;

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

	session.logout = function (successHandler) {
		$http.post(api(API_LOGOUT_URL))
			.success(function (data, status, headers, config) {
				session.user = defaultUser;
				session.isAuthenticated = false;

				$cookieStore.remove(COOKIEUSER_KEY);

				if (successHandler) successHandler();
			})
			.error(function () {

			});
	}

	return session;
});