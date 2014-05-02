app.controller('indexController', function ($scope, $location, sessionService) {
	$scope.session = sessionService;
	 
	if (!sessionService.isAuthenticated) $location.path('/login');

	if (sessionService.isAuthenticated) $location.path('/room');
});

app.controller('statusController', function ($scope, sessionService) {
	$scope.session = sessionService;
});