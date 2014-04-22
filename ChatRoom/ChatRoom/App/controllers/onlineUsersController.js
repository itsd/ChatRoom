app.controller('onlineUsersController', function ($scope, $http, chatService, signalrService, sessionService) {

	$scope.onlineUsers = chatService.onlineUsers;

	$scope.changeRoom = function (index) {
		if (sessionService.isAuthenticated) {

			selectedUser = chatService.onlineUsers[index];

			newRoom = {
				name: selectedUser.username,
				token: '',
				userIds: [selectedUser.id, sessionService.user.userID],
				isOpen: true,
				messages: []
			}


			 
			//chatService.rooms.push(newRoom);

			//selectedRoom = chatService.rooms[index];

			//chatService.openRoom.name = selectedRoom.name;
			//chatService.openRoom.token = selectedRoom.token;
			//chatService.openRoom.isOpen = true;
			//chatService.openRoom.messages = selectedRoom.messages;

			//chatService.openRoom.userIds = [];
			//chatService.openRoom.userIds.push();
			//chatService.openRoom.userIds.push();
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