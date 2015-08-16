$(document).ready(function () {
    $(".courses").each(function () {
        var courseClass = ['course-blue', 'course-red', 'course-green', 'course-peach', 'course-darck-green', 'course-darck-blue', 'course-purple', 'course-orange'];
        var rand = courseClass[Math.floor(Math.random() * courseClass.length)];
        $(this).addClass(rand);
    });

});