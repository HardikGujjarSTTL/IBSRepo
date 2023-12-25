/*---------------------------------------------------------------------*/
;(function($){

/*================= Global Variable Start =================*/		   
var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
var IEbellow9 = !$.support.leadingWhitespace;
var iPhoneAndiPad = /iPhone|iPod/i.test(navigator.userAgent);
var isIE = navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0;
function isIEver () {
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
//document.write('<link rel="stylesheet" type="text/css" href="' + cssFolder +'animate.css">');	

/*================= On Document Load Start =================*/	
$(document).ready( function(){
	$('body').removeClass('noJS').addClass("hasJS");	

// marquee
 if( $(".marqueeScrolling li").length > 1){
                var $mq = $('.marquee').marquee({
                         speed: 25000
                        ,gap: 0
                        ,duplicated: true
                        ,pauseOnHover: true
                        });
                $(".btnMPause").toggle(function(){
                        $(this).addClass('play');
                        $(this).text('Play');
                        $mq.marquee('pause');
                },function(){
                        $(this).removeClass('play');
                        $(this).text('Pause');
                        $mq.marquee('resume');
                        return false;
                });
        };
        
//Home Banner
	if($(".homeBannerImg").length){
		$(".homeBannerImg").each(function(){
			  var imagePath=$(this).find("img").attr("src");
			  $(this).css("background-image","url( "+imagePath+" )");
		});
	}
	if($(".homeImgSlider").length){
	  var swiper = new Swiper(".homeImgSlider", {
		speed: 2500,
    	spaceBetween: 0,
		slidesPerView: 1,
		lazy: true,
		loop:true,
		simulateTouch: true,
		keyboard:true,
		autoplay:false,					  
        navigation: {
        nextEl: " .homeImgSlider-next",
        prevEl: " .homeImgSlider-prev",
      },
    });
	}


	//Certification Slider
	if($(".certificationSlider").length){
		var certificationSlider = new Swiper('.certificationSlider .swiper-container', {
		speed: 1500,
    	spaceBetween:10,
		slidesPerView:3,
		lazy: true,
		loop:true,
		simulateTouch: true,
		autoplay:true,	
    	navigation: {
			nextEl: '.certificationSlider-prev',
			prevEl: '.certificationSlider-next',
        },
		breakpoints: {
		639: {
          slidesPerView: 1,
		  simulateTouch: true,
		  centerSlide:true,
        },
		992: {
          slidesPerView: 2,
		  simulateTouch: true,
		  centerSlide:true,
        },
		1600: {
          slidesPerView: 3,
		  simulateTouch: true,
		  centerSlide:true,
        }
			}
		});
		
		$('.certificationSlider .swiper-container').on('mouseenter', function (e) {
			certificationSlider.autoplay.stop();
		})
		$('.certificationSlider .swiper-container').on('mouseleave', function (e) {
			certificationSlider.autoplay.start();
		});
	}
	
	//Awards Slider
	if($(".awardsSlider").length){
		var awardsSlider = new Swiper('.awardsSlider .swiper-container', {
		speed: 1500,
    	spaceBetween:10,
		slidesPerView:3,
		lazy: true,
		loop:true,
		simulateTouch: true,
		autoplay:true,	
    	navigation: {
			nextEl: '.awardsSlider-next',
			prevEl: '.awardsSlider-prev',
        },
		breakpoints: {
		639: {
          slidesPerView: 1,
		  simulateTouch: true,
		  centerSlide:true,
        },
		992: {
          slidesPerView: 2,
		  simulateTouch: true,
		  centerSlide:true,
        },
		1600: {
          slidesPerView: 3,
		  simulateTouch: true,
		  centerSlide:true,
        }
      }
		
		});
		$('.awardsSlider .swiper-container').on('mouseenter', function (e) {
			awardsSlider.autoplay.stop();
		})
		$('.awardsSlider .swiper-container').on('mouseleave', function (e) {
			awardsSlider.autoplay.start();
		});
	}

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
		setTimeout(function () {
			$(".menuIcon").click(function () {
				$(this).prev().slideToggle();
				//if (!$(this).parent().hasClass("hover")) {
				//	$("#nav li").find("ul").siblings().slideUp();
				//}
			});
		}, 500);
		dropdown('nav', 'hover', 1);
		adjustMenu();
	};
	
	//Our Clients Slider
	if($(".ourClientSlider").length){
		var ourClientSlider = new Swiper('.ourClientSlider .swiper-container', {
		speed: 2000,
    	spaceBetween:0,
		slidesPerView: 8,
		lazy: true,
		loop:true,
		simulateTouch: true,
		autoplay:true,	
    	navigation: {
			nextEl: '.ourClientSlider-next',
			prevEl: '.ourClientSlider-prev',
        },
		breakpoints: {
		479: {
          slidesPerView: 2,
		  simulateTouch: true,
		  centerSlide:true,
        },
		639: {
          slidesPerView: 3,
		  simulateTouch: true,
		  centerSlide:true,
        },
		1024: {
          slidesPerView: 4,
		  simulateTouch: true,
		  centerSlide:true,
        },
		1169: {
          slidesPerView: 6,
		  simulateTouch: true,
		  centerSlide:true,
        },
		1600: {
          slidesPerView: 8,
		  simulateTouch: true,
		  centerSlide:true,
        }
      }
		
		});
	}

	//home gallery Slider
	if ($(".homeGallerySlider").length) {
		var ourClientSlider = new Swiper('.homeGallerySlider .swiper-container', {
			speed: 2000,
			spaceBetween: 10,
			slidesPerView: 8,
			lazy: true,
			loop: true,
			simulateTouch: true,
			autoplay: true,
			navigation: {
				nextEl: '.homeGallerySlider-next',
				prevEl: '.homeGallerySlider-prev',
			},
			breakpoints: {
				479: {
					slidesPerView: 1,
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
					slidesPerView: 4,
					simulateTouch: true,
					centerSlide: true,
				},
				1600: {
					slidesPerView: 4,
					simulateTouch: true,
					centerSlide: true,
				}
			}

		});
	}

	//footer logo Slider
	if ($(".footerLogoSlider").length) {
		var ourClientSlider = new Swiper('.footerLogoSlider .swiper-container', {
			speed: 2000,
			spaceBetween: 0,
			slidesPerView: 8,
			lazy: true,
			loop: true,
			simulateTouch: true,
			autoplay: true,
			navigation: {
				nextEl: '.footerLogoSlider-next',
				prevEl: '.footerLogoSlider-prev',
			},
			breakpoints: {
				479: {
					slidesPerView: 1,
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
					slidesPerView: 4,
					simulateTouch: true,
					centerSlide: true,
				},
				1600: {
					slidesPerView: 6,
					simulateTouch: true,
					centerSlide: true,
				}
			}

		});
	}


	$("#backToTop").hide();
	$(window).scroll(function () {
		if ($(this).scrollTop() > 100) {
			$('#backToTop').fadeIn('fast');
		} else {
			$('#backToTop').hide();
		}
	});
	$('#backToTop a').click(function () {
		$('html, body').animate({
			scrollTop: 0
		}, '200');
		return false;
	});


	$('a').not(".litebox, .galleryBox a").filter(function () {
		return this.hostname && this.hostname !== location.hostname;
	}).click(function (e) {
		e.preventDefault();
		var url = $(this).attr("href");
		smoke.confirm("You are about to proceed to an external website. Click Yes to proceed.", function (e) {
			if (e) {
				window.open(url, "_blank");
			} else {
				return false;
			}
		}, {
			ok: "Yes",
			cancel: "No",
			classname: "custom-class",
			reverseButtons: true
		});
	});

});	
})(jQuery);