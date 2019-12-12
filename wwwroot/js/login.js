$(document).ready(function () {

    $("#clockin").click(function () {
        console.log("Test");
        var body = $("body");
        toastr.success('You have been clocked in')

    });

    $("#clockout").click(function () {
        console.log("Test");
        var body = $("body");
        toastr.success('You have been clocked out')

    });


});