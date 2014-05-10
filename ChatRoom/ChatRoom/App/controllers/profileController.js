app.controller('profileController', function ($scope, $http, sessionService) {
	$scope.isEdit = false;
	$scope.username = sessionService.user.username;
	$scope.name = sessionService.user.name;

	$scope.showEdit = function () {
		$scope.isEdit = true;
	};

	$scope.saveSettings = function () {
		$scope.isEdit = false;
	}
});