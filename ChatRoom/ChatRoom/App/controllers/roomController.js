app.controller('roomController', function ($scope, $rootScope, $http, signalrService) {

	$scope.currentMessage = '';

	$scope.onlineUsers = [
		{ id: 1, username: 'user 1' },
		{ id: 2, username: 'user 2' },
		{ id: 3, username: 'user 3' },
		{ id: 4, username: 'user 4' },
		{ id: 5, username: 'user 5' },
		{ id: 6, username: 'user 6' },
		{ id: 7, username: 'user 7' }
	];

	$scope.currentTalk = {
		username: 'user 1',

		messages: [
				{ isUser: true, message: 'Hello' },
				//{ isUser: false, message: 'hi' },
				//{ isUser: false, message: 'hi' },
				//{ isUser: false, message: 'hi' },
				//{ isUser: false, message: 'hi' },
				//{ isUser: false, message: 'hiiiiiiiiiiiiiiiiiiiiiiii' },
				//{ isUser: false, message: 'hi' },
				//{ isUser: false, message: 'hi' }
		]
	};

	$scope.talking = function () {
		if ($scope.currentMessage != '') {
			$scope.currentTalk.messages.push({ isUser: false, message: $scope.currentMessage });
			$scope.currentMessage = '';
			$(".conversation-content-inner").animate({ scrollTop: $(".conversation-content-inner").get(0).scrollHeight }, 'slow');
		}
	}

	////$scope.talkTo = function (index) {
	//$scope.$on('talkTo', function (evt, data) {
	//	$scope.currentTalk.username = $scope.onlineUsers[data.index].username;
	//});

	//$scope.xxxx = function (index) {
	//	$rootScope.$broadcast('talkTo', { index: index });
	//}

	$scope.talkTo = function (index) {
		$scope.currentTalk.username = $scope.onlineUsers[index].username;


	};


	$scope.send = function () {
		//$scope.chatArray.push({ text: $scope.writeText, timeAgo: 1 });
		//$scope.writeText = '';
		//$(".room").animate({ scrollTop: $(".room").get(0).scrollHeight }, 'slow');

		socketService.sendMessage($scope.writeText);
		$scope.writeText = '';
	}

	$scope.increaseTimeAgo = function () {
		setInterval(function () {
			for (var i = 0; i < $scope.chatArray.length; i++) {
				$scope.chatArray[i].timeAgo += 1;
			}
			$scope.$apply();
		}, 1000);
	}

	$scope.openChat = function (index) {
		$(".opener-link-" + index).parent().children(".item-content-" + index).toggleClass("shower-class");
		$(".opener-link-" + index).parent().toggleClass("item-open");
		$(".opener-link-" + index).hide();
	}

	$scope.openChatItem = function (index) {
		$scope.roomArray[index].isOpen = true;
	}

	$scope.closeChatItem = function (index) {
		$scope.roomArray[index].isOpen = false;
	}

	$scope.closeChat = function (index) {
		$(".closer-link-" + index).parent().toggleClass("shower-class");
		$(".closer-link-" + index).parent().parent().toggleClass("item-open");
		$(".closer-link-" + index).parent().parent().children(".opener-link-" + index).show();
	}

	$scope.openTabletUsers = function () {
		$(".online-users-content").show();
		$(".users-opener").hide();
	}

	$scope.startTalk = function (index) {
		socketService.sendMessage(JSON.stringify({
			type: 1,
			userids: [$scope.onlineUsers[index].ID]
		}));
	}
});