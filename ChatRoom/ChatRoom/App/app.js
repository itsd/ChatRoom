
var app = angular.module('chatApp', ['ngResource', 'ngRoute', 'ngCookies']);

app.directive('enter', function () {
	return function (scope, element, attrs) {
		element.bind("keydown", function () {
			if (event.keyCode == 13) {
				$("#btnSender").click();
			}
		})
	};
});

function api(path) {
	return configuration.apiUrl + '/' + path;
}

Array.prototype.where = function (predicate) {
	var results = [];
	for (i = 0; i < this.length; i++) {
		if (predicate(this[i]))
			results.push(this[i]);
	}
	return results;
};

Array.prototype.remove = function (obj) {
	var index = this.indexOf(obj);
	var rest = this.splice(index, 1);
};

Array.prototype.removeWithID = function (predicate) {
	for (i = 0; i < this.length; i++) {
		if (predicate(this[i])) this.remove(this[i]);
	}
};