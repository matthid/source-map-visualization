import $ from 'jquery';
window.jQuery = $;
window.$ = $;

export default function() {
    $("body").delegate(".original-item, .generated-item, .mapping-item", "mouseenter", function() {
        $(".selected").removeClass("selected");
        var mappedItems = $(this).data('mapped');
        if (!mappedItems){
            var source = $(this).data("source");
            var line = $(this).data("line");
            var column = $(this).data("column");
            mappedItems = $(".item-" + source + "-" + line + "-" + column);
            $(this).data('mapped', mappedItems)
        }
        $(mappedItems).addClass("selected");

    }).delegate(".original-item, .generated-item, .mapping-item", "click", function() {
        var mappedItems = $(this).data('mapped');
        var elems = $(mappedItems).not(this).get();
        if (elems.length) {
            elems.forEach(function (elem) {
                if ('scrollIntoViewIfNeeded' in elem)
                    return elem.scrollIntoViewIfNeeded();
                elem.scrollIntoView({behavior: 'smooth'})
            })
        }
    });
}