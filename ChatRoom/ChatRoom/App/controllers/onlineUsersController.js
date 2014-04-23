app.controller('onlineUsersController', function ($scope, $http, chatService, signalrService, sessionService) {

	$scope.onlineUsers = chatService.onlineUsers;

	$scope.changeRoom = function (index) {
		if (sessionService.isAuthenticated) {

			selectedUser = chatService.onlineUsers[index];
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

			selectedRoom = chatService.rooms.where(function (obj) {
				if (obj.id == newGroupID) return true;
			});

			chatService.openRoom.name = selectedRoom[0].name;
			chatService.openRoom.token = selectedRoom[0].token;
			chatService.openRoom.id = selectedRoom[0].id;
			chatService.openRoom.isOpen = true;
			chatService.openRoom.messages = selectedRoom[0].messages;
			chatService.openRoom.userIds = selectedRoom[0].userIds;

		}
	}

	signalrService.startListening(
		function (data) { // U got online users 
			for (var i = 0; i < data.length; i++) {
				chatService.onlineUsers.push({ id: data[i].id, username: data[i].username });
			}
		},
		function (data) { // U got online user
			console.log("u got online user");
			console.log(data);
		},
		function (data) { // U got offline user
			console.log("u got offline user");
			console.log(data);
		},
		function () { // call back
			$scope.$apply();
		}
	)
});