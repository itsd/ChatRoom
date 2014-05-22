app.controller('profileController', function ($scope, $http, sessionService) {
	$scope.isEdit = false;

	$scope.user = null;

	$scope.username = sessionService.user.username;
	$scope.name = sessionService.user.name;

	$scope.dialog = {
		isOpen: false,
		title: 'Upload Image',
		closeDialog: function () {
			this.isOpen = false;
		},
		openDialog: function () {
			this.isOpen = true;
		}
	};

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

		$http.post(api("account/updateProfile"), { name: $scope.user.name, username: $scope.user.username, email: $scope.user.email })
		.success(function (data, status, headers, config) {
			//$scope.isEdit = false;
		})
		.error(function (data, status, headers, config) {

		});
	};
});