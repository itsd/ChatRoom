app.controller('chatUsersController', function ($scope, $http, chatService, signalrService, sessionService) {
	$scope.isAuthorised = sessionService.isAuthenticated;

	$scope.chatUsers = chatService.chatUsers;

	$scope.changeRoom = function (userId, username) {
		if (sessionService.isAuthenticated) {
			var room = $scope.rooms.where(function (obj) { if (obj.widthUser == userId) return true; });
			if (room.length == 1) {
				room[0].isOpen = true;
			} else {
				newRoomId = $scope.rooms.length + 1;
				$scope.rooms.push(
					{
						id: newRoomId,
						isOpen: true,
						name: username,
						widthUser: userId,
						currentMessage: '',
						messages: []
					}
				);
			}
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

	$scope.closeItem = function (obj) {
		alert($(obj.target).attr("class"));
	}

	$scope.rooms = [
		//{
		//	id: 1,
		//	isOpen: false,
		//	name: 'sa',
		//	widthUser: 1,
		//	currentMessage: '',
		//	messages: [
		//	  {
		//	  	isUser: true,
		//	  	message: 'hello'
		//	  },


		//	  {
		//	  	isUser: true,
		//	  	message: 'a asd asd asd ssss as kjask djaskjdhkjashdkjas h kashd kasdhj askjdh askd haksdh'
		//	  },
		//	  {
		//	  	isUser: true,
		//	  	message: 'a asd asd asd ssss as kjask djaskjdhkjashdkjas h kashd kasdhj askjdh askd haksdh'
		//	  },

		//	]
		//},
		//{
		//	id: 2,
		//	isOpen: false,
		//	name: 'sasa',
		//	widthUser: 2,
		//	currentMessage: '',
		//	messages: [
		//		{
		//			isUser: false,
		//			message: 'hello'
		//		}]
		//},
	];

	$scope.openRoom = function (roomId) {
		$scope.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0].isOpen = true;
	}

	$scope.closeRoom = function (roomId) {
		$scope.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0].isOpen = false;
	}

	$scope.sendMessage = function (roomId) {

		var inRoom = $scope.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0];

		if (inRoom.currentMessage != '') {
			inRoom.messages.push({ isUser: false, message: inRoom.currentMessage });
			$(".body-chat-content").animate({ scrollTop: $(".body-chat-content").get(0).scrollHeight }, 'slow');
			inRoom.currentMessage = '';
		}
	}

	if (sessionService.isAuthenticated) {
		$scope.connectToUsers();
	} else {
		sessionService.initializeLoginSuccess($scope.connectToUsers);
	}
});