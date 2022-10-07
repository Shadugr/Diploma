function toggleMenu() {
    var elem = document.getElementById("menu");
    elem.classList.toggle("toggle");
}

function toggleNews() {
    var elem = document.getElementById("news");
    elem.classList.toggle("toggle");
}

if ($(window).width() < 940) {
    $(document).click(function (event) {
        if ($(event.target).hasClass("parent") || $(event.target).hasClass("menuItem")) {
            $(event.target).find('img').first().toggleClass('clicked');
            $(event.target).find('div').first().toggleClass('display');
        }
    });
}