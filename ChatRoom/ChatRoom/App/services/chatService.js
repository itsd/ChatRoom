﻿app.factory('chatService', function ($http, $cookieStore) {

	var shareData = {

		chatUsers: [
			//{ id: 1, username: 'sa', isOnline: false },
			//{ id: 2, username: 'sasa', isOnline: false },
			//{ id: 3, username: 'sasasa', isOnline: false }
		],

		rooms: [],

		openRoom: {}
	};

	shareData.addOnlineUser = function (user, callBack) {
		var userInList = shareData.chatUsers.where(function (obj) { if (obj.id == user.id) return true; });

		if (userInList.length == 1) {
			userInList[0].isOnline = true;
		} else {
			//temporary rem this
			//shareData.chatUsers.push({ id: user.id, username: user.username, isOnline: true });
		}

		if (callBack) { callBack(); }
	}

	shareData.removeOnlineUser = function (user, callBack) {
		var userInList = shareData.chatUsers.where(function (obj) { if (obj.id == user.id) return true; });

		if (userInList.length == 1) {
			userInList[0].isOnline = false;
		}

		if (callBack) { callBack(); }
	}

	shareData.getFriends = function (callBack) {

		$http.get(api('user/friends')).success(
				function (data, status, headers, config) {

					for (var i = 0; i < data.length; i++) {
						shareData.chatUsers.push({ id: data[i].id, username: data[i].username, isOnline: false });
					}

					if (callBack) { callBack(); }
				}
			);

	}

	return shareData;
});