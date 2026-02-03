// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
	var toggle = document.querySelector('.menu-toggle');
	var mobileMenu = document.querySelector('.mobile-menu');

	if (toggle && mobileMenu) {
		toggle.style.cursor = 'pointer';
		toggle.addEventListener('click', function () {
			mobileMenu.classList.toggle('open');
			toggle.classList.toggle('open');
			document.body.classList.toggle('no-scroll');
		});

		// Close menu when clicking outside (on small screens)
		document.addEventListener('click', function (e) {
			if (!mobileMenu.contains(e.target) && !toggle.contains(e.target)) {
				if (mobileMenu.classList.contains('open')) {
					mobileMenu.classList.remove('open');
					toggle.classList.remove('open');
					document.body.classList.remove('no-scroll');
				}
			}
		});

		// Submenu toggle for mobile
		var submenuWrappers = mobileMenu.querySelectorAll('.submenu-wrapper');
		submenuWrappers.forEach(function (wrapper) {
			wrapper.addEventListener('click', function (e) {
				// If an anchor inside was clicked, let it navigate
				if (e.target && e.target.tagName && e.target.tagName.toLowerCase() === 'a') return;
				wrapper.classList.toggle('active');
			});
		});

		// Reset state on resize to desktop
		window.addEventListener('resize', function () {
			if (window.innerWidth >= 992) {
				if (mobileMenu.classList.contains('open')) mobileMenu.classList.remove('open');
				if (toggle.classList.contains('open')) toggle.classList.remove('open');
				document.body.classList.remove('no-scroll');
				submenuWrappers.forEach(function (w) { w.classList.remove('active'); });
			}
		});
	}

});
