app.factory('shareService', function ($http, $cookieStore) {

	var shareData = {
		shareProperty: 'I am shared property',
		onlineUsers: [
			{ id: 1, username: 'user 1' },
			{ id: 2, username: 'user 2' },
			{ id: 3, username: 'user 3' },
			{ id: 4, username: 'user 4' },
			{ id: 5, username: 'user 5' },
			{ id: 6, username: 'user 6' },
			{ id: 7, username: 'user 7' }
		]
	};

	return shareData;
});