
var isDebug = true;

var configuration = {
	apiUrl: isDebug ? 'http://localhost:10512' : 'http://localhost/chatAPI',
	viewUrlPrefix: isDebug ? 'http://localhost:10511' : 'http://localhost/chat',
	signalRUrl: isDebug ? 'http://localhost:47806/signalr' : 'http://localhost/chatServer/signalr/'
}