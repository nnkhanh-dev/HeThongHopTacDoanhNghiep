// Swiper initialization for banners and posts

document.addEventListener('DOMContentLoaded', function () {
    
    // ===== Banner Slider =====
    const bannerSwiper = new Swiper('.banner-slider', {
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        pagination: {
            el: '.banner-slider .swiper-pagination',
            clickable: true,
        },
        speed: 800,
        effect: 'slide',
    });

    // ===== Posts Slider =====
    const postsSwiper = new Swiper('.posts-slider', {
        slidesPerView: 1,
        spaceBetween: 16,
        pagination: {
            el: '.posts-slider .swiper-pagination',
            clickable: true,
            dynamicBullets: true,
        },
        breakpoints: {
            // Mobile
            576: {
                slidesPerView: 2,
                spaceBetween: 20,
            },
            // Tablet
            992: {
                slidesPerView: 3,
                spaceBetween: 24,
            },
        },
        speed: 500,
        grabCursor: true,
    });

});
