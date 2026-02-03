// Global variables
var ckeditorInstance;

// Document ready
$(document).ready(function () {
    initDanhMucSelect();
    initDanhMucFilter();
    initStatusFilter();
    initCKEditor();
});

// ========================================
// SELECT2 FOR CATEGORY
// ========================================

/**
 * Initialize Select2 for category dropdown with AJAX
 */
function initDanhMucSelect() {
    var $select = $('#DanhMucSelect');
    
    // Only init if select exists on page
    if ($select.length === 0) {
        return;
    }

    var initialVal = $select.data('initial-val');
    var initialText = $select.data('initial-text');

    $select.select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Chọn danh mục',
        allowClear: true,
        ajax: {
            url: '/admin/danh-muc-bai-viet/danh-sach',
            type: 'GET',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    keyword: params.term,
                    pageIndex: params.page || 1,
                    pageSize: 20
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                var results = [];
                var items = data.data || data.records || [];
                
                if (items.length > 0) {
                    results = items.map(function (item) {
                        return {
                            id: item.id,
                            text: item.ten
                        };
                    });
                }

                return {
                    results: results,
                    pagination: {
                        more: false
                    }
                };
            },
            cache: true
        },
        language: {
            errorLoading: function () {
                return 'Không thể tải dữ liệu';
            },
            inputTooShort: function () {
                return 'Nhập từ khóa để tìm kiếm';
            },
            loadingMore: function () {
                return 'Đang tải thêm...';
            },
            noResults: function () {
                return 'Không tìm thấy kết quả';
            },
            searching: function () {
                return 'Đang tìm...';
            }
        }
    });

    // Pre-populate if editing
    if (initialVal && initialText) {
        var option = new Option(initialText, initialVal, true, true);
        $select.append(option).trigger('change');
    }
}

// ========================================
// CATEGORY FILTER FOR INDEX PAGE
// ========================================

/**
 * Initialize Select2 for category filter on index page
 */
function initDanhMucFilter() {
    var $filter = $('#DanhMucFilter');
    
    // Only init if filter exists on page
    if ($filter.length === 0) {
        return;
    }

    // Get initial value from URL if exists
    var initialVal = new URLSearchParams(window.location.search).get('danhMucId');

    // Load initial value first if exists
    if (initialVal) {
        $.ajax({
            url: '/admin/danh-muc-bai-viet/danh-sach',
            type: 'GET',
            dataType: 'json',
            data: { pageSize: 100 },
            success: function (data) {
                var items = data.data || data.records || [];
                var selectedItem = items.find(function(item) {
                    return item.id == initialVal;
                });
                if (selectedItem) {
                    // Add option before initializing Select2
                    var option = new Option(selectedItem.ten, selectedItem.id, true, true);
                    $filter.append(option);
                }
                // Initialize Select2 after adding the option
                initSelect2ForDanhMucFilter($filter);
            },
            error: function() {
                // Initialize Select2 even if loading fails
                initSelect2ForDanhMucFilter($filter);
            }
        });
    } else {
        // Initialize Select2 directly if no initial value
        initSelect2ForDanhMucFilter($filter);
    }
}

/**
 * Initialize Select2 configuration for DanhMucFilter
 */
function initSelect2ForDanhMucFilter($filter) {
    $filter.select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Tất cả danh mục',
        allowClear: true,
        ajax: {
            url: '/admin/danh-muc-bai-viet/danh-sach',
            type: 'GET',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    keyword: params.term,
                    pageIndex: params.page || 1,
                    pageSize: 50
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                var results = [];
                var items = data.data || data.records || [];
                
                if (items.length > 0) {
                    results = items.map(function (item) {
                        return {
                            id: item.id,
                            text: item.ten
                        };
                    });
                }

                return {
                    results: results,
                    pagination: {
                        more: false
                    }
                };
            },
            cache: true
        },
        language: {
            errorLoading: function () {
                return 'Không thể tải dữ liệu';
            },
            noResults: function () {
                return 'Không tìm thấy kết quả';
            },
            searching: function () {
                return 'Đang tìm...';
            }
        }
    });
}

// ========================================
// STATUS FILTER
// ========================================

/**
 * Initialize Select2 for status filter
 */
function initStatusFilter() {
    var $statusFilter = $('select[name="status"]');
    
    // Only init if filter exists on page
    if ($statusFilter.length === 0) {
        return;
    }

    $statusFilter.select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Tất cả trạng thái',
        allowClear: true,
        minimumResultsForSearch: Infinity // Hide search box for simple select
    });
}

// ========================================
// CKEDITOR 4 INITIALIZATION
// ========================================

/**
 * Initialize CKEditor 4 with image upload
 */
function initCKEditor() {
    var $editor = $('#NoiDung');
    
    // Only init if editor exists on page
    if ($editor.length === 0) {
        return;
    }

    // Destroy existing instance if any
    if (ckeditorInstance) {
        ckeditorInstance.destroy();
        ckeditorInstance = null;
    }

    ckeditorInstance = CKEDITOR.replace('NoiDung', {
        height: 400,
        language: 'vi',
        toolbar: [
            { name: 'document', items: ['Source', '-', 'Preview'] },
            { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
            { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll'] },
            '/',
            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
            { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'SpecialChar'] },
            '/',
            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
            { name: 'colors', items: ['TextColor', 'BGColor'] },
            { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
        ],
        filebrowserImageUploadUrl: '/admin/bai-viet/upload-image',
        imageUploadUrl: '/admin/bai-viet/upload-image',
        uploadUrl: '/admin/bai-viet/upload-image',
        removePlugins: 'easyimage',
        image_previewText: ' ',
        removeDialogTabs: 'image:advanced;link:advanced'
    });

    // Handle upload success
    ckeditorInstance.on('fileUploadResponse', function (evt) {
        evt.stop();

        var data = evt.data;
        var xhr = data.fileLoader.xhr;
        var response = xhr.responseText;

        try {
            var result = JSON.parse(response);
            
            if (result.uploaded === 1) {
                data.url = result.url;
            } else {
                evt.cancel();
                data.message = result.error?.message || 'Upload failed';
            }
        } catch (e) {
            evt.cancel();
            data.message = 'Lỗi xử lý phản hồi từ server';
        }
    });

    // Handle upload request
    ckeditorInstance.on('fileUploadRequest', function (evt) {
        var fileLoader = evt.data.fileLoader;
        var formData = new FormData();
        var xhr = fileLoader.xhr;

        xhr.open('POST', fileLoader.uploadUrl, true);
        formData.append('upload', fileLoader.file, fileLoader.fileName);
        fileLoader.xhr.send(formData);

        evt.stop();
    });
}

// ========================================
// DELETE OPERATION
// ========================================

/**
 * Delete bai viet with confirmation
 */
function DeleteBaiViet(id, title) {
    if (!id) {
        showError('ID không hợp lệ');
        return;
    }

    var confirmMessage = 'Bạn có chắc chắn muốn xóa bài viết "' + escapeHtml(title) + '"?';

    Swal.fire({
        title: 'Xác nhận xóa',
        text: confirmMessage,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then(function (result) {
        if (result.isConfirmed) {
            performDelete(id);
        }
    });
}

/**
 * Perform actual delete via AJAX
 */
function performDelete(id) {
    $.ajax({
        type: 'DELETE',
        url: '/admin/bai-viet/xoa/' + id,
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: response.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    window.location.reload();
                });
            } else {
                showError(response.message || 'Không thể xóa bài viết');
            }
        },
        error: function (xhr, status, error) {
            console.error('Delete error:', error);
            showError('Có lỗi xảy ra khi xóa bài viết');
        }
    });
}

// ========================================
// UTILITY FUNCTIONS
// ========================================

/**
 * Escape HTML to prevent XSS
 */
function escapeHtml(text) {
    if (!text) return '';
    
    var map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    
    return String(text).replace(/[&<>"']/g, function (m) {
        return map[m];
    });
}

/**
 * Show success message
 */
function showSuccess(message) {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: 'success',
            title: 'Thành công',
            text: message,
            timer: 2000,
            showConfirmButton: false
        });
    } else {
        alert(message);
    }
}

/**
 * Show error message
 */
function showError(message) {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: message
        });
    } else {
        alert(message);
    }
}
