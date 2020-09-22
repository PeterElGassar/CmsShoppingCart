
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
    //علشان لو ضغط علي link btn
    newCatA.click(function (e) {
        e.preventDefault();
        //القيمة المكتوبة في textBox
        var catName = newCatTextInput.val();
        //لو القيمة اقل من حرفين
        if (catName.length < 2) {
            //bootbox.alert("Category Name Should Be at Least 2 Characters Long.");
            new duDialog('Warning!!', 'Category Name Should Be at Least 2 Characters Long.');

            return false;
        }

        ajaxText.show();

        var url = "/admin/shop/AddNewCategory";
        $.post(url, { catName: catName }, function (data) {
            //يخزن الاسم بعد التخلص من spaces
            var response = data.trim();

            if (response == "titletaken") {
                //هنا بتظهر رسالة ان الاسم مكرر فقط
                ajaxText.html("<span class='alert alert-danger'>this Title Is Taken Before</span>");
                // هنا علشان الرسالة تختفي بعدها ب 2000 ملي ثانية
                setTimeout(function () {
                    ajaxText.fadeOut("fast", function () {
                        ajaxText.html("<img src='/Content/Img/lg.harmony-taiji-spinner.gif'/>");
                    });
                }, 2000);
                return false;
            } else {
                if (!$("table#Pages").length) {
                    location.reload();
                }
                else {
                    ajaxText.html("<span class='alert alert-success'>the Category Has Been added</span>");
                    ///نفس الحكاية هتخفي رسالة نجاح العملة بعد ثانيتين
                    setTimeout(function () {
                        ajaxText.fadeOut("fast", function () {
                            ajaxText.html("<img src='/Content/Img/lg.harmony-taiji-spinner.gif'/>");
                        });
                    }, 2000);

                    newCatTextInput.val("");
                    var toAppend = $("table#Pages tbody tr:last").clone();
                    toAppend.attr("id", "id_" + data);
                    toAppend.find("#item_Name").val(catName);
                    toAppend.find("a.delete").attr("href", "/admin/shop/DeleteCategory/" + data);
                    table.append(toAppend);
                    //table.sortable("refresh");
                }
            }
        });

    });

    /////////////////////////////////////////////////////////////////////
    /*///////////////////////////////////////////////////////////////////
    * Jquery Sortable
    */
    //$("table#pages tbody").sortable({

    //    items: "tr:not(.home)",
    //    placeholder: "ui-state-highlight",
    //    update: function () {
    //        //Proplem Here
    //        var ids = $("table#pages tbody").sortable("serialize");
    //        var url = "/Admin/Shop/ReorderCategories/";

    //        $.post(url, ids, function (data) {
    //        });

    //    }
    //});
    //////////////////////////////////////////////
    //confirm Category Deletion
    ////
    $("body").on("click", "a.delete", function (e) {

        e.preventDefault();
        var button, url, temp;
        button = $(this);
        buttonId = parseInt(button.attr("data-catId"));
        url = "/Admin/Shop/DeleteCategory"

        // External  library called 'duDialog'
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

        //if (!confirm("Confirm Page Deletion")) return false;
    });


    //////////////////////
    /*
    Rename Categorys
    */
    var oreginalTextBoxValue;
    //first get All
    $("table#Pages input.text-box").dblclick(function () {
        oreginalTextBoxValue = $(this).value;
        //console.log(oreginalTextBoxValue);

        $(this).attr("readonly", false);
    });

    $("table#Pages input.text-box").keyup(function (e) {
        if (e.keyCode == 13) {
            console.log(e.keyCode);
            $(this).blur();
        }
    });
    // blur Event firing when loses focus
    $("table#Pages input.text-box").blur(function () {
        var $this = $(this);
        var ajaxDiv = $this.parent().find(".ajaxDivTd");
        var newCatName = $this.val();
        //Work theory of Substring Function
        //Get All Character after the Number ;
        //in This Case get ID_ Of <tr>
        var id = $this.parent().parent().attr("id").substring(3);
        var url = "/admin/shop/RenameCategory";
        if (newCatName.length < 2) {
            alert("Category Name must be at least more than 2 Charracter.");
            $this.attr("readonly", true);
            return false;
        }

        $.post(url, { newCatName: newCatName, id: id }, function (data) {
            var response = data.trim();

            if (response == "titletaken") {
                //Here I changed $this.val(); Replacement oreginalTextBoxValue because it not work
                $this.val($this.val());
                ajaxDiv.html("<span class='alert alert-danger'>this Title Is Taken Before</span>").show();
            } else {
                ajaxDiv.html("<span class='alert alert-success'>Name changed successfully</span>").show();
            }
            setTimeout(function () {
                ajaxDiv.fadeOut("fast", function () {
                    ajaxDiv.html("");
                })
            }, 2000);

        }).done(function () {
            $this.attr("readonly", true)
        });
    });

});
///==========================
//End //Main Page Of Categories
//==========================