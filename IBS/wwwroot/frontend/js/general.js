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
	}
	
	
	//Our Clients Slider
	if($(".ourClientSlider").length){
		var ourClientSlider = new Swiper('.ourClientSlider .swiper-container', {
		speed: 2000,
    	spaceBetween:10,
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
});	
})(jQuery);