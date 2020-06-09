var AddToCartService = function (notificationService) {

    /*--Add To Cart Animation
------------------------*/
    var addToCartAnimation = function () {

        $('.add-to-cart').on('click', function (e) {
            e.preventDefault();

            if ($(this).hasClass('added')) {
                $(this).removeClass('added').find('i').removeClass('ti-check').addClass('ti-shopping-cart').siblings('span').text('add to cart');
            } else {
                $(this).addClass('added').find('i').addClass('ti-check').removeClass('ti-shopping-cart').siblings('span').text('added');
            }
        });

    };

    /*--Wishlist & Compare
------------------------*/
    var wishlistCompare = function () {
        $('.wishlist-compare a').on('click', function (e) {
            e.preventDefault();

            if ($(this).hasClass('added')) {
                $(this).removeClass('added');
            } else {
                $(this).addClass('added');
            }
        });
    }


    /*-----Vaidation Quantity Private Function
------------------------*/
    var quantityIncDec = function (incOrDecbtn) {
        // plus or minus btns
        var $button = incOrDecbtn;
        debugger
        //input qunatity
        var $inputQty = $('#qty-input');
        var oldValue = parseInt($inputQty.val());
        var productQuantity = parseInt($inputQty.attr('data-max-value'));


        if ($button.hasClass('inc') && oldValue < productQuantity) {
            var newVal = parseFloat(oldValue) + 1;
            // add new val to input
            $inputQty.val(newVal);

        } else if ($button.hasClass('dec')) {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
            $inputQty.val(newVal);

        } else {
            $inputQty.val(oldValue);
        }

    };

    /*-----Vaidation Quantity
------------------------*/
    var quantity = function () {
        $('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
        $('.pro-qty').append('<span class="inc qtybtn">+</span>');

        //'.qtybtn' is a class for plus & munse btn togther
        $('.qtybtn').on('click', function () {
            quantityIncDec($(this));
        });


        // foucs event to save old value of quantity
        $('#qty-input').on('focusin', function () {
            $(this).attr("data-old-val", $(this).val());
        });

        //Also Check When change Quantity Input 
        // i want if input value changed to > max value
        //return input to original value and warning user 
        $('#qty-input').on("input change focusout", function () {
            var button = $(this);

            var inputNewVal = parseInt(button.val());

            if (inputNewVal <= 0 || isNaN(inputNewVal)) {
                inputNewVal = 0;
                button.val(0);
            }

            var maxValue = parseInt(button.attr('data-max-value'));
            var prevValue = parseInt(button.attr('data-old-val'));

            if (inputNewVal > maxValue) {

                button.val(prevValue);

                $('.ajax-message span').text('the number of Quantity greater than in stock');
                $('.ajax-message').fadeIn();

                setTimeout(function () {
                    $('.ajax-message span').text('');
                    $('.ajax-message').fadeOut("fast");
                }, 2500);

            }


        });

    }

    /*-----Add To Cart
------------------------*/
    var addToCart = function () {

        $("a.addToCard").click(function (e) {
            e.preventDefault();
            $("div.loader").addClass("ib");


            var button, productId, qtyProduct, url;
            button = $(this);
            productId = button.attr('data-product-id');
            productName = button.attr('data-product-name');
            //Optinal
            qtyProduct = $('#qty-input').val();
            url = "/Cart/AddToCartPartial";


            $.get(url, { id: productId, Qyt: qtyProduct }, function (data) {

                $(".ajaxCart").html(data);

            }).done(function () {
                $("div.loader").removeClass("ib");
                $("span.ajaxMS").addClass("ib");

                setTimeout(function () {
                    $("span.ajaxMS").fadeOut("fast");
                    $("span.ajaxMS").removeClass("ib");
                }, 1500);

            });
            closeModalPopup();

            setTimeout(function () {
                notificationService.successNotify(productName + " Added to Cart Successfully.");
            }, 1000);
        });
    }

    var incrementItem = function () {
        /////////////////////////////////////////////////////////////////////
        // Increment Products In Cart
        /////////////////////////////////////////////////////////////////////
        $("a.IncrProduct").click(function (e) {
            e.preventDefault();
            var productId = $(this).data("id");
            var url = "/cart/IncrementProduct";
            $.getJSON(url, { productId: productId }, function (data) {

                //Here Price And qty Come From (data) Paramenter Of Action  Method

                $("input.qty" + productId).val(data.qty);
                var price = data.qty * data.price;
                var massage = data.ms;
                var priceHtml = "$" + price.toFixed(2);

                if (massage !== "") {
                    NotificationService.errorNotify("Sorry We Dont Have More Of This Item Now.");

                } else {
                    //=====================Increment Grand Total For All Product In Cart
                    $("td.total" + productId).html(priceHtml);
                    var gt = parseFloat($("td.grandtotal span").text());
                    var grandtotal = (gt + data.price).toFixed(2);
                    $("td.grandtotal span").text(grandtotal);
                }

                //Done Call Back Function Its work to get a new quantity into Paypal form
            }).done(function (e) {
                var url2 = "/cart/PaypalPartial";
                $.get(url2, {}, function (data) {
                    $("div.paypalDiv").html(data);
                });
            });
        });
    }


    /*-----Close Modal
------------------------*/
    var closeModalPopup = function () {
        if ($('#MyModel') != null || $('#MyModel') != undefined) {
            $("#MyModel").modal("hide");
        }
    }


    //$("span.incrProduct").on("clcik", function (e) {
    //    e.preventDefault();
    //    var productId, url, inputQty, price, priceHtml, responseMs, grandTotal;


    //    inputQty = $('#qty-input');

    //    if (inputQty.val < inputQty.attr("data-max-value")) {

    //        url = "/Cart/IncrementProduct";
    //        productId = $(this).data('id');

    //        //Call Server
    //        $.getJSON(url, { productId: productId }, function (response) {
    //            //Change to new Values
    //            //1-input
    //            inputQty.val(response.qty);
    //            //2-Calc single product Total
    //            price = response.qty * response.price;
    //            //3-convert a number into a string
    //            priceHtml = price.toFixed(2);

    //            responseMs = response.ms;
    //            if (massage !== "") {
    //                toastr.success(responseMs)
    //            }
    //            else {

    //                //initial total td
    //                $('.total' + productId).html(priceHtml);

    //                var oldGrandTotal = parseFloat($('td.grandtotal span').text());
    //                grandTotal = (oldGrandTotal + response.price).toFixed(2);

    //                //put new grand total
    //                $('td.grandtotal span').text(grandTotal);
    //            }
    //            //Done Call Back Function Its work to get a new quantity into Paypal form

    //        }).done(function (e) {
    //            var url2 = "/cart/PaypalPartial";
    //            $.get(url2, {}, function (data) {
    //                $("div.paypalDiv").html(data);
    //            });
    //        });


    //    } else {
    //        toastr.success("the quantity your Need Greater Than in Stock");
    //    }

    //});



    return {
        addToCartAnimation: addToCartAnimation,
        wishlistCompare: wishlistCompare,
        quantity: quantity,
        addToCart: addToCart,
        incrementItem: incrementItem
    }

}(NotificationService);