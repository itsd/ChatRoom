﻿app.controller('roomController', function ($scope, $rootScope, $http, $sce, signalrService, chatService) {

	$scope.openRoom = chatService.openRoom;

	$scope.dialog = {
		isOpen: false,
		title: 'Title ...',
		closeDialog: function () {
			this.isOpen = false;
		},
		openDialog: function () {
			this.isOpen = true;
		}
	};

	$scope.currentMessage = '';

	signalrService.setScope($scope);

	$scope.hideChat = function () {
		return Object.keys($scope.openRoom).length === 0;
	}

	$scope.writingInRoom = function () {
		if ($scope.currentMessage != '') {
			message = $scope.currentMessage;
			$scope.currentMessage = '';
			$(".conversation-content-inner").animate({ scrollTop: $(".conversation-content-inner").get(0).scrollHeight }, 'slow');

			signalrService.sendMessageTo(message, chatService.openRoom,
				function () {
					$scope.$apply();
				});
		}
	}

	$scope.renderHtml = function (message) {
		return $sce.trustAsHtml(message.showSmiles());
	}
});