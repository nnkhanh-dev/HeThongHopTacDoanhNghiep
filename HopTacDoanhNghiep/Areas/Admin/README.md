# Admin Layout Documentation

## Tổng quan
Layout Admin được thiết kế với sidebar và header responsive, hỗ trợ tốt trên các thiết bị mobile, tablet và desktop.

## Cấu trúc Layout

### 1. _Layout.cshtml
File layout chính bao gồm:
- Sidebar (navigation menu)
- Header (search, notifications, user menu)
- Main Content Area
- Footer (optional)

### 2. _Sidebar.cshtml
**Các tính năng:**
- Logo branding
- Navigation menu với icon
- Submenu có thể mở rộng/thu gọn
- Active state cho menu item hiện tại
- Responsive với overlay trên mobile

**Cách thêm menu item mới:**
```html
<!-- Menu item đơn -->
<div class="nav-item">
    <a href="/admin/your-route">
        <i class="fa-solid fa-icon-name"></i>
        <span>Tên Menu</span>
    </a>
</div>

<!-- Menu item với submenu -->
<div class="nav-submenu">
    <div class="nav-submenu-header">
        <i class="fa-solid fa-icon-name"></i>
        <span>Tên Menu Cha</span>
        <i class="fa-solid fa-chevron-down submenu-arrow"></i>
    </div>
    <div class="submenu">
        <div class="submenu-item">
            <a href="/admin/route-1">Submenu 1</a>
        </div>
        <div class="submenu-item">
            <a href="/admin/route-2">Submenu 2</a>
        </div>
    </div>
</div>
```

### 3. _Header.cshtml
**Các tính năng:**
- Toggle button cho sidebar (mobile)
- Search bar
- Notification icon với badge
- User dropdown menu

**Tùy chỉnh user menu:**
Chỉnh sửa dropdown menu trong _Header.cshtml:
```html
<ul class="dropdown-menu dropdown-menu-end">
    <li><a class="dropdown-item" href="/admin/your-route">
        <i class="fa-solid fa-icon"></i> Menu Item
    </a></li>
</ul>
```

## Responsive Breakpoints

- **Desktop**: > 992px - Sidebar hiển thị cố định
- **Tablet**: 768px - 992px - Sidebar có thể toggle
- **Mobile**: < 768px - Sidebar ẩn, hiển thị qua overlay

## CSS Classes

### Sidebar Classes
- `.sidebar` - Container chính
- `.sidebar.active` - Sidebar mở (mobile)
- `.nav-item` - Menu item đơn
- `.nav-submenu` - Menu có submenu
- `.nav-submenu.active` - Submenu đang mở

### Header Classes
- `.header` - Container chính
- `.sidebar-toggle` - Button toggle sidebar
- `.header-search` - Search box
- `.notification-btn` - Notification button
- `.user-btn` - User menu button

### Utility Classes
- `.text-primary` - Text màu primary
- `.text-secondary` - Text màu secondary
- `.bg-gradient-primary` - Background gradient primary
- `.fade-in` - Animation fade in

## JavaScript Functions

### site.js cung cấp:
1. **toggleSidebar()** - Mở/đóng sidebar trên mobile
2. **closeSidebar()** - Đóng sidebar
3. **Submenu toggle** - Mở/đóng submenu
4. **setActiveMenuItem()** - Đánh dấu menu item active
5. **Auto close sidebar** - Tự động đóng khi resize window

## Tùy chỉnh

### Thay đổi màu sắc
Chỉnh sửa trong `_variables.css`:
```css
:root {
    --primary-color: #3782f5;
    --secondary-color: #0a4181;
    /* ... */
}
```

### Thay đổi kích thước sidebar
Trong `style.css`:
```css
.sidebar {
    width: 250px; /* Thay đổi giá trị này */
    min-width: 250px;
}
```

### Thêm animation
Sử dụng các animation có sẵn hoặc tạo mới:
```css
@keyframes yourAnimation {
    from { /* ... */ }
    to { /* ... */ }
}
```

## Browser Support
- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- Mobile browsers (iOS Safari, Chrome Android)

## Dependencies
- Bootstrap 5.x
- Font Awesome 6.x
- jQuery (cho Bootstrap components)
- SweetAlert2 (cho notifications)

## Tips & Best Practices

1. **Luôn test trên nhiều kích thước màn hình**
2. **Sử dụng FontAwesome icons để đồng nhất**
3. **Giữ submenu không quá nhiều cấp (tối đa 2 cấp)**
4. **Đặt tên route rõ ràng và nhất quán**
5. **Thêm active class cho menu item tương ứng với trang hiện tại**

## Troubleshooting

### Sidebar không toggle trên mobile
- Kiểm tra xem `site.js` đã được load chưa
- Kiểm tra ID của các elements: `sidebar`, `sidebarToggle`, `sidebarClose`, `sidebarOverlay`

### Submenu không mở
- Kiểm tra class `.nav-submenu-header` có đúng không
- Kiểm tra JavaScript event listener

### Style không áp dụng
- Clear cache browser
- Kiểm tra thứ tự import CSS trong _Layout.cshtml
- Kiểm tra `asp-append-version="true"` trong link tag

## Support
Liên hệ team phát triển nếu cần hỗ trợ.
