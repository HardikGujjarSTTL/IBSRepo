/*---------------------------------------------------------------------*/
; (function ($) {

	/*================= Global Variable Start =================*/
	var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
	var IEbellow9 = !$.support.leadingWhitespace;
	var iPhoneAndiPad = /iPhone|iPod/i.test(navigator.userAgent);
	var isIE = navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0;
	function isIEver() {
		var myNav = navigator.userAgent.toLowerCase();
		return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
	}
	//if (isIEver () == 8) {}

	var jsFolder = "js/";
	var cssFolder = "css/";
	var ww = document.body.clientWidth, wh = document.body.clientHeight;
	var mobilePort = 800, ipadView = 1024, wideScreen = 1600;

	/*================= Global Variable End =================*/

	//css3 style calling 
	document.write('<link rel="stylesheet" type="text/css" href="' + cssFolder + 'animate.css">');

	/*================= On Document Load Start =================*/
	$(document).ready(function () {
		$('body').removeClass('noJS').addClass("hasJS");



		// Home page functions
		// marquee
		if ($(".marqueeScrolling li").length > 1) {
			var $mq = $('.marquee').marquee({
				speed: 25000
				, gap: 0
				, duplicated: true
				, pauseOnHover: true
			});
			$(".btnMPause").toggle(function () {
				$(this).addClass('play');
				$(this).text('Play');
				$mq.marquee('pause');
			}, function () {
				$(this).removeClass('play');
				$(this).text('Pause');
				$mq.marquee('resume');
				return false;
			});
		};



		//Home Banner
		if ($(".homeBannerImg").length) {
			$(".homeBannerImg").each(function () {
				var imagePath = $(this).find("img").attr("src");
				$(this).css("background-image", "url( " + imagePath + " )");
			});
		}
		if ($(".homeImgSlider").length) {
			var swiper = new Swiper(".homeImgSlider", {
				speed: 2500,
				spaceBetween: 0,
				slidesPerView: 1,
				lazy: true,
				loop: true,
				simulateTouch: true,
				keyboard: true,
				autoplay: false,
				navigation: {
					nextEl: " .homeImgSlider-next",
					prevEl: " .homeImgSlider-prev",
				},
			});
		}


		//Certification Slider
		if ($(".certificationSlider").length) {
			var certificationSlider = new Swiper('.certificationSlider .swiper-container', {
				speed: 1500,
				spaceBetween: 10,
				slidesPerView: 3,
				lazy: true,
				loop: true,
				simulateTouch: true,
				autoplay: true,
				navigation: {
					nextEl: '.certificationSlider-prev',
					prevEl: '.certificationSlider-next',
				},
				breakpoints: {
					639: {
						slidesPerView: 1,
						simulateTouch: true,
						centerSlide: true,
					},
					992: {
						slidesPerView: 2,
						simulateTouch: true,
						centerSlide: true,
					},
					1600: {
						slidesPerView: 3,
						simulateTouch: true,
						centerSlide: true,
					}
				}

			});
		}

		//Awards Slider
		if ($(".awardsSlider").length) {
			var awardsSlider = new Swiper('.awardsSlider .swiper-container', {
				speed: 1500,
				spaceBetween: 10,
				slidesPerView: 3,
				lazy: true,
				loop: true,
				simulateTouch: true,
				autoplay: true,
				navigation: {
					nextEl: '.awardsSlider-next',
					prevEl: '.awardsSlider-prev',
				},
				breakpoints: {
					639: {
						slidesPerView: 1,
						simulateTouch: true,
						centerSlide: true,
					},
					992: {
						slidesPerView: 2,
						simulateTouch: true,
						centerSlide: true,
					},
					1600: {
						slidesPerView: 3,
						simulateTouch: true,
						centerSlide: true,
					}
				}

			});
		}


		//Our Clients Slider
		if ($(".ourClientSlider").length) {
			var ourClientSlider = new Swiper('.ourClientSlider .swiper-container', {
				speed: 2000,
				spaceBetween: 10,
				slidesPerView: 8,
				lazy: true,
				loop: true,
				simulateTouch: true,
				autoplay: true,
				navigation: {
					nextEl: '.ourClientSlider-next',
					prevEl: '.ourClientSlider-prev',
				},
				breakpoints: {
					479: {
						slidesPerView: 2,
						simulateTouch: true,
						centerSlide: true,
					},
					639: {
						slidesPerView: 3,
						simulateTouch: true,
						centerSlide: true,
					},
					1024: {
						slidesPerView: 4,
						simulateTouch: true,
						centerSlide: true,
					},
					1169: {
						slidesPerView: 6,
						simulateTouch: true,
						centerSlide: true,
					},
					1600: {
						slidesPerView: 8,
						simulateTouch: true,
						centerSlide: true,
					}
				}

			});
		}



		if ($("#hdnSiteName").length) {
			var siteName = $("#hdnSiteName").val().replace(/\s+/g, '');
		}

		$(this).scrollTop(0);
		getWidth();

		//Set Element to vertical center using padding
		$.fn.verticalAlign = function () { return this.css("padding-top", ($(this).parent().height() - $(this).height()) / 2 + 'px'); };

		setTimeout(function () {
			$('.vCenter').each(function () { $(this).verticalAlign(); });
		}, 800);

		//seachToggle
		if ($(".searchIcon").length) {
			$(".searchIcon").click(function () {
				if (ww < 1023) {
					$(".searchFiled").slideToggle();
					$(this).toggleClass("closed");
				}
				else {
					$(".searchFiled").animate({ width: 'toggle' }, 700);
					$(this).toggleClass("closed");
				}
			});
		}
		//topDorp Popup
		if ($(".openDrpPopup").length) {
			$(".openDrpPopup").click(function () {
				if ($(this).next().is(":visible")) {
					$(this).next().slideUp("fast");
					$(this).removeClass("active");
					return false;
				}
				else {
					$(".openDrpPopup").removeClass("active");
					$(".dropPopup").slideUp("fast");
					$(this).next().slideDown("fast");
					$(this).addClass("active");
					return false;
				}

			});
		}
		$('body').on('click touchstart', function () {
			$('.messagePopup, .notifiaction').slideUp();
		});





		//openPopup Custom function
		if ($(".popupOpen").length) {
			$(".popupOpen").click(function (event) {
				event.preventDefault();
				var openPopup = $(this).attr("href");
				$(openPopup).removeClass("popupDisplayNone").addClass("popupDisplay");
			});
		}
		//ClosePoupup
		if ($(".cancelBtn, .closePopup").length) {
			$(".cancelBtn, .closePopup").click(function (event) {
				event.preventDefault();
				$(this).parents(".adminPopup").removeClass("popupDisplay").addClass("popupDisplayNone");
			});
		}
		$(document).keyup(function (e) {
			if (e.keyCode === 27) {
				$('.adminPopup .cancelBtn').trigger("click");
				$('.closePopup').trigger("click");
			}
		});

		// Multiple Ticker	
		if ($(".ticker").length) {
			$('.ticker').each(function (i) {
				$(this).addClass('tickerDiv' + i).attr('id', 'ticker' + i);
				$('#ticker' + i).find('.tickerDivBlock').first().addClass('newsTikker' + i).attr('id', 'newsTikker' + i);
				$('#ticker' + i).find('a.playPause').attr('id', 'stopNews' + i)
				$('#newsTikker' + i).vTicker({ speed: 1E3, pause: 4E3, animation: "fade", mousePause: false, showItems: 3, height: 150, direction: 'up' })
				$("#stopNews" + i).click(function () {
					if ($(this).hasClass('stop')) {
						$(this).removeClass("stop").addClass("play").text("Play").attr('title', 'Play');
					} else {
						$(this).removeClass("play").addClass("stop").text("Pause").attr('title', 'pause');
					}
					return false;
				});
			});
		};



		// Responsive Tabing Script
		if ($(".resTab").length) {
			$('.resTab').responsiveTabs({
				rotate: false
				, startCollapsed: 'tab' //accordion
				, collapsible: 'tab' //accordion
				, scrollToAccordion: true
			});
		};

		if ($(".accordion").length) {
			$('.accordion .accordDetail').hide();
			// temporary hide
			$(".accordion .accordDetail:nth-child(4)").show();
			$(".accordion .accTrigger:first").addClass("active");
			$('.accordion .accTrigger').click(function () {
				if ($(this).hasClass('active')) {
					$(this).removeClass('active');
					$(this).next().slideUp();
				} else {
					if ($('body').hasClass('desktop')) {
						$('.accordion .accTrigger').removeClass('active');
						$('.accordion .accordDetail').slideUp();
					}
					$(this).addClass('active');
					$(this).next().slideDown();
				}
				return false;
			});




		};



		if ($(".tableData").length > 0) {
			$('.tableData').each(function () {
				$(this).wrap('<div class="tableOut"></div>');
				$(this).find('tr').each(function () {
					$(this).find('td:first').addClass('firstTd');
					$(this).find('th:first').addClass('firstTh');
					$(this).find('th:last').addClass('lastTh');
				});
				$(this).find('tr:last').addClass('lastTr');
				$(this).find('tr:even').addClass('evenRow');
				$(this).find('tr:nth-child(2)').find('th:first').removeClass('firstTh');
			});
		};


		/*Login droupdown Code*/
		if ($(".loginDrpPopup").length > 0) {
			$('.loginLink').click(function () {
				if ($('.loginDrpPopup').css("display") == "block") {
					$(this).removeClass('active');
					$('.loginDrpPopup').slideUp();
				} else {
					$(this).addClass('active');
					$('.loginDrpPopup').slideDown();
				}
				return false;
			});

			$(document).bind('mousedown touchstart', function (e) {
				//if (ww < 767) {
				if ($(e.target).closest('.loginDrpPopup').length === 0) {
					$('.loginLink').removeClass('active');
					$('.loginDrpPopup').slideUp();
				}
				//}
			});
		}

		// Responsive Table
		if ($(".responsiveTable").length) {
			$(".responsiveTable").each(function () {
				$(this).find('td').removeAttr('width');
				//$(this).find('td').removeAttr('align');
				var head_col_count = $(this).find('tr th').size();
				// loop which replaces td
				for (i = 0; i <= head_col_count; i++) {
					// head column label extraction
					var head_col_label = $(this).find('tr th:nth-child(' + i + ')').text();
					// replaces td with <div class="column" data-label="label">
					$(this).find('tr td:nth-child(' + i + ')').attr("data-label", head_col_label);
				}
			});
		};

		// Responsive Table
		if ($(".tableScroll").length) {
			$(".tableScroll").each(function () {
				$(this).wrap('<div class="tableOut"></div>');
			});
		};

		// Get Focus Inputbox
		if ($(".getFocus").length) {
			$(".getFocus").each(function () {
				$(this).on("focus", function () {
					if ($(this).val() == $(this)[0].defaultValue) { $(this).val(""); };
				}).on("blur", function () {
					if ($(this).val() == "") { $(this).val($(this)[0].defaultValue); };
				});
			});
		};

		// For device checking
		if (isMobile == false) {

		};

		//$('.equalHeights > div').equalHeight();

		setTimeout(function () {
			if ($(".fixedErrorMsg").length) {
				$(".fixedErrorMsg").slideDown("slow");
				setTimeout(function () { $('.fixedErrorMsg').slideUp(); }, 5000);
			}
			if ($(".fixedSuccessMsg").length) {
				$(".fixedSuccessMsg").slideDown("slow");
				setTimeout(function () { $('.fixedSuccessMsg').slideUp(); }, 5000);
			}
		}, 500);

		/*================= On Document Load and Resize Start =================*/
		$(window).on('resize', function () {

			ww = document.body.clientWidth;
			wh = document.body.clientHeight;

			$('.vCenter').each(function () { $(this).verticalAlign(); });

			if ($("body").hasClass("mobilePort")) {
				$("body").removeClass("wob");
			}

			//$('.container').resize(function(){});

		}).trigger('resize');
		/*================= On Document Load and Resize End =================*/

		/*Navigation */
		if ($("#nav").length) {
			if ($(".toggleMenu").length == 0) {
				$("#mainNav").prepend('<a href="#" class="toggleMenu"><span class="mobileMenu">Menu</span><span class="iconBar"></span></a>');
			}
			$(".toggleMenu").click(function () {
				$(this).toggleClass("active");
				$("#nav").slideToggle();
				return false;
			});
			$("#nav li a").each(function () {
				if ($(this).next().length) {
					$(this).parent().addClass("parent");
				};
			})
			$("#nav li.parent").each(function () {
				if ($(this).has(".menuIcon").length <= 0) $(this).append('<i class="menuIcon">&nbsp;</i>')
			});
			dropdown('nav', 'hover', 1);
			adjustMenu();
		};







		// Navigation

		if ($("#menu-toggle").length) {
			$('body').removeClass('left-side-collapsed nav-hover').addClass("left-side-full");
			setTimeout(function () { $('body').addClass("left-side-opend"); }, 700)
			$('.navbar-toggle').removeClass('active');
			$('ul#side-menu li').hover(function () {
				if ($('body').hasClass('left-side-collapsed')) {
					$(this).addClass('nav-hover');
				}
			}, function () {
				if ($('body').hasClass('left-side-collapsed')) {
					$(this).removeClass('nav-hover');
				}
			});

			if ($("body").hasClass("left-side-full")) {


				$('.nav-second-level').removeClass("show").addClass("hide");
				var url = location.pathname;
				var urlsplit = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
				var href = $(this).attr('href');
				$('.nav-second-level li a').each(function () {
					var href = $(this).attr('href');
					if (href == urlsplit) {
						$(this).addClass('activeLink');
						$(this).parents('.nav-second-level').parent('li').addClass('active');
						$(this).parents('.nav-second-level').addClass('show');
					}

				});


				/*$('ul#side-menu > li').click(function () {
					$('ul#side-menu > li').removeClass('active');
					if ($(this).children("ul").length) {	
						if ($(this).hasClass('active')) {
						   $(this).removeClass('active');
						   $(this).children("ul").removeClass("show").addClass("hide");
						  } else {
							   $(this).addClass('active');
							   $('.nav-second-level').removeClass("show").addClass("hide");
							   $(this).children("ul").removeClass("hide").addClass("show");
							   
							 // $("#container").css({"min-height": $("#sidebar").outerHeight()}); 
						  }
					}
					else {}
				});*/
				$('ul#side-menu > li').click(function () {
					if ($(this).children("ul").length) {
						if ($(this).hasClass('active')) {
							$(this).children("ul").slideToggle();
							$(this).toggleClass('active');
						} else {
							$(this).children("ul").slideToggle();
							$(this).toggleClass('active');
						}
					}
					else { }
				});


			};

			if ($.cookie(siteName) == 1) {
				$('body').removeClass("left-side-full").addClass('left-side-collapsed nav-hover');
				$("#menu-toggle").addClass('active');
				setTimeout(function () { $('body').removeClass("left-side-opend"); }, 700)

			}
			$('#menu-toggle').bind('click', function () {
				if (!$("body").hasClass("menu-animate")) {
					$("body").addClass("menu-animate");
				}
				if ($("body").hasClass("mobileMenu")) {

					if (!$("#sidebar").hasClass("open")) {
						$("#sidebar").animate({ left: "0" }, 200);
						$("#sidebar").addClass("open");
						$(this).removeClass("active")
					}
					else {
						$("#sidebar").animate({ left: "0px" }, 200);
						$("#sidebar").removeClass("open");
						$(this).addClass("active");
					}
				}
				else {

					$.removeCookie(siteName);

					if ($("body").hasClass("left-side-collapsed nav-hover")) {
						$.removeCookie(siteName);
						$.cookie(siteName, 2, { expires: 20, path: '/' });
						$('body').removeClass('left-side-collapsed nav-hover').addClass("left-side-full");
						setTimeout(function () { $('body').addClass("left-side-opend"); }, 700)
						$(this).removeClass('active');

					}
					else {
						$.cookie(siteName, 1, { expires: 20, path: '/' });
						$('body').removeClass("left-side-full").addClass('left-side-collapsed nav-hover');
						setTimeout(function () { $('body').removeClass("left-side-opend"); }, 700)
						$(this).addClass('active');

					}

				}

			});




		}

		/*-------------------------------- Navigation ---------------------------------*/



		// Custom Select Box
		/*if( $('select').length){
			$('select').customSelect();
		}*/

		$('input.inputicheck').iCheck({
			checkboxClass: 'icheckbox_square',
			radioClass: 'iradio_square'
		});


		// Message on Cookie Disabled
		$.cookie('cookieWorked', 'yes', { path: '/' });
		if ($.cookie('cookieWorked') == 'yes') {
		}
		else {
			if ($("div.jsRequired").length == 0) {
				$("body").prepend(
					'<div class="jsRequired">Cookies are not enabled on your browser. Need to adjust this in your browser security preferences. Please enable cookies for better user experience.</div>'
				);
			}
		}


		$(".datepicker").datepicker();

	});
	/*================= On Document Load End =================*/

	/*================= On Window Resize Start =================*/
	$(window).bind('resize orientationchange', function () {
		getWidth();
		adjustMenu();
		$('.vCenter').each(function () { $(this).verticalAlign(); });
	});

	/*================= On Window Resize End =================*/

	/*================= On Window Load Start =================*/
	$(window).load(function () {

	});
	/*================= On Document Load End =================*/


	function getWidth() {
		ww = document.body.clientWidth;
		if (ww > wideScreen) { $('body').removeClass('device').addClass('desktop widerDesktop'); }
		if (ww > mobilePort && ww <= wideScreen) { $('body').removeClass('device widerDesktop').addClass('desktop'); }
		if (ww <= mobilePort) { $('body').removeClass('desktop widerDesktop').addClass('device'); }
		if (ww > 767 && ww < 1025) { $('body').addClass('ipad'); }
		else { $('body').removeClass('ipad'); }

	}

})(jQuery);


// added for left links selected

$(function () {
	var url = window.location.pathname,
		urlRegExp = new RegExp(url.replace(/\/$/, '') + "$"); // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there
	// now grab every link from the navigation
	$('#side-menu a').each(function () {
		// and test its normalized href against the url pathname regexp
		if (urlRegExp.test(this.href.replace(/\/$/, ''))) {
			$(this).addClass('selectLink');
		}
	});
});