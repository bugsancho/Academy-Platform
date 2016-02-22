//$('.tree-toggle').click(function () {
//    $(this).parent().children('ul.tree').toggle(200);
//});
$(function () {
    $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Сгънете това меню');
    $('.tree li.parent_li > span').on('click', function (e) {
        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(":visible")) {
            children.hide('fast');
            $(this).attr('title', 'Разпънете това меню').find(' > i').addClass('fa-plus').removeClass('fa-minus');
        } else {
            children.show('fast');
            $(this).attr('title', 'Сгънете това меню').find(' > i').addClass('fa-minus').removeClass('fa-plus');
        }
        e.stopPropagation();
    });
});