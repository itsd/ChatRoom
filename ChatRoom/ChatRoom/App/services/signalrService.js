app.factory('signalrService', function ($http, $cookieStore, chatService) {

	var signalrSession = {
		isInitialized: false,
		roomScope: {}
	};

	var connection;
	var connectionUrl = "http://localhost:47806/signalR";
	var COOKIEUSER_KEY = "CURRENT_USER";

	signalrSession.startListening = function (getChatUsers, getOnlineUser, wentOffline, callBack) {
		//Configure connection
		connection = $.connection;
		connection.hub.url = connectionUrl;
		//connection.hub.logging = true;
		//connection.hub.transportConnectTimeout = 50;
		connection.hub.qs = "token=" + $cookieStore.get(COOKIEUSER_KEY).token;

		connection.chatRoom.client.getWhoCameOnline = function (data) {
			getOnlineUser(data);
			callBack();
		}

		connection.chatRoom.client.getChatUsers = function (data) {
			getChatUsers(data);
			callBack();
		}

		connection.chatRoom.client.wentOffline = function (data) {
			wentOffline(data);
			callBack();
		}

		connection.chatRoom.client.getMessage = function (token, message, createdBy, userIds) {

			console.log("You got a new message ...");

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

			if (Object.keys(signalrSession.roomScope).length !== 0) {
				signalrSession.roomScope.$apply();
			}

		}

		//Start hub connection
		//connection.hub.start({ jsonp: true/*, transport: 'webSockets'*/ })     longPolling
		connection.hub.start({ jsonp: true, transport: 'longPolling' })
			.done(function () {
				console.log("connected to >> " + connectionUrl);
				$("#connectionID").html($.connection.hub.id);
			})
			.fail(function () { console.log("could not connect to " + connectionUrl); });

		connection.hub.disconnected(function () {
			//alert("You went offline");
		});
	}

	signalrSession.stopListening = function () {
		connection.hub.stop();
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

		console.log("Listening messages ...");


	}

	signalrSession.setScope = function (scope) {
		signalrSession.roomScope = scope;
	}

	return signalrSession;
});