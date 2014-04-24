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

	signalrService.startListening(
		function (data) { // U got online users 
			for (var i = 0; i < data.length; i++) {
				chatService.onlineUsers.push({ id: data[i].id, username: data[i].username });
			}
		},
		function (data) { // U got online user 
			chatService.onlineUsers.push({ id: data.id, username: data.username });
			console.log("got online user");
		},
		function (data) { // U got offline user 
			chatService.onlineUsers.remove(chatService.onlineUsers.where(function (x) { if (x.id == data.id) return true; })[0]);
			console.log("Got offline user");
		},
		function () { // call back
			$scope.$apply();
		}
	)
});