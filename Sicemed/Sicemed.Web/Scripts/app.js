/// <reference path="jquery-1.6.2.js" />
/// <reference path="postAntiForgery.js" />

var app = (function ($, app) {
    app.initialize = function () {
        //Menu
        $(function () {
            /**
            * for each menu element, on mouseenter, 
            * we enlarge the image, and show both sdt_active span and 
            * sdt_wrap span. If the element has a sub menu (sdt_box),
            * then we slide it - if the element is the last one in the menu
            * we slide it to the left, otherwise to the right
            */
            $('#sdt_menu > li').bind('mouseenter', function () {
                var $elem = $(this);
                $elem.find('.sdt_wrap')
					     .stop(true)
						 .animate({ 'top': '30px' }, 500, 'easeOutBack')
						 .andSelf()
						 .find('.sdt_active')
					     .stop(true)
						 .animate({ 'height': '20px' }, 300, function () {
						     var $sub_menu = $elem.find('.sdt_box');
						     if ($sub_menu.length) {
						         $sub_menu.show('blind', { direction: 'vertical' }, 100);
						     }
						 });
            }).bind('mouseleave', function () {
                var $elem = $(this);
                var $sub_menu = $elem.find('.sdt_box');
                if ($sub_menu.length)
                    $sub_menu.animate({ 'left': '0px' }, 200, function () {
                        $sub_menu.hide('blind', { direction: 'vertical' }, 100);
                    });

                $elem.find('.sdt_active')
						 .stop(true)
						 .animate({ 'height': '0px' }, 300)
						 .andSelf()
						 .find('.sdt_wrap')
						 .stop(true)
						 .animate({ 'top': '25px' }, 500);
            });
        });
    };
    return app;
})(jQuery, app || {});

