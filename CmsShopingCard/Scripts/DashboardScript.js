
// toggel Class
$(".humburger").click(function () {

    $(".wrapper").toggleClass("collapsee");
});


//Preview Selected Image
/////////////////////////////////////////////////////////////////////
function readUrl(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("img#ImgPreview")
                .attr("src", e.target.result)
                .width(200)
                .height(200);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$("#ImageUpload").change(function () {
    readUrl(this);
});