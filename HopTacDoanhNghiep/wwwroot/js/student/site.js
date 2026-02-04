// Admin Layout JavaScript
(function() {
    'use strict';

    // DOM Elements
    const sidebar = document.getElementById('sidebar');
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebarClose = document.getElementById('sidebarClose');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    const navSubmenus = document.querySelectorAll('.nav-submenu');

    // Toggle Sidebar (Mobile)
    function toggleSidebar() {
        sidebar.classList.toggle('active');
        sidebarOverlay.classList.toggle('active');
        document.body.style.overflow = sidebar.classList.contains('active') ? 'hidden' : '';
    }

    function closeSidebar() {
        sidebar.classList.remove('active');
        sidebarOverlay.classList.remove('active');
        document.body.style.overflow = '';
    }

    // Event Listeners for Sidebar Toggle
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', toggleSidebar);
    }

    if (sidebarClose) {
        sidebarClose.addEventListener('click', closeSidebar);
    }

    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', closeSidebar);
    }

    // Submenu Toggle
    navSubmenus.forEach(function(submenu) {
        const header = submenu.querySelector('.nav-submenu-header');
        if (header) {
            header.addEventListener('click', function() {
                // Close other submenus
                navSubmenus.forEach(function(otherSubmenu) {
                    if (otherSubmenu !== submenu) {
                        otherSubmenu.classList.remove('active');
                    }
                });

                // Toggle current submenu
                submenu.classList.toggle('active');
            });
        }
    });

    // Active Menu Item
    function setActiveMenuItem() {
        const currentPath = window.location.pathname;
        const navItems = document.querySelectorAll('.nav-item a, .submenu-item a');

        navItems.forEach(function(item) {
            const href = item.getAttribute('href');
            if (href && currentPath.startsWith(href) && href !== '/') {
                item.classList.add('active');
                
                // If it's a submenu item, open its parent submenu
                const parentSubmenu = item.closest('.nav-submenu');
                if (parentSubmenu) {
                    parentSubmenu.classList.add('active');
                }
            }
        });
    }

    // Close sidebar on window resize if screen is large
    window.addEventListener('resize', function() {
        if (window.innerWidth > 992) {
            closeSidebar();
        }
    });

    // Initialize
    document.addEventListener('DOMContentLoaded', function() {
        setActiveMenuItem();
    });

    // Search functionality (placeholder)
    const headerSearch = document.querySelector('.header-search input');
    if (headerSearch) {
        headerSearch.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                const searchTerm = this.value.trim();
                if (searchTerm) {
                    console.log('Searching for:', searchTerm);
                    // Implement search functionality here
                }
            }
        });
    }

})();
