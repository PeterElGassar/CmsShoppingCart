//Module Design Pattern
var WishListService = function () {

    function AddToWishList(wishListBtn) {
        $.post("/api/WishList", { "": wishListBtn.attr('data-product-id') })
            .done(function () {

                $(wishListBtn).addClass('added1');
                changeOtherBtnsColor(wishListBtn.attr('data-product-id'));
                initialWishlistValues();
            })
            .fail(function () {
                alert('please login');
            });
    }

    function RemoveToWishList(wishListBtn) {

        $(wishListBtn).removeClass('added1');
        console.log(wishListBtn.attr('data-product-id'));
        $.ajax({
            url: "/api/WishList",
            data: { "": wishListBtn.attr('data-product-id') },
            method: 'DELETE',
            success: function () {
                $(wishListBtn).removeClass('added1');
                initialWishlistValues();
            },
            error: function () {
                alert('Something Wrong..');
            }

        });
    }

    function renderDropDownWishlist() {

        //call api to get user All whishlist Products
        $.getJSON("/api/WishList/GetWishListProducts", function (products) {

            if (products.wishListProducts != null) {
                $('.withlist-number')
                    .empty()
                    .text(products.wishListProducts.length)
                    .addClass("animated shake");
            }

            updateDomValues(products.wishListProducts);
        });
        //underScore js by popover
        //$(".js-wishlist").popover({
        //    placement: "bottom",
        //    html: true,
        //    title: "WishList",
        //    content: function () {
        //        var complied = _.template($("#wishlist-template").html());
        //        var html = complied({ products: productList });
        //        return html;
        //    }
        //});

    }

    //to change Color of wishlistBtn in all project
    var changeOtherBtnsColor = function (productId) {
        $('.wishlist').each(function () {
            if ($(this).attr("data-product-id") === productId) {
                $(this).addClass("added1");
            }
        });
    }


    //private func
    var updateDomValues = function (productList) {
        $(".wishlist-body").empty().append(function () {
            var complied = _.template($("#wishlist-template").html());
            var html = complied({ products: productList });
            return html;
        });
    };

    var initialWishlistValues = function () {
        renderDropDownWishlist();
    };

    return {
        AddToWishList: AddToWishList,
        RemoveToWishList: RemoveToWishList,
        renderDropDownWishlist: renderDropDownWishlist,
        initialWishlistValues: initialWishlistValues

    }
}()