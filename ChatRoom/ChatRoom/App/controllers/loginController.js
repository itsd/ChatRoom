app.controller('loginController', function ($scope, $http, $controller, $location, sessionService) {

	$controller('baseController', { $scope: $scope });

	$scope.loading = false;
	$scope.username = '';
	$scope.password = '';
	$scope.errorMessage = ''

	$scope.items = [];

	$scope.login = function () {
		$scope.loading = true;
		$scope.errorMessage = ''

		sessionService.login($scope.username, $scope.password,
			function () {
				$scope.loading = false;
				$location.path('/');
			},
			function (data, status) {
				if (status == 403) {
					$scope.errorMessage = 'Incorrect username or password';
				} else {
					$scope.errorMessage = 'Could not connect to the server, please try again later';
				}
				$scope.loading = false;
			});
	}
});

app.controller('logoutController', function ($scope, $location, sessionService, signalrService) {

	$scope.loading = true;
	sessionService.logout(function () {
		$scope.loading = false;
		signalrService.stopListening();
		$location.path('/');
	});
});