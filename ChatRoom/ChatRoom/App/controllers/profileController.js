app.controller('profileController', function ($scope, $http, sessionService) {
	$scope.isEdit = false;

	$scope.user = null;

	$scope.username = sessionService.user.username;
	$scope.name = sessionService.user.name;

	$scope.getUser = function () {
		$http.get(api('user/')).success(
			function (data, status, headers, config) {
				$scope.user = data;

				console.log(data);
			}
		);
	}; $scope.getUser();

	$scope.showEdit = function () {
		$scope.isEdit = true;
	};

	$scope.saveSettings = function () {
		$scope.isEdit = false;
	};
});