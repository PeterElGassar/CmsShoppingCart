var WishListController = function (wishListService) {


    var initial = function () {

        if ($(".wishlist") != null && $(".wishlist") != undefined) {


            $(".wishlist").on("click", toggleWishListBtn);
            wishListService.initialWishlistValues();
        }
    }


    //(privet method)==>make decision of choose which Api function will calling
    var toggleWishListBtn = function (e) {

        let wishListBtn = $(e.target).parent();
        //unsure if product Already added before
        if (wishListBtn.hasClass("added1")) {

            wishListService.RemoveToWishList(wishListBtn);

        } else {

            wishListService.AddToWishList(wishListBtn);

        }
    }

    var renderWishlist = function () {
        $(".js-wishlist").on('click', function (e) {
            wishListService.renderDropDownWishlist();
        })

        $(".js-wishlist").on("click", function (e) {
            wishListService.renderDropDownWishlist();

            $(".dropdown-wishlist").slideToggle('fast');
            e.preventDefault();
        });
    }


    return {
        initial: initial,
        renderWishlist: renderWishlist
    }
}(WishListService)