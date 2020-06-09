/*
*FancyBox
*/
$(function () {
    $(".fancybox").fancybox();
});
//////////////////////////////////
/*
*Add ToCart
*/


//Change Event For Selected List
var dropDownList = document.querySelector(".QuntList");
var qtyDropList;
if (dropDownList !== null) {
    dropDownList.addEventListener("change", function (e) {
        debugger;
        e.preventDefault();
        qtyDropList = $(this).val();
        console.log(qtyDropList);
    });
}



////////////////////
//// Add Product To Cart function
///////////////////
//$("a.addToCard").click(function (e) {
//    e.preventDefault();
//    $("span.loader").addClass("ib");



//    //first check if quantity compatible with stock or not 
//    1
//    var qtyProduct = $('#qty-input').val();

//    var qtyProductInt = parseInt(qtyProduct);
//    //2 check if user user quantity
//    debugger;


//    //if (qtyProductInt === productQuantity) {
//    //    alert("Quantity Compatible")
//    //} else {
//    //    alert("Not't Quantity Compatible")

//    //}


//    var url = "/Cart/AddToCartPartial";

//    debugger;
//    $.get(url, { id: productId, Qyt: qtyProduct} ,function(data){

//        $(".ajaxCart").html(data);

//    }).done(function(){
//        $("span.loader").removeClass("ib");
//        $("span.ajaxMS").addClass("ib");

//        setTimeout(function(){
//            $("span.ajaxMS").fadeOut("fast");
//            $("span.ajaxMS").removeClass("ib");
//        },1500);
//        qtyDropList = null;

//        var url2 ="/Shop/GetProductQuantity";
//        debugger;
//        $.get(url2, { productId: productId }, function (data) {
//            debugger;
//            var checkValue = data.includes("</h4>");
//            if (checkValue) {
//                $(".product-btns").empty();
//                $(".product-btns").html(data);
//            }
//            else{
//                $(".QuntList").empty();
//                $(".QuntList").html(data);
//            }

//        }).done(function(){
//            console.log("done");
//        });
//    });

//});

////////////////////////////////////

//Invok 

//$(document).ready(function () {

//    mainScriptController.addToCartAnimation;
//    mainScriptController.wishlistCompare;
//    mainScriptController.quantity(productQuantity, productId);
//});