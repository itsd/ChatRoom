app.factory('signalrService', function ($http, $cookieStore, chatService) {

	var signalrSession = {
		isInitialized: false,
		roomScope: {}
	};

	var connection;
	var connectionUrl = configuration.signalRUrl;
	var COOKIEUSER_KEY = "CURRENT_USER";

	signalrSession.startListening = function (getChatUsers, getOnlineUser, wentOffline, onMessageCallBack, onYourMessageCallBack, callBack) {
		//Configure connection
		connection = $.connection;
		connection.hub.url = connectionUrl;
		//connection.hub.logging = true;
		//connection.hub.transportConnectTimeout = 50;
		//connection.hub.qs = "token=" + $cookieStore.get(COOKIEUSER_KEY).token;
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
		//connection.hub.start({ jsonp: true/*, transport: 'webSockets'*/ })     longPolling
		connection.hub.start({ jsonp: true, transport: 'webSockets' })
			.done(function () {
				console.log("connected to >> " + connectionUrl);
				$("#connectionID").html($.connection.hub.id);
			})
			.fail(function () { console.log("could not connect to " + connectionUrl); });

		connection.hub.disconnected(function () {
			//alert("You went offline");
		});
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