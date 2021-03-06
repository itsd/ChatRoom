﻿app.controller('chatUsersController', function ($scope, $http, chatService, signalrService, sessionService) {
	$scope.isAuthorised = sessionService.isAuthenticated;

	$scope.showMoreItems = 0;

	$scope.showSettingBubble = false;

	$scope.chat = chatService;

	$scope.session = sessionService;

	$scope.changeRoom = function (userId, username) {
		if (sessionService.isAuthenticated) {
			var room = chatService.rooms.where(function (obj) { if (obj.toUser == userId) return true; });
			if (room.length == 1) {
				room[0].isOpen = true;
			} else {
				newRoomId = chatService.rooms.length + 1;
				chatService.rooms.push(
					{
						id: newRoomId,
						token: '',
						isOpen: true,
						name: username,
						toUser: userId,
						userIds: [userId, sessionService.user.userID],
						currentMessage: '',
						messages: []
					}
				);

				$scope.showMoreItems++;
			}
		}


	}

	$scope.connectToUsers = function () {

		chatService.getFriends(
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
				function (data) {
					var room = chatService.rooms.where(function (obj) { if (obj.token == data.roomToken) return true; });
					if (room.length == 1) {
						room[0].isOpen = true;
						room[0].messages.push({ isUser: true, message: data.message });
					} else {

						//Let find room with userId
						var toUserId = data.userIds.where(function (obj) { if (obj != sessionService.user.userID) return true; });

						if (toUserId.length == 1) {
							toUserId = toUserId[0];
						} else {
							toUserId = -1;
						}

						var room = chatService.rooms.where(function (obj) { if (obj.toUser == toUserId) return true; });

						if (room.length == 1) {
							room[0].isOpen = true;
							room[0].messages.push({ isUser: true, message: data.message });
						} else {
							newRoomId = chatService.rooms.length + 1;
							chatService.rooms.push(
								{
									id: newRoomId,
									token: data.roomToken,
									isOpen: true,
									name: data.username,
									userIds: data.userIds,
									toUser: toUserId,
									currentMessage: '',
									messages: [{ isUser: true, message: data.message }]
								}
							);
						}
					}

					$scope.$apply();
					$scope.playSound();

					$(".body-chat-content").animate({ scrollTop: $(".body-chat-content").get(0).scrollHeight }, 'slow');
				},
				function (data) {
					var room = chatService.rooms.where(function (obj) { if (obj.token == data.roomToken) return true; });
					if (room.length == 1) {
						room[0].isOpen = true;
						room[0].messages.push({ isUser: false, message: data.message });

					} else {
						newRoomId = chatService.rooms.length + 1;
						chatService.rooms.push(
							{
								id: newRoomId,
								token: data.roomToken,
								isOpen: true,
								name: sessionService.user.username,
								userIds: data.userIds,
								toUser: data.userIds.where(function (obj) { if (obj != sessionService.user.userID) return true; })[0],
								currentMessage: '',
								messages: [{ isUser: false, message: data.message }]
							}
						);
					}
					$scope.$apply();
					$(".body-chat-content").animate({ scrollTop: $(".body-chat-content").get(0).scrollHeight }, 'slow');
				},
				function () { // call back
					$scope.$apply();
				}
			)
		);
	}

	$scope.stopConnection = function () {
		signalrService.stopListening();
	}

	$scope.closeItem = function (obj) {
		alert($(obj.target).attr("class"));
	}

	$scope.rooms = chatService.rooms;

	$scope.openRoom = function (roomId) {
		chatService.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0].isOpen = true;
	}

	$scope.closeRoom = function (roomId) {
		chatService.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0].isOpen = false;
		//$scope.showMoreItems--;
	}

	$scope.removeRoom = function (roomId) {
		chatService.rooms.removeWithProperty(function (obj) { if (obj.id == roomId) return true; });
		$scope.showMoreItems--;
	}

	$scope.sendMessage = function (roomId, roomToken) {

		var inRoom;
		if (roomToken != '') {
			inRoom = chatService.rooms.where(function (obj) { if (obj.token == roomToken) return true; })[0];
		} else {
			inRoom = chatService.rooms.where(function (obj) { if (obj.id == roomId) return true; })[0];
		}

		if (inRoom.currentMessage != '') {
			messageText = inRoom.currentMessage;
			inRoom.currentMessage = '';

			signalrService.sendMessageTo(messageText, inRoom.token, inRoom.userIds, function (data) {
				inRoom.messages.push({ isUser: false, message: messageText });
				inRoom.token = data;

				$scope.$apply();
			});

			$(".body-chat-content").animate({ scrollTop: $(".body-chat-content").get(0).scrollHeight }, 'slow');
		}
	}

	$scope.playSound = function () {
		var hiddenAudio = $('.hiddenPlayer');
		var soundAudio = $('.chat_audio').html();
		hiddenAudio.html('');
		hiddenAudio.append(soundAudio);
	}

	$scope.showSettingBubbles = function () {
		$scope.showSettingBubble = !$scope.showSettingBubble;
	}

	//if (sessionService.isAuthenticated) {
	//	console.log('is authorized');
	//	$scope.connectToUsers();
	//} else {
	//	sessionService.initializeLoginSuccess($scope.connectToUsers);
	//	console.log('is not authorized');
	//}

	if (sessionService.isAuthenticated) {
		$scope.connectToUsers();
	}

	//Don't like this ...
	$scope.$watch(function () {
		return sessionService.isAuthenticated;
	}, function (newVal, oldVal) {

		if (newVal === oldVal) { return; }

		if (sessionService.isAuthenticated) {
			$scope.connectToUsers();
		}
	}, true);

});