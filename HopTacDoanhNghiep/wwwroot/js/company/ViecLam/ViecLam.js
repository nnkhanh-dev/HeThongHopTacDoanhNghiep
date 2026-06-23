// Global variables
var ckeditorInstances = {};

// Document ready
$(document).ready(function () {
    // initLinhVucSelect();
    // initLinhVucFilter();
    initCKEditor();
    initSalaryRangeSliders();
    initSortSalaryRadio();
});

// ========================================
// SELECT2 FOR LINH VUC
// ========================================

/**
 * Initialize salary range sliders
 */
function initSalaryRangeSliders() {
    var $luongMinRange = $('#luongMinRange');
    var $luongMaxRange = $('#luongMaxRange');
    var $luongMinValue = $('#luongMinValue');
    var $luongMaxValue = $('#luongMaxValue');
    var $thumbMin = $('#thumbMin');
    var $thumbMax = $('#thumbMax');
    var $line = $('#salaryLine');

    if ($luongMinRange.length === 0 || $luongMaxRange.length === 0) {
        return;
    }

    var minValue = 0;
    var maxValue = 1000000000;
    var currentMin = parseInt($luongMinRange.val()) || 0;
    var currentMax = parseInt($luongMaxRange.val()) || 1000000000;

    // Calculate left position percentage
    var calcLeftPosition = function (value) {
        return (100 / (maxValue - minValue)) * (value - minValue);
    };

    // Update UI
    var updateUI = function () {
        var minPercent = calcLeftPosition(currentMin);
        var maxPercent = calcLeftPosition(currentMax);

        $thumbMin.css('left', minPercent + '%');
        $thumbMax.css('left', maxPercent + '%');
        $luongMinValue.text(currentMin);
        $luongMaxValue.text(currentMax);

        $line.css({
            'left': minPercent + '%',
            'right': (100 - maxPercent) + '%'
        });
    };

    // Initialize UI
    updateUI();

    // Min range input handler
    $luongMinRange.on('input', function (e) {
        var newValue = parseInt(e.target.value);
        if (newValue > currentMax) return;
        currentMin = newValue;
        updateUI();
    });

    // Max range input handler
    $luongMaxRange.on('input', function (e) {
        var newValue = parseInt(e.target.value);
        if (newValue < currentMin) return;
        currentMax = newValue;
        updateUI();
    });
}

/**
 * Initialize sort salary radio buttons
 */
function initSortSalaryRadio() {
    var $sapXepRadios = $('input[name="sapXepTheo"]');
    var $sapXepLuongToiDaInput = $('#sapXepLuongToiDaInput');

    if ($sapXepRadios.length === 0 || $sapXepLuongToiDaInput.length === 0) {
        return;
    }

    // Update hidden input when radio changes
    $sapXepRadios.on('change', function () {
        var selectedValue = $(this).val();

        if (selectedValue === 'asc' || selectedValue === 'desc') {
            // Nếu chọn tăng dần hoặc giảm dần, set sapXepLuongToiDa = true
            $sapXepLuongToiDaInput.val('true');
        } else {
            // Nếu chọn không sắp xếp, set sapXepLuongToiDa = false hoặc empty
            $sapXepLuongToiDaInput.val('');
        }
    });

    // Set initial value on page load
    var checkedRadio = $sapXepRadios.filter(':checked');
    if (checkedRadio.length > 0) {
        var initialValue = checkedRadio.val();
        if (initialValue === 'asc' || initialValue === 'desc') {
            $sapXepLuongToiDaInput.val('true');
        } else {
            $sapXepLuongToiDaInput.val('');
        }
    }
}

// ========================================
// CATEGORY FILTER FOR INDEX PAGE
// ========================================

/**
 * Initialize Select2 for linh vuc filter on index page
 */
function initLinhVucFilter() {
    var $filter = $('#LinhVucFilter');

    // Only init if filter exists on page
    if ($filter.length === 0) {
        return;
    }

    // Get initial value from URL if exists
    var initialVal = new URLSearchParams(window.location.search).get('linhVuc');

    // Load initial value first if exists
    if (initialVal) {
        $.ajax({
            url: '/doanh-nghiep/linh-vuc/danh-sach',
            type: 'GET',
            dataType: 'json',
            data: { pageSize: 100 },
            success: function (data) {
                var items = data.records || [];
                var selectedItem = items.find(function (item) {
                    return item.slug == initialVal;
                });
                if (selectedItem) {
                    // Add option before initializing Select2
                    var option = new Option(selectedItem.ten, selectedItem.id, true, true);
                    $filter.append(option);
                }
                // Initialize Select2 after adding the option
                initSelect2ForLinhVucFilter($filter);
            },
            error: function () {
                // Initialize Select2 even if loading fails
                initSelect2ForLinhVucFilter($filter);
            }
        });
    } else {
        // Initialize Select2 directly if no initial value
        initSelect2ForLinhVucFilter($filter);
    }
}

/**
 * Initialize Select2 configuration for LinhVucFilter
 */
function initSelect2ForLinhVucFilter($filter) {
    $filter.select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Tất cả lĩnh vực',
        allowClear: true,
        ajax: {
            url: '/doanh-nghiep/linh-vuc/danh-sach',
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
                var items = data.records || [];

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
                        more: (params.page * 50) < data.totalRecords
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
// CKEDITOR 4 INITIALIZATION
// ========================================

/**
 * Initialize CKEditor 4 with image upload for all textareas
 */
function initCKEditor() {
    var editorIds = ['MoTa', 'YeuCau', 'QuyenLoi', 'UuTien'];

    editorIds.forEach(function (editorId) {
        var $editor = $('#' + editorId);

        // Only init if editor exists on page
        if ($editor.length === 0) {
            return;
        }

        // Destroy existing instance if any
        if (ckeditorInstances[editorId]) {
            ckeditorInstances[editorId].destroy();
            ckeditorInstances[editorId] = null;
        }

        ckeditorInstances[editorId] = CKEDITOR.replace(editorId, {
            height: 350,
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
            filebrowserImageUploadUrl: '/doanh-nghiep/viec-lam/upload-image',
            imageUploadUrl: '/doanh-nghiep/viec-lam/upload-image',
            uploadUrl: '/doanh-nghiep/viec-lam/upload-image',
            removePlugins: 'easyimage',
            image_previewText: ' ',
            removeDialogTabs: 'image:advanced;link:advanced'
        });

        // Handle upload success
        ckeditorInstances[editorId].on('fileUploadResponse', function (evt) {
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
        ckeditorInstances[editorId].on('fileUploadRequest', function (evt) {
            var fileLoader = evt.data.fileLoader;
            var formData = new FormData();
            var xhr = fileLoader.xhr;

            xhr.open('POST', fileLoader.uploadUrl, true);
            formData.append('upload', fileLoader.file, fileLoader.fileName);
            fileLoader.xhr.send(formData);

            // Prevent default request
            evt.stop();
        });
    });
}

// ========================================
// DELETE FUNCTION
// ========================================

/**
 * Delete viec lam with confirmation
 * @param {number} id - Viec lam ID
 * @param {string} title - Viec lam title
 */
function DeleteViecLam(id, title) {
    Swal.fire({
        title: 'Xác nhận xóa',
        text: `Bạn có chắc chắn muốn xóa việc làm "${title}"?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/doanh-nghiep/viec-lam/xoa/${id}`,
                type: 'DELETE',
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: response.message || 'Xóa việc làm thành công',
                            icon: 'success',
                            confirmButtonText: 'OK'
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire({
                            title: 'Thất bại!',
                            text: response.message || 'Xóa việc làm thất bại',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                    Swal.fire({
                        title: 'Lỗi!',
                        text: 'Có lỗi xảy ra khi xóa việc làm',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });
        }
    });
}

// ========================================
// CLEANUP ON PAGE UNLOAD
// ========================================

$(window).on('beforeunload', function () {
    // Destroy all CKEditor instances
    for (var key in ckeditorInstances) {
        if (ckeditorInstances[key]) {
            ckeditorInstances[key].destroy();
        }
    }
});
