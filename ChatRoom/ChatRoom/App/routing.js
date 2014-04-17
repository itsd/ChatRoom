app
	.config(function ($routeProvider, $locationProvider) {

		$routeProvider.when('/', {
			templateUrl: 'app/views/index.html',
			controller: 'indexController'
		})

		$routeProvider.when('/login',
			{
				templateUrl: 'app/views/login/index.html',
				controller: 'loginController'
			});

		$routeProvider.when('/logout',
			{
				templateUrl: 'app/views/logout/index.html',
				controller: 'logoutController'
			});

		$routeProvider.when('/room',
			{
				templateUrl: 'app/views/room/index.html',
				controller: 'roomController'
			});

		$routeProvider.when('/profile',
			{
				templateUrl: 'app/views/profile/index.html',
				controller: 'profileController'
			});

		//$locationProvider.html5Mode(true);
	});