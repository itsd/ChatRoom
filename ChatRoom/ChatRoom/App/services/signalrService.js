app.factory('signalrService', function ($http, $cookieStore, chatService) {

	var signalrSession = {
		isInitialized: false
	};

	var connection;
	var connectionUrl = "http://localhost:47806/signalR";
	var COOKIEUSER_KEY = "CURRENT_USER";

	signalrSession.startListening = function (getOnlineUsers, getOnlineUser, wentOffline, callBack) {
		//Configure connection
		connection = $.connection;
		connection.hub.url = connectionUrl;
		//connection.hub.logging = true;
		connection.hub.qs = "token=" + $cookieStore.get(COOKIEUSER_KEY).token;

		connection.chatRoom.client.getWhoCameOnline = function (data) {
			getOnlineUser(data);
			callBack();
		}

		connection.chatRoom.client.getOnlineUsers = function (data) {
			getOnlineUsers(data);
			callBack();
		}

		connection.chatRoom.client.wentOffline = function (data) {
			wentOffline(data);
			callBack();
		}

		//Start hub connection
		connection.hub.start({ jsonp: true })
			.done(function () { console.log("connected to >> " + connectionUrl); })
			.fail(function () { console.log("could not connect to" + connectionUrl); });
	}

	signalrSession.sendMessageTo = function (msg, room, callBack) {
		connection.hub.qs = { 'token': $cookieStore.get(COOKIEUSER_KEY).token, 'groupToken': room.token };

		connection.chatRoom.server.sendMessage(msg, room.userIds)
		.done(function (data) {
			if (room.token == '') {
				chatService.openRoom.token = data;
			}
			chatService.openRoom.messages.push({ isUser: false, message: msg });
			callBack();
		});
	}

	signalrSession.listenMessages = function (callBack) {

		connection.chatRoom.client.getMessage = function (token, message, createdBy, userIds) {

			console.log(chatService.openRoom);

			if (Object.keys(chatService.openRoom).length === 0) {
				console.log("We don't have open room");
				chatService.openRoom.name = createdBy;
				chatService.openRoom.token = token;
				chatService.openRoom.id = 1;
				chatService.openRoom.isOpen = true;
				chatService.openRoom.userIds = userIds;
				chatService.openRoom.messages = [];
				chatService.openRoom.messages.push({ isUser: true, message: message });
			} else {

				if (chatService.openRoom.token == token) {
					chatService.openRoom.messages.push({ isUser: true, message: message });
				} else {
					alert("You got new notification in other room");
				}
			}

			//if (chatService.openRoom != null && chatService.openRoom.token == token) {
			//	//message came in current room
			//	chatService.openRoom.messages.push({ isUser: true, message: message });
			//} else {

			//	chatService.openRoom.name = createdBy;
			//	chatService.openRoom.token = token;
			//	chatService.openRoom.id = 1;
			//	chatService.openRoom.isOpen = true;
			//	chatService.openRoom.userIds = userIds;
			//	chatService.openRoom.messages = [];
			//	chatService.openRoom.messages.push({ isUser: true, message: message });
			//}
			callBack();
		}
	}

	return signalrSession;
});