if ($(window).width() >= 940) {
    window.addEventListener('resize', removeMirror);
    $(function () {
        $('li.parent').on('mouseover', function () {
            var $menuItem = $(this),
                $submenuWrapper = $('> .wrapper', $menuItem);

            var menuItemPos = $menuItem.position();

            $submenuWrapper.css({
                top: menuItemPos.top,
                left: menuItemPos.left + Math.round($menuItem.outerWidth())
            });

            var $isOut = ($(window).width() - ($submenuWrapper.offset().left + $submenuWrapper.outerWidth()));
            if ($isOut < 0) {
                $submenuWrapper.addClass('mirror');
            }
        });
    });
}

function removeMirror() {
    var elem = document.getElementsByClassName("wrapper");
    for (var i = 0; i < elem.length; i++) {
        elem[i].classList.remove("mirror");
    }
}