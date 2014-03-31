app.directive('loading', function () {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: configuration.viewUrlPrefix + '/app/views/loading.html',
		link: function (scope, element, attrs) {
			scope.$watch('loading', function (val) {
				if (val) $(element).show();
				else $(element).hide();
			});
		}
	}
});

app.directive('opener', function () {
	return {
		restrict: 'E',
		replace: true,
		template: '<div><button id="btn">Click Me</button></div>',
		link: function (scope, element, attrs) {
			$(this).on('click', function () {
				scope.testXXX();
			});
		}
	}
});