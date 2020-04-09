/*
*FancyBox
*/
$(function () {
    $(".fancybox").fancybox();
});
////////////////////////////////////
/*
*Add ToCart
*/

//Change Event For Selected List
var dropDownList = document.querySelector(".QuntList");
var qtyDropList;
if (dropDownList !== null) {
    dropDownList.addEventListener("change",function(e){
        debugger;
        e.preventDefault();
        qtyDropList = $(this).val();
        console.log(qtyDropList);
    });
}


debugger;

//var addToCartBtn = document.querySelector(".addToCard");

//addToCartBtn.addEventListener("click", function () {
//    console.log("Btn Works Fine");
//});


$("a.addToCard").click(function (e) {
    e.preventDefault();
    debugger;
    $("span.loader").addClass("ib");
    var url = "/Cart/AddToCartPartial";
    debugger;
    $.get(url, { id:productId , Qyt:qtyDropList} ,function(data){

        $(".ajaxCart").html(data);
    }).done(function(){
        $("span.loader").removeClass("ib");
        $("span.ajaxMS").addClass("ib");

        setTimeout(function(){
            $("span.ajaxMS").fadeOut("fast");
            $("span.ajaxMS").removeClass("ib");
        },1500);
        qtyDropList = null;

        var url2 ="/Shop/GetProductQuantity";
        debugger;
        $.get(url2, { productId: productId }, function (data) {
            debugger;
            var checkValue = data.includes("</h4>");
            if (checkValue) {
                $(".product-btns").empty();
                $(".product-btns").html(data);
            }
            else{
                $(".QuntList").empty();
                $(".QuntList").html(data);
            }

        }).done(function(){
            console.log("done");
        });
    });

});

////////////////////////////////////