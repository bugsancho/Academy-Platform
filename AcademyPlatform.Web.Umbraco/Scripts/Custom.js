$(document).ready(function () {
    $('a[href^="#"]').on('click', function (e) {
        e.preventDefault();

        var target = this.hash;
        var $target = $(target);

        $('html, body').stop().animate({
            'scrollTop': $target.offset().top
        }, 900, 'swing', function () {
            window.location.hash = target;
        });
    });

    //Add custom class on Courses box
    $(".courses").each(function (index) {
        var courseClass = ['course-blue', 'course-red', 'course-green', 'course-darck-green', 'course-darck-blue', 'course-light-blue'];
        var rand = courseClass[Math.floor(Math.random() * courseClass.length)];
        $(this).addClass(rand);
    });
});
