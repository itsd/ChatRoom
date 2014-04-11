app.controller('loginController', function ($scope, $http, $controller, $location, sessionService) {

	$controller('baseController', { $scope: $scope });

	$scope.loading = false;
	$scope.username = '';
	$scope.password = '';
	$scope.errorMessage = ''

	$scope.items = [];

	$scope.login = function () {
		$scope.loading = true;
		sessionService.login($scope.username, $scope.password,
			function () {
				$scope.loading = false;
				$location.path('/');
			},
			function () {
				$scope.loading = false;
			});
	}
});

app.controller('logoutController', function ($scope, $location, sessionService) {

	$scope.loading = true;
	sessionService.logout(function () {
		$scope.loading = false;
		$location.path('/');
	});
});