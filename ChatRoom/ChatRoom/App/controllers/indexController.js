app.controller('indexController', function ($scope, $location, sessionService) {
	$scope.session = sessionService;

	if (!sessionService.isAuthenticated) $location.path('/login');

	if (sessionService.isAuthenticated) $location.path('/room');
});