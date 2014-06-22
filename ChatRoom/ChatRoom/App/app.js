
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
	return configuration.api.Url + '/' + path;
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

Array.prototype.removeWithProperty = function (predicate) {
	for (i = 0; i < this.length; i++) {
		if (predicate(this[i])) this.remove(this[i]);
	}
};

String.prototype.showSmiles = function () {
	return this.replace(/:P/g, '<img width="20" src="Content/images/smiles/Smiley-stick-tongue-icon.png" />');

	//var msgssss1 = this.replace(/:D/g, '<img src="http://img4.wikia.nocookie.net/__cb20061223084010/lostpedia/images/e/e4/Big_smile.gif" />');
	////var msgssss2 = msgssss1.replace(/:P/g, '<img src="http://www.oodmag.com/community/images/smilies/1134_tongue.gif" />');
	////var msgssss3 = msgssss2.replace(';)', '<img src="https://uglounge.com/images/smilies/Wink.gif" />');
	////var msgssss4 = msgssss3.replace(':)', '<img src="https://www.ihg.com/hotels/images/chatImages/images/smiles/smile_smily.png" />');

	//return msgssss1;
}