app.controller('chatUsersController', function ($scope, $http, chatService, signalrService, sessionService) {

	$scope.chatUsers = chatService.chatUsers;

	$scope.changeRoom = function (index) {
		if (sessionService.isAuthenticated) {

			selectedUser = chatService.chatUsers[index];
			newGroupID = chatService.rooms.length + 1;

			newRoom = {
				name: selectedUser.username,
				token: '',
				id: newGroupID,
				userIds: [sessionService.user.userID, selectedUser.id],
				isOpen: true,
				messages: []
			}

			chatService.rooms.push(newRoom);

			//selectedRoom = chatService.rooms.where(function (obj) {
			//	if (obj.id == newGroupID) return true;
			//});

			chatService.openRoom.name = newRoom.name;
			chatService.openRoom.token = newRoom.token;
			chatService.openRoom.id = newRoom.id;
			chatService.openRoom.isOpen = true;
			chatService.openRoom.messages = newRoom.messages;
			chatService.openRoom.userIds = newRoom.userIds;

		}
	}

	$scope.connectToUsers = function () {

		signalrService.startListening(
				function (data) { // U got online users 
					for (var i = 0; i < data.length; i++) {
						chatService.addOnlineUser(data[i], function () {
							$scope.$apply();
						});
					}
				},
				function (data) { // U got online user
					chatService.addOnlineUser(data, function () { $scope.$apply(); });
				},
				function (data) { // U got offline user 
					chatService.removeOnlineUser(data, function () { $scope.$apply(); });
				},
				function () { // call back
					$scope.$apply();
				}
			);
	}

	$scope.stopConnection = function () {
		signalrService.stopListening();
	}

	if (sessionService.isAuthenticated) {
		$scope.connectToUsers();
	} else {
		sessionService.initializeLoginSuccess($scope.connectToUsers);
	}
});