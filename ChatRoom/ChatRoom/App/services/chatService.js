app.factory('chatService', function ($http, $cookieStore) {

	var shareData = {

		onlineUsers: [],

		rooms: [
			{
				name: 'user 1', token: '', userIds: [], isOpen: true, messages:
					[
						{ isUser: true, message: 'Hello' },
						{ isUser: true, message: 'hi' },
						{ isUser: false, message: 'hi :P :D :D ' },
						{ isUser: true, message: '<img src="http://img4.wikia.nocookie.net/__cb20061223084010/lostpedia/images/e/e4/Big_smile.gif" />' }
					]
			},
			{
				name: 'user 2', token: '', userIds: [], isOpen: false, messages:
					[

					]
			},
		],

		openRoom: {}
	};

	return shareData;
});