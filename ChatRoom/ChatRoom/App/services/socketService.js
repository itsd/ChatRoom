//app.factory('socketService', function ($http, $cookieStore) {

//	var COOKIEUSER_KEY = "CURRENT_USER";
//	var SOCKET_SERVER_URL = "ws://localhost:20094/server.ashx?token=" + $cookieStore.get(COOKIEUSER_KEY).token;
//	var socketSession = {};

//	var socket = new WebSocket(SOCKET_SERVER_URL);

//	socketSession.startListening = function (callBack) {


//		socket.onopen = function () {
//			//alert("Socket has been opened!");
//		}

//		socket.onmessage = function (msg) {
//			//console.log(msg.data);

//			callBack(msg.data);
//		}

//		socket.onerror = function () {
//			alert("Error ...");
//		}
//	};

//	socketSession.sendMessage = function (msg) {
//		//socket.send(JSON.parse(msg));
//		socket.send(msg);
//	}

//	socketSession.getChatUsers = function () { }

//	return socketSession;
//});