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

		$routeProvider.when('/room',
			{
				templateUrl: 'app/views/room/index.html',
				controller: 'roomController'
			});

		//$locationProvider.html5Mode(true);
	});