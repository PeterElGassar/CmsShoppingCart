var searchInput = document.querySelector("#search-note input");

if (searchInput) {
    searchInput.addEventListener("keyup", function (e) {
        //الكلمة اللي جوة البوكس
        var searchChar = e.target.value.toUpperCase();

        var titels = document.getElementsByTagName("tr");
        //here this code get first of all the first 'TR' mean the titels header,, then get second 'TH' chiled in "TR"
        //var getSecondTdName = titels[0].getElementsByTagName('th')[1];
        //================
        //var Exption = titels.firstElementChild;
        //console.log(Exption.firstElementChild.textContent.toUpperCase());
        Array.from(titels).forEach(function (titel) {
            //var getSecondTdName = titel.getElementsByTagName('th')[1].textContent.replace(/\s/g, "");

            var parTitel = titel.firstElementChild.textContent.replace(/\s/g, "");

            if (parTitel !== "OrderNumber") {
                if (parTitel.toUpperCase().indexOf(searchChar) !== -1) {
                    titel.style.display = "block";
                    console.log(parTitel.toUpperCase().indexOf(searchChar));
                }
                else {
                    titel.style.display = "none";
                }
            }

        });

    });
}



//Invok Glopal Method In All Pages
CartController.quantityFunc();
CartController.wishlistCompareFunc();
CartController.addToCartAnimationFunc();
CartController.addToCartFunc();
CartController.incrementItemFunc();


var quickViewFunction = function (productId) {
    var url = "/Shop/QuickViewProduct?Id=" + productId;

    $("#MyModel_BodyDiv").load(url, function () {
        $("#MyModel").modal("show");
        CartController.quantityFunc();
        CartController.wishlistCompareFunc();
        CartController.addToCartAnimationFunc();
        CartController.addToCartFunc();
    });

}


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

//Slick Slider 
/////////////////////////////////////////////////////////////////////
var listSlickDiv = document.getElementsByClassName("post-wrapper");
var spacificClases = [];

for (var i = 0; i < listSlickDiv.length; i++) {
    debugger;
    var singlClassName = "." + listSlickDiv[i].classList[1];
    var classIdNum = singlClassName.split("_")[1];
    var next = ".next_" + classIdNum;
    var prev = ".prev_" + classIdNum;

    console.log(next);
    console.log(prev);

    //List Clases Names Of Sliders
    spacificClases.push("." + listSlickDiv[i].classList[1]);

    ///Slick Slider
    $(singlClassName).slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        nextArrow: $(next),
        prevArrow: $(prev),
        //asNavFor: '.post-wrapper'

    });
    //console.log(spacificClases[i].classList[1]);

}

$(document).ready(function () {

    $(function () {

        if ($("#search-input") != null && $("#search-input") != undefined) {


            $("#search-input").autocomplete({

                minLength: 1,
                source: function (request, response) {

                    $.ajax({
                        url: "/Shop/AutoComplete",
                        type: "GET",
                        dataType: "json",
                        data: { prefix: request.term },
                        success: function (data) {
                            //var ojec = JSON.stringify(data);
                            //response($.map(data, function (item) {

                            //    return { lable: item, value: item };

                            //}))
                            response(data);
                        }
                    })
                },

                focus: function (event, ul) {
                    $(this).val(ul.item.ProductName);
                    return false;
                },
                select: function (event, ul) {
                    $(this).val(ul.item.ProductName);
                    window.location.href = "/Shop/product-details/" + ul.item.ProductSlug;
                    return false;
                },
                messages: {
                    noResults: "", results: function (resultsCount) { }
                }
            })
                .autocomplete('instance')._renderItem = function (ul, item) {
                    return $('<li>')
                        .append('  <img class="rounded imgAutoC" src="/Images/Uploads/Products/' + item.ProductId + '/Thumbs/' + item.ProductImg + ' " />')
                        .append('<p class="autoComplete-titel">' + item.ProductName + '</p>')
                        .appendTo(ul);
                }

        }
    });

});


console.log(spacificClases);


$('.test-slick').slick({
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
            bootbox.alert("Category Name Should Be at Least 2 Characters Long.");
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
                    table.sortable("refresh");
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

        bootbox.confirm("Are You Sure You Want to Delete This Category ?", function (result) {
            //if true call Ajax
            if (result) {
                debugger;
                $.get(url, { id: buttonId }, function () {
                    // get a Specific Item <tr> to remove it
                    var trElement = $("tr#id_" + buttonId);
                    trElement.remove();
                });
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
        //Work Thuory of Substring Function
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


//Tamplate Scripts 
(function ($) {
    "use strict";

    /*--
        Menu Sticky
    -----------------------------------*/
    var windows = $(window);
    var screenSize = windows.width();
    var sticky = $('.header-sticky');

    windows.on('scroll', function () {
        var scroll = windows.scrollTop();
        if (scroll < 300) {
            sticky.removeClass('is-sticky');
        } else {
            sticky.addClass('is-sticky');
        }
    });

    /*--
        Mobile Menu
    ------------------------*/
    var mainMenuNav = $('.main-menu nav');
    mainMenuNav.meanmenu({
        meanScreenWidth: '991',
        meanMenuContainer: '.mobile-menu',
        meanMenuClose: '<span class="menu-close"></span>',
        meanMenuOpen: '<span class="menu-bar"></span>',
        meanRevealPosition: 'right',
        meanMenuCloseSize: '0',
    });

    /*--
        Category Menu
    ------------------------*/

    /*-- Variables --*/
    var categoryToggleWrap = $('.category-toggle-wrap');
    var categoryToggle = $('.category-toggle');
    var categoryMenu = $('.category-menu');

    /*
    *  Category Menu Default Close for Mobile & Tablet Device
    *  And Open for Desktop Device and Above
    */
    function categoryMenuToggle() {
        var screenSize = windows.width();
        if (screenSize <= 991) {
            categoryMenu.slideUp();
        } else {
            categoryMenu.slideDown();
        }
    }

    /*-- Category Menu Toggles --*/
    function categorySubMenuToggle() {
        var screenSize = windows.width();
        if (screenSize <= 991) {
            $('.category-menu .menu-item-has-children > a').prepend('<i class="expand menu-expand"></i>');
            $('.category-menu .menu-item-has-children ul').slideUp();
            //        $('.category-menu .menu-item-has-children i').on('click', function(e){
            //            e.preventDefault();
            //            $(this).toggleClass('expand');
            //            $(this).siblings('ul').css('transition', 'none').slideToggle();
            //        })
        } else {
            $('.category-menu .menu-item-has-children > a i').remove();
            $('.category-menu .menu-item-has-children ul').slideDown();
        }
    }
    categoryMenuToggle();
    windows.resize(categoryMenuToggle);
    categorySubMenuToggle();
    windows.resize(categorySubMenuToggle);

    categoryToggle.on('click', function () {
        categoryMenu.slideToggle();
    });

    /*-- Category Sub Menu --*/
    $('.category-menu').on('click', 'li a, li a .menu-expand', function (e) {
        var $a = $(this).hasClass('menu-expand') ? $(this).parent() : $(this);
        if ($a.parent().hasClass('menu-item-has-children')) {
            if ($a.attr('href') === '#' || $(this).hasClass('menu-expand')) {
                if ($a.siblings('ul:visible').length > 0) $a.siblings('ul').slideUp();
                else {
                    $(this).parents('li').siblings('li').find('ul:visible').slideUp();
                    $a.siblings('ul').slideDown();
                }
            }
        }
        if ($(this).hasClass('menu-expand') || $a.attr('href') === '#') {
            e.preventDefault();
            return false;
        }
    });

    /*-- Sidebar Category --*/
    var categoryChildren = $('.sidebar-category li .children');

    categoryChildren.slideUp();
    categoryChildren.parents('li').addClass('has-children');

    $('.sidebar-category').on('click', 'li.has-children > a', function (e) {

        if ($(this).parent().hasClass('has-children')) {
            if ($(this).siblings('ul:visible').length > 0) $(this).siblings('ul').slideUp();
            else {
                $(this).parents('li').siblings('li').find('ul:visible').slideUp();
                $(this).siblings('ul').slideDown();
            }
        }
        if ($(this).attr('href') === '#') {
            e.preventDefault();
            return false;
        }
    });

    /*--
        Header Search
    ------------------------*/
    var searchToggle = $('.search-toggle');
    var searchContainer = $('.header-search-container');

    searchToggle.on('click', function () {

        if (!$(this).hasClass('active')) {
            $(this).addClass('active').find('i').removeClass('icofont-search-alt-1').addClass('icofont-close');
            searchContainer.slideDown();
        } else {
            $(this).removeClass('active').find('i').removeClass('icofont-close').addClass('icofont-search-alt-1');
            searchContainer.slideUp();
        }

    });
    /*--Header Cart
    //------------------Popup Side Card------*/
    //var headerCart = $('.header-cart');
    //var closeCart = $('.close-cart, .cart-overlay');
    //var miniCartWrap = $('.mini-cart-wrap');

    //headerCart.on('click', function (e) {
    //    e.preventDefault();
    //    $('.cart-overlay').addClass('visible');
    //    miniCartWrap.addClass('open');
    //});
    //closeCart.on('click', function (e) {
    //    e.preventDefault();
    //    $('.cart-overlay').removeClass('visible');
    //    miniCartWrap.removeClass('open');
    //});

    /*--
        Hero Slider
    --------------------------------------------*/
    var heroSlider = $('.hero-slider');
    heroSlider.slick({
        arrows: true,
        autoplay: false,
        autoplaySpeed: 5000,
        dots: true,
        pauseOnFocus: false,
        pauseOnHover: false,
        fade: true,
        infinite: true,
        slidesToShow: 1,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
    });

    /*--
        Product Slider
    -----------------------------------*/
    $('.product-slider-4').slick({
        arrows: true,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 4,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    autoplay: true,
                    slidesToShow: 1,
                    arrows: false,
                }
            }
        ]
    });

    $('.product-slider-4-full').slick({
        arrows: true,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 4,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
        responsive: [
            {
                breakpoint: 1499,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    autoplay: true,
                    slidesToShow: 1,
                    arrows: false,
                }
            }
        ]
    });

    $('.product-slider-3').slick({
        arrows: true,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 3,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    autoplay: true,
                    slidesToShow: 1,
                    arrows: false,
                }
            }
        ]
    });

    /*-- Single Product Big Image Slider --*/
    $('.big-image-slider').slick({
        arrows: false,
        dots: true,
        slidesToShow: 1,
    });

    /*----- 
        Team Slider
    --------------------------------*/
    $('.team-slider-5').slick({
        arrows: false,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 5,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-long-arrow-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-long-arrow-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 4,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    autoplay: true,
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 479,
                settings: {
                    autoplay: true,
                    slidesToShow: 1,
                }
            }
        ]
    });

    /*----- 
        Testimonial Slider
    --------------------------------*/
    $('.testimonial-slider').slick({
        arrows: true,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 1,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    $('.testimonial-slider-2').slick({
        arrows: false,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 2,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 1,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    autoplay: true,
                    slidesToShow: 1,
                }
            }
        ]
    });

    /*-- Image Slider For Sync with Content Slider --*/
    $('.testimonial-image-slider').slick({
        arrows: false,
        dots: false,
        autoplay: false,
        infinite: true,
        asNavFor: '.testimonial-content-slider',
        slidesToShow: 3,
        centerMode: true,
        centerPadding: '0',
        focusOnSelect: true,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*-- Content Slider For Sync with Image Slider --*/
    $('.testimonial-content-slider').slick({
        arrows: false,
        dots: false,
        autoplay: false,
        infinite: true,
        slidesToShow: 1,
        asNavFor: '.testimonial-image-slider',
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*--
        Brand Slider
    -----------------------------------*/
    $('.brand-slider').slick({
        arrows: false,
        dots: false,
        autoplay: false,
        infinite: false,
        slidesToShow: 5,
        prevArrow: '<button type="button" class="slick-prev"><i class="icofont icofont-rounded-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="icofont icofont-rounded-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 5,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 4,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 2,
                }
            }
        ]
    });

    /*--
        Product View Mode
    ------------------------*/
    $('.product-view-mode a').on('click', function (e) {
        e.preventDefault();

        var shopProductWrap = $('.shop-product-wrap');
        var viewMode = $(this).data('target');

        $('.product-view-mode a').removeClass('active');
        $(this).addClass('active');
        shopProductWrap.removeClass('grid list').addClass(viewMode);
    })

    /*--
        Product Tab Filter Select Style For Mobile
    --------------------------------------------*/
    function productTabFilterInit() {
        var productTabFilter = $('.product-tab-filter');

        productTabFilter.each(function () {
            var filterToggle = $(this).find('.product-tab-filter-toggle');
            var filterToggleCatElement = $(this).find('.product-tab-filter-toggle span');
            var filterList = $(this).find('.product-tab-list');
            var filterListItem = $(this).find('.product-tab-list li a');

            var filterCatText = filterList.find('.active').text();

            filterToggleCatElement.text(filterCatText);

            /*-- Open Filter Tab List --*/
            filterToggle.on('click', function () {
                $(this).siblings('.product-tab-list').slideToggle();
            });

            /*-- Close Filter Tab List On Select a Category --*/
            filterListItem.on('click', function () {
                var screenSize = windows.width();
                var filterCatText = $(this).text();
                filterToggleCatElement.text(filterCatText);

                if (screenSize < 767) {
                    filterList.slideToggle();
                }

            });

        });

    }
    productTabFilterInit();

    /*-- Product Tab Filter Show Hide For Mobile & Desktop --*/
    function productTabFilterScreen() {
        var screenSize = windows.width();
        var filterList = $('.product-tab-list');

        if (screenSize < 767) {
            filterList.slideUp();
        } else {
            filterList.slideDown();
        }
    }
    productTabFilterScreen();
    windows.resize(productTabFilterScreen);

    ///*--
    //    Add To Cart Animation
    //------------------------*/
    //$('.add-to-cart').on('click', function (e) {
    //    e.preventDefault();

    //    if ($(this).hasClass('added')) {
    //        $(this).removeClass('added').find('i').removeClass('ti-check').addClass('ti-shopping-cart').siblings('span').text('add to cart');
    //    } else {
    //        $(this).addClass('added').find('i').addClass('ti-check').removeClass('ti-shopping-cart').siblings('span').text('added');
    //    }
    //});
    ///*--
    //    Wishlist & Compare
    //------------------------*/
    //$('.wishlist-compare a').on('click', function (e) {
    //    e.preventDefault();

    //    if ($(this).hasClass('added')) {
    //        $(this).removeClass('added');
    //    } else {
    //        $(this).addClass('added');
    //    }
    //});

    /*--
        Count Down Timer
    ------------------------*/
    $('[data-countdown]').each(function () {
        var $this = $(this), finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function (event) {
            $this.html(event.strftime('<span class="cdown day"><span class="time-count">%-D</span> <p>Days</p></span> <span class="cdown hour"><span class="time-count">%-H</span> <p>Hours</p></span> <span class="cdown minutes"><span class="time-count">%M</span> <p>Minute</p></span> <span class="cdown second"><span class="time-count">%S</span> <p>Second</p></span>'));
        });
    });

    /*--
        CLose Popup
    -----------------------------------*/
    $('.close-popup').on('click', function () {
        $('[data-modal="popup-modal"]').fadeOut('slow');
    });

    /*--
        Video Popup
    -----------------------------------*/
    var videoPopup = $('.video-popup');
    videoPopup.magnificPopup({
        disableOn: 700,
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        fixedContentPos: false
    });

    /*--
        Image Popup
    -----------------------------------*/
    //    var imagePopup = $('.image-popup, .big-image-popup');
    //    debugger;
    //imagePopup.magnificPopup({
    //    type: 'image',
    //    mainClass: 'mfp-fade',
    //});

    /*--
        Gallery Popup
    -----------------------------------*/
    var galleryPopup = $('.gallery-popup');
    galleryPopup.magnificPopup({
        type: 'image',
        mainClass: 'mfp-fade',
        gallery: {
            enabled: true,
        },
    });

    /*--
        Counter UP
    -----------------------------------*/
    var counter = $('.counter');
    counter.counterUp({
        delay: 20,
        time: 3000
    });

    /*--
        Twitter Feed
    -----------------------------------*/
    $('.footer-tweet').twittie({
        template: '<span class="author">{{screen_name}}</span>, {{tweet}}',
        count: 2,
        apiPath: 'assets/api/tweet.php',
    });

    /*--
        Scroll Up
    -----------------------------------*/
    $.scrollUp({
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade',
        scrollText: '<i class="icofont icofont-swoosh-up"></i>',
    });

    /*--
        Nice Select
    ------------------------*/
    $('.nice-select').niceSelect()

    /*--
        Price Range Slider
    ------------------------*/
    $('#price-range').slider({
        range: true,
        min: 0,
        max: 2000,
        values: [25, 970],
        slide: function (event, ui) {
            $('#price-amount').val('$' + ui.values[0] + ' TO $' + ui.values[1]);
        }
    });
    $('#price-amount').val('$' + $('#price-range').slider('values', 0) +
        ' TO $' + $('#price-range').slider('values', 1));

    /*----- 
        Quantity
    --------------------------------*/
    //$('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
    //$('.pro-qty').append('<span class="inc qtybtn">+</span>');
    //$('.qtybtn').on('click', function() {
    //	var $button = $(this);
    //	var oldValue = $button.parent().find('input').val();
    //	if ($button.hasClass('inc')) {
    //	  var newVal = parseFloat(oldValue) + 1;
    //	} else {
    //	   // Don't allow decrementing below zero
    //	  if (oldValue > 0) {
    //		var newVal = parseFloat(oldValue) - 1;
    //		} else {
    //		newVal = 0;
    //	  }
    //	  }
    //	$button.parent().find('input').val(newVal);
    //});  

    /*----- 
        Shipping Form Toggle
    --------------------------------*/
    $('[data-shipping]').on('click', function () {
        if ($('[data-shipping]:checked').length > 0) {
            $('#shipping-form').slideDown();
        } else {
            $('#shipping-form').slideUp();
        }
    })

    /*----- 
        Payment Method Select
    --------------------------------*/
    $('[name="payment-method"]').on('click', function () {

        var $value = $(this).attr('value');

        $('.single-method p').slideUp();
        $('[data-method="' + $value + '"]').slideDown();

    })

    /*----- 
        Account Image Upload
    --------------------------------*/
    $('#account-image-upload').on('change', function () {
        var filename = $(this).val();
        if (/^\s*$/.test(filename)) {
            $(".account-image-label").text("Choose your image");
        }
        else {
            $(".account-image-label").text(filename.replace("C:\\fakepath\\", ""));
        }
    });


})(jQuery);

