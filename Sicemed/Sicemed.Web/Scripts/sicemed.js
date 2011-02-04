jQuery.noConflict();

jQuery(document).ready(function () {
    // cufon font replacement
    font_improvement('h1, #featured:not(.curtain, .accordion, .newsslider) .sliderheading');
});

function font_improvement($selectors) {

    jQuery($selectors).each(function () {
        $size = parseInt(jQuery(this).css('fontSize'));
        jQuery(this).css('fontSize', $size * 1.4)
    });

    Cufon.replace($selectors, { fontFamily: 'geosans' });
}

