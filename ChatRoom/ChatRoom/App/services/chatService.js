app.factory('chatService', function ($http, $cookieStore) {

	var shareData = {

		onlineUsers: [],

		rooms: [],

		openRoom: {}
	};

	return shareData;
});