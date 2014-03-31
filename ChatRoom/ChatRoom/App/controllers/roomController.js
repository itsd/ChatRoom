app.controller('roomController', function ($scope, $http, socketService) {

	$scope.writeText = '';

	$scope.chatArray =
		[
			{ id: 1, text: 'Message 1', timeAgo: 5 },
			{ id: 2, text: 'Message 2', timeAgo: 4 },
			{ id: 3, text: 'Message 3', timeAgo: 3 },
			{ id: 4, text: 'Message 4', timeAgo: 2 },
			{ id: 5, text: 'Message 5', timeAgo: 2 }
		];

	$scope.roomArray = [
		//{ id: 1, userTo: "Username 1" },
		//{ id: 2, userTo: "Username 2" },
		//{ id: 3, userTo: "Username 3" },
	];

	$scope.messagesArray = [
		{ comment: "Hello", isYou: false },
		{ comment: "Hi", isYou: true },
		{ comment: "this is a message ... ", isYou: false },
		{ comment: "this is a message ... kmna sdas dasd aks daskdaksh daskd aksd qjh dasmd asdk ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... kmna sdas dasd aks daskdaksh daskd aksd qjh dasmd asdk ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... ", isYou: true },
		{ comment: "this is a message ... ", isYou: false },
		{ comment: "this is a message ... ja  asdk asdk asdk aiwqjekas daskd qkwje askd as,md aeqwkje qwk asmd as,md ak qweqw me ,mas d,mas dqwjenqkwm d,amsd ", isYou: true },
	];

	$scope.onlineUsers = [
		//{ ID: 1, Username: "Username 1" },
		//{ ID: 2, Username: "Username 2" },
		//{ ID: 3, Username: "Username 3" },
		//{ ID: 4, Username: "Username 4" }
	];

	$scope.send = function () {
		//$scope.chatArray.push({ text: $scope.writeText, timeAgo: 1 });
		//$scope.writeText = '';
		//$(".room").animate({ scrollTop: $(".room").get(0).scrollHeight }, 'slow');

		socketService.sendMessage($scope.writeText);
		$scope.writeText = '';
	}

	$scope.getMessage = function (msg) {
		//$scope.chatArray.push({ text: msg, timeAgo: 1 });
		//$scope.$apply();
		//$(".room").animate({ scrollTop: $(".room").get(0).scrollHeight }, 'slow');

		var response = JSON.parse(msg);
		switch (response.Type) {
			case 1: $scope.onlineUsers = response.Users; break;
			case 2: $scope.onlineUsers.push({ ID: response.ID, Username: response.Username }); break;
			case 3: $scope.onli.remove();

			default: break;
		}

		console.log(response);

		$scope.$apply();
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
		$scope.roomArray.push({ id: index, userTo: "Username " + index });
	}

	socketService.startListening($scope.getMessage);
});