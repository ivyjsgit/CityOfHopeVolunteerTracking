$(document).ready(function () {

    $("input").click(function () {
        console.log("Test");
        var body = $("body");
        body.overhang({
            type: "success",
            message: "Woohoo! Our message works!"
        });
    });
});