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
		//{ id: 1, roomName: "Username 1" },
		//{ id: 2, roomName: "Username 2" },
		//{ id: 3, roomName: "Username 3" },
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

	$scope.getSocketResponse = function (msg) {
		//$scope.chatArray.push({ text: msg, timeAgo: 1 });
		//$scope.$apply();
		//$(".room").animate({ scrollTop: $(".room").get(0).scrollHeight }, 'slow');

		var response = JSON.parse(msg);

		switch (response.Type) {
			case 1:
				$scope.onlineUsers = response.Users;
				break;

			case 2:
				$scope.onlineUsers.push({ ID: response.ID, Username: response.Username });
				break;

			case 3:
				$scope.onlineUsers.removeWithID(function (x) { if (x.ID == response.ID) return true; });
				break;

			case 4:

				var response = JSON.parse(msg);

				$scope.roomArray.push({
					id: 123,
					roomName: 'room name',
					roomToken: response.RoomID,
					isOpen: true,
					currentMessage: '',
					messages: [
							//{ comment: "Hello", isYou: false },
							//{ comment: "Hi", isYou: true },
					]
				}); 

				break;

			case 5:
				var response = JSON.parse(msg);
				var postedRoom = $scope.roomArray.where(function (x) { if (x.roomToken == response.RoomToken) return true; });

				if (postedRoom == "") {
					$scope.roomArray.push({
						id: 123,
						roomName: 'room name',
						roomToken: response.RoomToken,
						isOpen: true,
						currentMessage: '',
						messages: [
							{ comment: response.Comment, isYou: false }
						]
					});
				} else {
					for (var i = 0; i < $scope.roomArray.length; i++) {

						if ($scope.roomArray[i].roomToken == response.RoomToken) {
							$scope.roomArray[i].messages.push(
								{ comment: response.Comment, isYou: false }
							);
						}
					}

					$(".item-content-body").animate({ scrollTop: $(".item-content-body").get(0).scrollHeight }, 'slow');
				}

				break;
			default: break;
		}
		$scope.$apply();
	}

	$scope.sendSocketMessage = function (index) {


		var cRoom = $scope.roomArray[index];

		var cMessage = cRoom.currentMessage;

		cRoom.currentMessage = '';

		$scope.roomArray[index].messages.push({
			comment: cMessage, isYou: true
		});

		//alert("c room token is >> " + cRoom.roomToken);

		socketService.sendMessage(JSON.stringify({
			roomToken: cRoom.roomToken,
			comment: cMessage,
			type: 2
		}));

		$(".item-content-body").animate({ scrollTop: $(".item-content-body").get(0).scrollHeight }, 'slow');
		console.log(cRoom);
	}

	socketService.startListening($scope.getSocketResponse);
});