app.controller('headerController', function ($scope, sessionService) {
	$scope.xxx = "Hello World";

	$scope.isAuthorised = sessionService.isAuthenticated;

	$scope.username = sessionService.user.username;

	$scope.showHeader = function () {
		return sessionService.isAuthenticated;
	}
});