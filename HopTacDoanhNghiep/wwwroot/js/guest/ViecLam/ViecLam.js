// Document ready
$(document).ready(function () {
    initLinhVucFilters();
    initOtherSelectFilters();
    initSalaryRangeSliders();
    initSortSalaryRadio();
});

// ========================================
// SELECT2 FOR LINH VUC FILTER
// ========================================

/**
 * Initialize Select2 for linh vuc filter dropdowns (both desktop and modal) with AJAX
 */
function initLinhVucFilters() {
    var $selectDesktop = $('#LinhVucFilterDesktop');
    var $selectModal = $('#LinhVucFilterModal');
    
    // Get current filter value from URL
    var urlParams = new URLSearchParams(window.location.search);
    var currentLinhVucSlug = urlParams.get('linhVucSlug');

    var select2Config = {
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: 'Tất cả lĩnh vực',
        allowClear: false,
        ajax: {
            url: '/danh-sach-linh-vuc',
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
                var items = data.records || [];
                
                // Always add "Tất cả lĩnh vực" option at the beginning
                if (params.page === 1 && !params.term) {
                    results.push({
                        id: '',
                        text: 'Tất cả lĩnh vực'
                    });
                }
                
                if (items.length > 0) {
                    var itemResults = items.map(function (item) {
                        return {
                            id: item.slug,
                            text: item.ten
                        };
                    });
                    results = results.concat(itemResults);
                }

                return {
                    results: results,
                    pagination: {
                        more: (params.page * 20) < data.totalRecords
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
    };

    // Initialize desktop select2 (if exists)
    if ($selectDesktop.length > 0) {
        $selectDesktop.select2(select2Config);
        
        // Pre-populate if there's a filter value
        if (currentLinhVucSlug) {
            loadAndSetLinhVucOption($selectDesktop, currentLinhVucSlug);
        } else {
            // Set default "Tất cả lĩnh vực" option
            var defaultOption = new Option('Tất cả lĩnh vực', '', true, true);
            $selectDesktop.append(defaultOption).trigger('change');
        }
    }

    // Initialize modal select2 (if exists)
    if ($selectModal.length > 0) {
        var modalConfig = $.extend({}, select2Config, {
            dropdownParent: $('#filterModal')
        });
        $selectModal.select2(modalConfig);
        
        // Pre-populate if there's a filter value
        if (currentLinhVucSlug) {
            loadAndSetLinhVucOption($selectModal, currentLinhVucSlug);
        } else {
            // Set default "Tất cả lĩnh vực" option
            var defaultOption = new Option('Tất cả lĩnh vực', '', true, true);
            $selectModal.append(defaultOption).trigger('change');
        }
    }
}

/**
 * Load and set the current linh vuc option for a select
 */
function loadAndSetLinhVucOption($select, slug) {
    $.ajax({
        url: '/danh-sach-linh-vuc',
        type: 'GET',
        dataType: 'json',
        data: { pageSize: 100 },
        success: function (data) {
            if (data && data.records) {
                var currentItem = data.records.find(function(item) {
                    return item.slug === slug;
                });
                
                if (currentItem) {
                    var option = new Option(currentItem.ten, currentItem.slug, true, true);
                    $select.append(option).trigger('change');
                }
            }
        }
    });
}

// ========================================
// SELECT2 FOR OTHER FILTERS
// ========================================

/**
 * Initialize Select2 for other filter dropdowns (loaiViecLam, doiTuong, trinhDo)
 */
function initOtherSelectFilters() {
    // Desktop filters
    $('#LoaiViecLamFilterDesktop').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity // Disable search for simple dropdowns
    });

    $('#DoiTuongFilterDesktop').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity
    });

    $('#TrinhDoFilterDesktop').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity
    });

    // Modal filters
    $('#LoaiViecLamFilterModal').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity,
        dropdownParent: $('#filterModal')
    });

    $('#DoiTuongFilterModal').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity,
        dropdownParent: $('#filterModal')
    });

    $('#TrinhDoFilterModal').select2({
        theme: 'bootstrap-5',
        width: '100%',
        minimumResultsForSearch: Infinity,
        dropdownParent: $('#filterModal')
    });
}

// ========================================
// SALARY RANGE SLIDERS
// ========================================

/**
 * Initialize salary range sliders for both desktop and modal
 */
function initSalaryRangeSliders() {
    // Initialize desktop slider
    initSingleSalarySlider('Desktop');
    
    // Initialize modal slider
    initSingleSalarySlider('Modal');
}

/**
 * Initialize a single salary slider
 * @param {string} suffix - 'Desktop' or 'Modal'
 */
function initSingleSalarySlider(suffix) {
    var $luongMinRange = $('#luongMinRange' + suffix);
    var $luongMaxRange = $('#luongMaxRange' + suffix);
    var $luongMinValue = $('#luongMinValue' + suffix);
    var $luongMaxValue = $('#luongMaxValue' + suffix);
    var $thumbMin = $('#thumbMin' + suffix);
    var $thumbMax = $('#thumbMax' + suffix);
    var $line = $('#salaryLine' + suffix);

    if ($luongMinRange.length === 0 || $luongMaxRange.length === 0) {
        return;
    }

    var minValue = 0;
    var maxValue = 1000000000;
    var currentMin = parseInt($luongMinRange.val()) || 0;
    var currentMax = parseInt($luongMaxRange.val()) || 1000000000;

    // Format number with thousands separator (Vietnamese format)
    var formatNumber = function(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    };

    // Calculate left position percentage
    var calcLeftPosition = function(value) {
        return (100 / (maxValue - minValue)) * (value - minValue);
    };

    // Update UI
    var updateUI = function() {
        var minPercent = calcLeftPosition(currentMin);
        var maxPercent = calcLeftPosition(currentMax);

        $thumbMin.css('left', minPercent + '%');
        $thumbMax.css('left', maxPercent + '%');
        $luongMinValue.text(formatNumber(currentMin) + ' VNĐ');
        $luongMaxValue.text(formatNumber(currentMax) + ' VNĐ');
        
        $line.css({
            'left': minPercent + '%',
            'right': (100 - maxPercent) + '%'
        });
    };

    // Initialize UI
    updateUI();

    // Min range input handler
    $luongMinRange.on('input', function(e) {
        var newValue = parseInt(e.target.value);
        if (newValue > currentMax) return;
        currentMin = newValue;
        updateUI();
    });

    // Max range input handler
    $luongMaxRange.on('input', function(e) {
        var newValue = parseInt(e.target.value);
        if (newValue < currentMin) return;
        currentMax = newValue;
        updateUI();
    });
}

// ========================================
// SORT SALARY RADIO
// ========================================

/**
 * Initialize sort salary radio buttons for both desktop and modal
 */
function initSortSalaryRadio() {
    // Initialize desktop radios
    initSingleSortSalaryRadio('Desktop');
    
    // Initialize modal radios
    initSingleSortSalaryRadio('Modal');
}

/**
 * Initialize sort salary radio buttons for a single form
 * @param {string} suffix - 'Desktop' or 'Modal'
 */
function initSingleSortSalaryRadio(suffix) {
    var $sapXepRadios = $('input[name="sapXepTheo"]').filter(function() {
        return $(this).attr('id').endsWith(suffix);
    });
    var $sapXepLuongToiDaInput = $('#sapXepLuongToiDaInput' + suffix);

    if ($sapXepRadios.length === 0 || $sapXepLuongToiDaInput.length === 0) {
        return;
    }

    // Update hidden input when radio changes
    $sapXepRadios.on('change', function() {
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
