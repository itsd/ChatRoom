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

	angular.element(document).ready(function () {
		if (sessionService.isAuthenticated) {
			signalrService.startListening(
				function (data) { // U got online users 
					for (var i = 0; i < data.length; i++) {
						chatService.chatUsers.push({ id: data[i].id, username: data[i].username });
					}
				},
				function (data) { // U got online user 
					//chatService.chatUsers.push({ id: data.id, username: data.username });
					console.log("+Came online");
					console.log("Online users count is: " + data);
				},
				function (data) { // U got offline user 
					//chatService.chatUsers.remove(chatService.chatUsers.where(function (x) { if (x.id == data.id) return true; })[0]);
					console.log("-Went offline");
					console.log("Online users count is: " + data);
				},
				function () { // call back
					$scope.$apply();
				}
			);
		}
	});
});