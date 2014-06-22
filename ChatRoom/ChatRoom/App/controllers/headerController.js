app.controller('headerController', function ($scope, sessionService, signalrService) {
	$scope.session = sessionService;
});