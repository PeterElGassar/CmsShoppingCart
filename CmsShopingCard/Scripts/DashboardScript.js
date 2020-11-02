
//Preview Selected Image
function readUrl(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("img#ImgPreview")
                .attr("src", e.target.result)
                .width(170)
                .height(160);
        }
        reader.readAsDataURL(input.files[0]);
        $("span#ImageUpload").css("display", "none");

    }
}

$(document).on("change", "#ImageUpload", function () {
    readUrl(this);
});


//for set Active Linke Style
var setDefaultActiveLink = function () {
    let path = window.location.pathname;

    var element = $(".navbar-nav a[href='" + path + "']");


    if (element.length > 0) {
        localStorage.setItem('anchor', JSON.stringify(element));
        element.removeClass("sidebar-link");
        element.parent("li").addClass("active-link");
    } else {
        let anchorInLocal = JSON.parse(localStorage.getItem('anchor'));
        console.log(anchorInLocal.selector);

        $(anchorInLocal.selector).removeClass("sidebar-link");
        $(anchorInLocal.selector).parent("li").addClass("active-link");
    }

}();


var initActiveLink = function () {

    $(document).on("click", "a.exit-btn", function () {
        debugger;
        $(".navbar-nav .nav-item").each(function () {
            $(this).removeClass("active-link");
            //clear Local Storage
            localStorage.removeItem('anchor');
            window.location.replace('http://localhost:19498/');

        });

    })
}()

///==========================
//Main Page Of Categories
//==========================
$(function () {

    //Add New Category
    ////
    var newCatA = $("a#newCatA");
    var newCatTextInput = $("#newCatName");
    var ajaxText = $("span.ajax-text");
    var table = $("table#Pages tbody");


    //علشان لو ضغط enter
    newCatTextInput.keyup(function (e) {
        if (e.keyCode == 13) {
            newCatA.click();
        }
    });
    newCatA.click(function (e) {
        e.preventDefault();
        var catName = newCatTextInput.val();
        if (catName.length < 2 || catName.trim() === "") {
            new duDialog('Warning!!', 'Category Name Should Be at Least 2 Characters Long.');
            return false;
        }

        ajaxText.show();

        var url = "/admin/shop/AddNewCategory";
        $.post(url, { catName: catName }, function (data) {
            //يخزن الاسم بعد التخلص من spaces
            var response = data.trim();

            if (response == "titletaken") {
                ajaxText.empty().text("Category Name Taken Before..").addClass("ib alert-danger");

                setTimeout(function () {
                    ajaxText.removeClass("ib alert-danger");
                }, 2500);
                return false;

            } else {

                if (!$("table#Pages").length) {
                    location.reload();
                }
                else {
                    ajaxText.empty().text("the Category has been Added").addClass("ib alert-success");
                    ///نفس الحكاية هتخفي رسالة نجاح العملة بعد ثانيتين
                    setTimeout(function () {
                        ajaxText.removeClass("ib alert-success");
                    }, 2500);

                    newCatTextInput.val("");
                    var toAppend = $("table#Pages tbody tr:last").clone();
                    toAppend.attr("id", "id_" + data);
                    toAppend.attr("id", "id_" + data);
                    toAppend.find("#item_Name").val(catName);
                    toAppend.find("a.delete").attr("href", "/admin/shop/DeleteCategory/" + data);
                    toAppend.find("a.delete").attr("data-catid", data);


                    table.append(toAppend);
                    //table.sortable("refresh");
                };
            }
        });

    });


    //////////////////////////////////////////////
    //confirm Category Deletion
    ////
    $("body").on("click", "a.delete", function (e) {

        var button, url, temp;
        button = $(this);
        buttonId = parseInt(button.attr("data-catId"));
        url = "/Admin/Shop/DeleteCategory"
        // External  library called 'duDialog duDialog duDialog duDialog'
        new duDialog('', 'Are You Sure you want Remove this Category', duDialog.OK_CANCEL, {
            okText: 'Remove from Cart',
            callbacks: {
                okClick: function () {
                    $.get(url, { id: buttonId }, function () {
                        // get a Specific Item <tr> to remove it
                        var trElement = $("tr#id_" + buttonId);
                        trElement.remove();
                    });

                    this.hide();
                },
                cancelClick: function () {
                    this.hide();
                }
            }
        });
        e.preventDefault();
        //if (!confirm("Confirm Page Deletion")) return false;
    });


    //////////////////////
    /*
    Rename Categorys
    */
    let oreginalTextBoxValue;
    //first get All
    $(document).on("dblclick", ".text-box", function (e) {

        oreginalTextBoxValue = $(this).val();

        $(this).attr("readonly", false);
    });

    $(document).on("keyup", ".text-box", function (e) {
        if (e.keyCode == 13) {
            $(this).blur();
        }
    });

    // Event firing when loses focus
    $(document).on("blur", ".text-box", function () {
        var $this = $(this);
        var ajaxDiv = $this.parent().next("td").children(".ajaxDivTd");
        var newCatName = $this.val();
        //Work theory of Substring Function
        //Get All Character after the Number
        //in This Case get ID_ Of <tr>
        var id = $this.parent().parent().attr("id").substring(3);
        var url = "/admin/shop/RenameCategory";

        if (newCatName.length < 2) {
            /// use custome alert here
            new duDialog('Warning!!', 'Category Name Should Be at Least 2 Characters Long.');
            $this.attr("readonly", true);
            $this.val(oreginalTextBoxValue);

            return false;
        }
        debugger;

        if ($this.val().toLowerCase() !== oreginalTextBoxValue.toLowerCase()) {


            $.post(url, { newCatName: newCatName, id: id }, function (data) {
                var response = data.trim();

                if (response == "titletaken") {
                    debugger;
                    //Here I changed $this.val(); Replacement oreginalTextBoxValue because it not work
                    console.log(oreginalTextBoxValue);
                    $this.val(oreginalTextBoxValue);
                    ajaxDiv.children("span").text("this Name Taken Before").addClass("alert-danger");

                    ajaxDiv.addClass("ib");

                } else {

                    ajaxDiv.children("span").text("Name changed Success").addClass("alert-success");
                    ajaxDiv.addClass("ib");

                }
                setTimeout(function () {

                    ajaxDiv.children("span")
                        .text("")
                        .removeClass("alert-success alert-danger");

                    ajaxDiv.removeClass("ib");



                }, 2000);

            }).done(function () {
                $this.attr("readonly", true)
            });
        }
        $this.attr("readonly", true)

    });


    //////////////////////
    /*
    Client Details
    */
    $(".user-details-btn").on("click", function (e) {
        userId = $(this).attr("data-user-id");
        let url = "/admin/shop/OrderUserDetils?id=" + userId;
        $(".client-modal-body").load(url, function () {
            $("#client-details").modal("show");

        });
        e.preventDefault();
    });



    //==================(Create btn)Display Dialog From ========
    $(".js-create-btn").on("click", function () {
        let url = "/admin/brand/CreateByPopup";
        $(".client-modal-body").load(url, function () {
            $("#client-details").modal("show");

        });
    });
    //==================Submit Create Form========
    $(document).on("click", "#submit-form-btn", function () {
        debugger;
        var fm = $('#createForm');
        var formdata = new FormData();
        formdata.append("Name", $("#Name").val());
        formdata.append("BrandImage", $("#ImageUpload").get(0).files[0]);

        $.validator.unobtrusive.parse(fm);

        if ($(fm).valid()) {
            $.ajax({
                type: "POST",
                url: "/admin/Brand/CreateByPopup/",
                data: formdata,
                processData: false,
                contentType: false,
                dataType: "text",
                success: function (result) {
                    console.log(result);
                    debugger;
                    if (result === "false") {
                        console.log(result);

                    } else {
                        if (typeof result === 'string') {
                            debugger;
                            alert("success");
                            $("#client-details").modal("hide");
                        }
                    }
                },
                error: function (result) {

                    alert(result);
                }
            });
        }


    });


    ///============Edit Brands Of Category==============
    $(document).on("click", "a.js-edit-cat-btn", function (e) {
        let button = $(this);
        let url = "/admin/brand/EditCategoryPartial?id=" + button.attr("data-cat-id");
        $(".edit-modal-body").load(url, function () {
            $("#edit-modal").modal("show");
        });
        e.preventDefault();
    });

});


///============Edit Brand PopUp==============
$(document).on("click", "a.edit-brand-btn", function (e) {
    debugger;
    let brandId = $(this).attr("data-brand-id");
    let url = "/admin/brand/EditBrandPartial?id=" + brandId;
    $(".client-modal-body").load(url, function () {
        $("#client-details").modal("show");
    });
    e.preventDefault();
});


