if ($(window).width() >= 860) {
    $(function () {
        $('li.parent').on('mouseover', function () {
            var $menuItem = $(this),
                $submenuWrapper = $('> .wrapper', $menuItem);

            var menuItemPos = $menuItem.position();

            $submenuWrapper.css({
                top: menuItemPos.top,
                left: menuItemPos.left + Math.round($menuItem.outerWidth())
            });
        });
    });
}