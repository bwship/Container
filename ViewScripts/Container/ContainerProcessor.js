$(document).ready(function () {
    $(".input-text").keyup(function () {
        $(".content-results").fadeTo("slow", 0.00, function () { //fade
            $(this).slideUp("slow", function () { //slide up
                $(this).remove(); //then remove from the DOM
            });
        });
    });
});
