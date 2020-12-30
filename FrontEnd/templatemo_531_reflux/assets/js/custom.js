(function($) {
    var toggle = document.getElementById("menu-toggle");
    var menu = document.getElementById("menu");
    var close = document.getElementById("menu-close");

    toggle.addEventListener("click", function(e) {
        if (menu.classList.contains("open")) {
            menu.classList.remove("open");
        } else {
            menu.classList.add("open");
        }
    });

    close.addEventListener("click", function(e) {
        menu.classList.remove("open");
    });

    // Close menu after click on smaller screens
    $(window).on("resize", function() {
        if ($(window).width() < 846) {
            $(".main-menu a").on("click", function() {
                menu.classList.remove("open");
            });
        }
    });

    $(".hover").mouseleave(function() {
        $(this).removeClass("hover");
    });

    $(".isotope-wrapper").each(function() {
        var $isotope = $(".isotope-box", this);
        var $filterCheckboxes = $('input[type="radio"]', this);

        var filter = function() {
            var type = $filterCheckboxes.filter(":checked").data("type") || "*";
            if (type !== "*") {
                type = '[data-type="' + type + '"]';
            }
            $isotope.isotope({ filter: type });
        };

        $isotope.isotope({
            itemSelector: ".isotope-item",
            layoutMode: "masonry"
        });

        $(this).on("change", filter);
        filter();
    });

    lightbox.option({
        resizeDuration: 200,
        wrapAround: true
    });
})(jQuery);
(function($) {
    $(".main-menu li:first").addClass("active");

    var showSection = function showSection(section, isAnimate) {
        var direction = section.replace(/#/, ""),
            reqSection = $(".section").filter(
                '[data-section="' + direction + '"]'
            ),
            reqSectionPos = reqSection.offset().top - 0;

        if (isAnimate) {
            $("body, html").animate({
                    scrollTop: reqSectionPos
                },
                800
            );
        } else {
            $("body, html").scrollTop(reqSectionPos);
        }
    };

    var checkSection = function checkSection() {
        $(".section").each(function() {
            var $this = $(this),
                topEdge = $this.offset().top - 80,
                bottomEdge = topEdge + $this.height(),
                wScroll = $(window).scrollTop();
			var scrolledToBottom = $(window).scrollTop() + $(window).height() > $(document).height() - 100;
			var currentId = $this.data("section");
            if ((topEdge < wScroll && bottomEdge > wScroll) || (scrolledToBottom && currentId == 'contact')) {
                var reqLink = $("a").filter("[href*=\\#" + currentId + "]");
                reqLink
                    .closest("li")
                    .addClass("active")
                    .siblings()
                    .removeClass("active");
            }
        });
    };

    $(".main-menu").on("click", "a", function(e) {
        e.preventDefault();
        showSection($(this).attr("href"), true);
    });

    $(window).scroll(function() {
        checkSection();
    });

    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        responsive: {
            0: {
                items: 3
            },
            600: {
                items: 3
            },
            1000: {
                items: 7
            }
        }
    })
})(jQuery);