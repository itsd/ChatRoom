app.factory('signalrService', function ($http, $cookieStore) {

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

	signalrSession.sendMessage = function (msg) {

	}

	return signalrSession;
});