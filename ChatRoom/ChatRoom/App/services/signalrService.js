app.factory('signalrService', function ($http, $cookieStore) {

	var signalrSession = {};

	var connection;
	var connectionUrl = "http://localhost:47806/signalR";
	var COOKIEUSER_KEY = "CURRENT_USER";


	signalrSession.startListening = function (getOnlineUsers, getOnlineUser, wentOffline) {
		//Configure connection
		connection = $.connection;
		connection.hub.url = connectionUrl;
		connection.hub.logging = true;
		connection.hub.qs = "token=" + $cookieStore.get(COOKIEUSER_KEY).token;

		connection.chatRoom.client.getWhoCameOnline = function (data) {
			getOnlineUser(data);
		}

		connection.chatRoom.client.getOnlineUsers = function (data) {
			getOnlineUsers(data);
		}

		connection.chatRoom.client.wentOffline = function (data) {
			wentOffline(data); 
		}

		//Start hub connection
		connection.hub.start({ jsonp: true })
			.done(function () {
				console.log("connected to >> " + connectionUrl);
			});
	}

	signalrSession.sendMessage = function (msg) {

	}



	return signalrSession;
});