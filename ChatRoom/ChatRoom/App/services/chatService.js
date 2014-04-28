app.factory('chatService', function ($http, $cookieStore) {

	var shareData = {

		chatUsers: [
			{ id: 1, username: 'username 1', isOnline: true },
			{ id: 2, username: 'username 2', isOnline: false },
			{ id: 3, username: 'username 3', isOnline: true }
		],

		rooms: [],

		openRoom: {}
	};

	return shareData;
});