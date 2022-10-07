if ($(window).width() >= 860) {
	window.addEventListener("resize", removeMirror);
}

function giveMirror() {
	if ($(window).width() >= 860) {
		var elem = document.getElementsByClassName("parent");

		for (var i = 0; i < elem.length; i++) {
			var child = elem[i].getElementsByClassName("wrapper");
			var isOut = isOutOfViewport(child[0]);
			if (isOut.right) {
				child[0].classList.add("mirror");
			}
		}
    }
}

function removeMirror() {
	var elem = document.getElementsByClassName("wrapper");
	for (var i = 0; i < elem.length; i++) {
		elem[i].classList.remove("mirror");
    }
}

var isOutOfViewport = function (elem) {

	var bounding = elem.getBoundingClientRect();

	var out = {};
	out.right = bounding.right > (window.innerWidth || document.documentElement.clientWidth);

	return out;

};