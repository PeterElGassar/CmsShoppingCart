var CartController = function (addToCartService) {
    //=========================Private Section ===========================//
    //glopal variables
    var productIdH, productQuantityH;


    var quantityFunc = function () {

        debugger;

        addToCartService.quantity(productQuantityH, productIdH);
    }
    //=========================Glopal Section ===========================//

    /*--Add To Cart Animation
   ------------------------*/
    var addToCartAnimationFunc = function () {
        
        addToCartService.addToCartAnimation();
    }

    /*--Wishlist & Compare
   ------------------------*/
    var wishlistCompareFunc = function () {
        addToCartService.wishlistCompare();
    }


    /*--Add To Cart
  ------------------------*/
    var addToCartFunc = function () {
        addToCartService.addToCart();
    }

    var incrementItemFunc = function () {
        addToCartService.incrementItem();
    }

    return {
        quantityFunc: quantityFunc,
        addToCartAnimationFunc: addToCartAnimationFunc,
        wishlistCompareFunc: wishlistCompareFunc,
        addToCartFunc: addToCartFunc,
        incrementItemFunc: incrementItemFunc
    }


}(AddToCartService);