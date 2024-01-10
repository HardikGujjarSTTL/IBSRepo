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

	var jsFolder = "/js/";
	var cssFolder = "/css/";

	var ww = document.body.clientWidth, wh = document.body.clientHeight;
	var mobilePort = 800, ipadView = 1024, wideScreen = 1600;

	/*================= Global Variable End =================*/

	//css3 style calling 
	document.write('<link rel="stylesheet" type="text/css" href="' + cssFolder + 'animate.css">');

	/*================= On Document Load Start =================*/
	$(document).ready(function () {
		$('body').removeClass('noJS').addClass("hasJS");

		if ($("#hdnSiteName").length) {
			var siteName = $("#hdnSiteName").val().replace(/\s+/g, '');
		}

		$(this).scrollTop(0);
		getWidth();

		//Set Element to vertical center using padding
		/*$.fn.verticalAlign = function () { return this.css("padding-top", ($(this).parent().height() - $(this).height()) / 2 + 'px'); };

		setTimeout(function () {
			$('.vCenter').each(function () { $(this).verticalAlign(); });
		}, 800);*/

		//seachToggle
		/*if ($(".searchIcon").length) {
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
		}*/
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
		/*if ($(".popupOpen").length) {
			$(".popupOpen").click(function (event) {
				event.preventDefault();
				var openPopup = $(this).attr("href");
				$(openPopup).removeClass("popupDisplayNone").addClass("popupDisplay");
			});
		}*/
		//ClosePoupup
		/*if ($(".cancelBtn, .closePopup").length) {
			$(".cancelBtn, .closePopup").click(function (event) {
				event.preventDefault();
				$(this).parents(".adminPopup").removeClass("popupDisplay").addClass("popupDisplayNone");
			});
		}*/
		$(document).keyup(function (e) {
			if (e.keyCode === 27) {
				$('.adminPopup .cancelBtn').trigger("click");
				$('.closePopup').trigger("click");
			}
		});

		// Responsive Tabing Script
		/*if ($(".resTab").length) {
			$('.resTab').responsiveTabs({
				rotate: false
				, startCollapsed: 'tab' //accordion
				, collapsible: 'tab' //accordion
				, scrollToAccordion: true
			});
		};*/

		/*if ($(".accordion").length) {
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
		};*/

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
		/*if ($(".loginDrpPopup").length > 0) {
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
		}*/

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
		/*if ($(".getFocus").length) {
			$(".getFocus").each(function () {
				$(this).on("focus", function () {
					if ($(this).val() == $(this)[0].defaultValue) { $(this).val(""); };
				}).on("blur", function () {
					if ($(this).val() == "") { $(this).val($(this)[0].defaultValue); };
				});
			});
		};*/

		// For device checking
		if (isMobile == false) {

		};

		//$('.equalHeights > div').equalHeight();

		var midContentHt = $(".adminInnerContent").height();
		var sideBarHt = $(".sidebar-collapse").height();
		$(".sidebar-collapse").on('resize', function () {
			if ($(".sidebar-collapse").height() > midContentHt) {
				//$(".adminInnerContent").css('height', sideBarHt); 
			} else {
				$(".#sidebar").css('height', midContentHt);
				//$(".adminInnerContent").removeAttr('style');
			}

		}).trigger('resize');;

		/*setTimeout(function () {
			if ($(".fixedErrorMsg").length) {
				$(".fixedErrorMsg").slideDown("slow");
				setTimeout(function () { $('.fixedErrorMsg').slideUp(); }, 5000);
			}
			if ($(".fixedSuccessMsg").length) {
				$(".fixedSuccessMsg").slideDown("slow");
				setTimeout(function () { $('.fixedSuccessMsg').slideUp(); }, 5000);
			}
		}, 500);*/

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
		/*if ($("#nav").length) {
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
		};*/

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

			if ($(window).width() < 1024) {
				setTimeout(function () {
					$("body").removeClass("left-side-opend").addClass("ipadView");
					$("#menu-toggle").click(function () {
						if ($('body').hasClass("ipadView")) {
							$('body').removeClass("left-side-opend");
						} else {
							$('body').removeClass("left-side-opend");
						}
					});
				}, 800);
			}

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

				$('ul#side-menu > li').click(function () {
					if ($(this).children("ul").length) {
						if ($(this).hasClass('active')) {
							$(this).removeClass('active');
							$(this).children("ul").slideUp();
							
						} else {
							$(this).children("ul").slideDown();
							$(this).addClass('active');	
							$(this).siblings().removeClass('active');	
							$(this).siblings().children("ul").slideUp();
						}
					}
				});



				/* $('ul#side-menu > li').click(function () {
					$('ul#side-menu > li').removeClass('active');
					if ($(this).children("ul").length) {
						if ($(this).hasClass('active')) {
							$(this).removeClass('active');
							$(this).children("ul").removeClass("show").addClass("hide");
							
						} else {

							$(this).addClass('active');
							$('.nav-second-level').removeClass("show").addClass("hide");
							$(this).children("ul").removeClass("hide").addClass("show");

							//$("#container").css({ "min-height": $("#sidebar").outerHeight() });
						}
					}
					else { }
				});*/


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
						$("#sidebar").animate({ left: "314px" }, 200);
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

		/*$('input.inputicheck').iCheck({
			checkboxClass: 'icheckbox_square',
			radioClass: 'iradio_square'
		});*/

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

		$(".datepicker").datepicker({
			changeMonth: true,
			changeYear: true,
			dateFormat: 'dd/mm/yy',
		}).mask("00/00/0000");
	});

	$(window).bind('resize orientationchange', function () {
		getWidth();
		//adjustMenu();
		//$('.vCenter').each(function () { $(this).verticalAlign(); });
	});

	function getWidth() {
		ww = document.body.clientWidth;
		if (ww > wideScreen) { $('body').removeClass('device').addClass('desktop widerDesktop'); }
		if (ww > mobilePort && ww <= wideScreen) { $('body').removeClass('device widerDesktop').addClass('desktop'); }
		if (ww <= mobilePort) { $('body').removeClass('desktop widerDesktop').addClass('device'); }
		if (ww > 767 && ww < 1025) { $('body').addClass('ipad'); }
		else { $('body').removeClass('ipad'); }
	}

	$(".select2").select2();

	if ($(".nav-third-level").length) {
		//if ($(".nav-third-level").is(":visible")) {
		$(".nav-third-level").prev().addClass("thirdLabelTrigger");
		$(".thirdLabelTrigger").click(function () {
			$(this).next().slideToggle();
			$(this).toggleClass("active3Label");
			return false;
		});
		//}
	}
	/*if ($("ul#side-menu > li a").length) {
		$('ul#side-menu > li a').click(function () {
			$(this).parent().addClass('mySelected').siblings().removeClass('mySelected');
		});
	}*/

	
	
	$('.chatImagePopup').magnificPopup({
			type: 'image',
			closeOnContentClick: true,
			closeBtnInside: false,
			fixedContentPos: true,
			mainClass: 'mfp-no-margins mfp-with-zoom',
			image: {
				verticalFit: true
			},
			zoom: {
				enabled: true,
				duration: 300
			}
	});
	
	



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
			//$(this).parent().parent().slideDown();
		}
	});
});