app.factory('signalrService', function ($http, $cookieStore, chatService) {

	var signalrSession = {
		isInitialized: false,
		roomScope: {}
	};

	var connection;
	var COOKIEUSER_KEY = "CURRENT_USER";

	signalrSession.startListening = function (getChatUsers, getOnlineUser, wentOffline, onMessageCallBack, onYourMessageCallBack, callBack) {
		//Configure connection
		connection = $.connection;
		connection.hub.url = configuration.signalR.serverUrl;
		connection.hub.logging = configuration.signalR.logging;
		//connection.hub.transportConnectTimeout = configuration.signalR.transportConnectTimeout;
		connection.hub.qs = { 'token': $cookieStore.get(COOKIEUSER_KEY).token };

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

		connection.chatRoom.client.getMessage = function (data) {
			if (onMessageCallBack) { onMessageCallBack(data); }
		}

		connection.chatRoom.client.getYourMessage = function (data) {
			if (onYourMessageCallBack) { onYourMessageCallBack(data); }
		}

		//Start hub connection
		connection.hub.start({ jsonp: configuration.signalR.jsonp, transport: configuration.signalR.transport })
			.done(function (x, y, z) {
				console.log("connected to >> " + configuration.signalR.serverUrl);
			})
			.fail(function () {
				console.log("could not connect to " + configuration.signalR.serverUrl);
			});

		connection.hub.disconnected(function () {
			chatService.chatUsers = [];
			console.log("disconnected from " + configuration.signalR.serverUrl);
		});
	}

	signalrSession.stopListening = function () {
		connection.hub.stop();
	}

	signalrSession.sendMessageTo = function (msg, roomToken, roomUsers, callBack) {
		connection.hub.qs = { 'token': $cookieStore.get(COOKIEUSER_KEY).token, 'groupToken': roomToken };

		connection.chatRoom.server.sendMessage(msg, roomUsers)
		.done(function (data) {
			if (callBack) callBack(data);
		});
	}

	signalrSession.setScope = function (scope) {
		signalrSession.roomScope = scope;
	}

	return signalrSession;
});