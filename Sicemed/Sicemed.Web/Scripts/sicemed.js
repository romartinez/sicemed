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


function isoDateReviver(key, value) {
    if (typeof value === 'string') {
        var a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)(?:([\+-])(\d{2})\:(\d{2}))?Z?$/.exec(value);
        console.log(a);
        if (a) {
            var utcMilliseconds = Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]);
            return new Date(utcMilliseconds);
        }
    }
    return value;
}

jQuery.extend(jQuery, {
    parseJSON: function (data) {
        console.log(data);
        return JSON.parse(data, isoDateReviver);
    }
});