
var isDebug = true;

var configuration = {
	api: {
		Url: isDebug ? 'http://localhost:10512' : 'http://localhost/chatAPI',
	},

	viewUrlPrefix: isDebug ? 'http://localhost:10511' : 'http://localhost/chat',

	signalR: {
		// serverUrl = "http://localhost:47806/signalR";
		//serverUrl: 'http://localhost/chatServer/signalr',
		serverUrl: isDebug ? 'http://localhost:47806/signalr' : 'http://localhost/chatServer/signalr',
		transportConnectTimeout: 50,
		logging: true,
		jsonp: true,
		transport: ['webSockets', 'longPolling']
	}
}