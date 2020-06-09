///*----- 
//	Quantity
//--------------------------------*/
//$('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
//$('.pro-qty').append('<span class="inc qtybtn">+</span>');
//$('.qtybtn').on('click', function () {
//    var $button = $(this);
//    var oldValue = $button.parent().find('input').val();
//    if ($button.hasClass('inc')) {
//        var newVal = parseFloat(oldValue) + 1;
//    } else {
//        // Don't allow decrementing below zero
//        if (oldValue > 0) {
//            var newVal = parseFloat(oldValue) - 1;
//        } else {
//            newVal = 0;
//        }
//    }
//    $button.parent().find('input').val(newVal);
//});

///*----- 
//	Shipping Form Toggle
//--------------------------------*/
//$('[data-shipping]').on('click', function () {
//    if ($('[data-shipping]:checked').length > 0) {
//        $('#shipping-form').slideDown();
//    } else {
//        $('#shipping-form').slideUp();
//    }
//})