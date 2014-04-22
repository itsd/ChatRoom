app.controller('roomController', function ($scope, $rootScope, $http, $sce, signalrService, chatService) {

	$scope.openRoom = chatService.openRoom;

	$scope.currentMessage = '';

	$scope.hideChat = function () {
		return Object.keys($scope.openRoom).length === 0;
	}

	$scope.writingInRoom = function () {
		if ($scope.currentMessage != '') {
			chatService.openRoom.messages.push({ isUser: false, message: $scope.currentMessage });
			$scope.currentMessage = '';
			$(".conversation-content-inner").animate({ scrollTop: $(".conversation-content-inner").get(0).scrollHeight }, 'slow');


		}
	}

	$scope.renderHtml = function (message) {
		return $sce.trustAsHtml(message.showSmiles());
	}
});