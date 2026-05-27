// ========================================
// MULTI-STEP FORM HANDLER
// ========================================

$(document).ready(function () {
    initMultiStepForm();
    checkAndShowStepWithErrors();
});

var currentStep = 1;
var totalSteps = 2;

/**
 * Check if any step has validation errors and show that step
 */
function checkAndShowStepWithErrors() {
    var foundError = false;
    
    // Check each step for validation errors (from first to last)
    for (var i = 1; i <= totalSteps; i++) {
        var $step = $('.form-step[data-step="' + i + '"]');
        
        // Only check for actual error messages with content
        var errorMessages = $step.find('.text-danger').filter(function() {
            return $(this).text().trim().length > 0;
        });
        
        var hasErrors = $step.find('.field-validation-error').length > 0 ||
                       errorMessages.length > 0 ||
                       $step.find('.is-invalid').length > 0;
        
        if (hasErrors) {
            goToStep(i);
            foundError = true;
            break;
        }
    }
    
    // If no errors found, ensure we're on step 1
    if (!foundError) {
        goToStep(1);
    }
}

/**
 * Initialize multi-step form functionality
 */
function initMultiStepForm() {
    // Next step button
    $('.next-step').on('click', function () {
        if (validateCurrentStep()) {
            goToStep(currentStep + 1);
        }
    });

    // Previous step button
    $('.prev-step').on('click', function () {
        goToStep(currentStep - 1);
    });

    // Form submission validation
    $('#multiStepForm').on('submit', function (e) {
        // Update CKEditor data before validation
        updateCKEditorData();

        // Validate all steps before submitting
        if (!validateAllSteps()) {
            e.preventDefault();
            
            // Find first step with errors and go to it
            for (var i = 1; i <= totalSteps; i++) {
                if (!validateStep(i)) {
                    goToStep(i);
                    break;
                }
            }
            return false;
        }
    });
}

/**
 * Navigate to specific step
 * @param {number} stepNumber - Step number to navigate to
 */
function goToStep(stepNumber) {
    if (stepNumber < 1 || stepNumber > totalSteps) {
        return;
    }

    // Hide all steps
    $('.form-step').removeClass('active');
    
    // Show target step
    $('.form-step[data-step="' + stepNumber + '"]').addClass('active');

    // Update current step
    currentStep = stepNumber;

    // Scroll to top
    $('html, body').animate({ scrollTop: 0 }, 300);
}

/**
 * Validate current step
 * @returns {boolean} - True if valid, false otherwise
 */
function validateCurrentStep() {
    return validateStep(currentStep);
}

/**
 * Validate specific step
 * @param {number} stepNumber - Step number to validate
 * @returns {boolean} - True if valid, false otherwise
 */
function validateStep(stepNumber) {
    var $step = $('.form-step[data-step="' + stepNumber + '"]');
    var isValid = true;

    // Clear previous errors only in current step
    $step.find('.text-danger').text('');
    $step.find('.is-invalid').removeClass('is-invalid');

    if (stepNumber === 1) {
        // Validate Step 1: Basic Information
        
        // Tiêu đề
        var tieuDe = $step.find('[name="TieuDe"]').val().trim();
        if (!tieuDe) {
            showFieldError('[name="TieuDe"]', 'Vui lòng nhập tiêu đề', stepNumber);
            isValid = false;
        }

        // Loại việc làm
        var loaiViecLam = $step.find('[name="LoaiViecLam"]').val();
        if (!loaiViecLam && loaiViecLam !== '0') {
            showFieldError('[name="LoaiViecLam"]', 'Vui lòng chọn loại việc làm', stepNumber);
            isValid = false;
        }

        // Đối tượng ứng tuyển
        var doiTuongUngTuyen = $step.find('[name="DoiTuongUngTuyen"]').val();
        if (!doiTuongUngTuyen) {
            showFieldError('[name="DoiTuongUngTuyen"]', 'Vui lòng chọn đối tượng ứng tuyển', stepNumber);
            isValid = false;
        }

        // Trình độ
        var trinhDo = $step.find('[name="TrinhDo"]').val();
        if (!trinhDo) {
            showFieldError('[name="TrinhDo"]', 'Vui lòng chọn trình độ', stepNumber);
            isValid = false;
        }

        // Lương
        var luongToiThieu = parseFloat($step.find('[name="LuongToiThieu"]').val()) || 0;
        var luongToiDa = parseFloat($step.find('[name="LuongToiDa"]').val()) || 0;

        if (!luongToiThieu || luongToiThieu <= 0) {
            showFieldError('[name="LuongToiThieu"]', 'Lương tối thiểu phải lớn hơn 0', stepNumber);
            isValid = false;
        }

        if (!luongToiDa || luongToiDa <= 0) {
            showFieldError('[name="LuongToiDa"]', 'Lương tối đa phải lớn hơn 0', stepNumber);
            isValid = false;
        }

        if (luongToiDa > 0 && luongToiThieu > 0 && luongToiDa < luongToiThieu) {
            showFieldError('[name="LuongToiDa"]', 'Lương tối đa phải lớn hơn hoặc bằng lương tối thiểu', stepNumber);
            isValid = false;
        }

        // Địa điểm
        var diaDiem = $step.find('[name="DiaDiem"]').val().trim();
        if (!diaDiem) {
            showFieldError('[name="DiaDiem"]', 'Vui lòng nhập địa điểm', stepNumber);
            isValid = false;
        }

        // Ngày bắt đầu và hết hạn
        var ngayBatDau = $step.find('[name="NgayBatDau"]').val();
        var ngayHetHan = $step.find('[name="NgayHetHan"]').val();

        if (!ngayBatDau) {
            showFieldError('[name="NgayBatDau"]', 'Vui lòng chọn ngày bắt đầu', stepNumber);
            isValid = false;
        }

        if (!ngayHetHan) {
            showFieldError('[name="NgayHetHan"]', 'Vui lòng chọn ngày hết hạn', stepNumber);
            isValid = false;
        }

        if (ngayBatDau && ngayHetHan) {
            var dateBatDau = new Date(ngayBatDau);
            var dateHetHan = new Date(ngayHetHan);

            if (dateHetHan <= dateBatDau) {
                showFieldError('[name="NgayHetHan"]', 'Ngày hết hạn phải sau ngày bắt đầu', stepNumber);
                isValid = false;
            }
        }

        // Từ khóa
        var tuKhoa = $step.find('[name="TuKhoa"]').val().trim();
        if (!tuKhoa) {
            showFieldError('[name="TuKhoa"]', 'Vui lòng nhập từ khóa', stepNumber);
            isValid = false;
        }
    } 
    else if (stepNumber === 2) {
        // Update CKEditor data first
        updateCKEditorData();

        // Validate Step 2: Job Details
        
        // Mô tả
        var moTa = $step.find('[name="MoTa"]').val().trim();
        if (!moTa || moTa === '<p>&nbsp;</p>' || moTa === '<p></p>') {
            showFieldError('[name="MoTa"]', 'Vui lòng nhập mô tả công việc', stepNumber);
            isValid = false;
        }

        // Yêu cầu
        var yeuCau = $step.find('[name="YeuCau"]').val().trim();
        if (!yeuCau || yeuCau === '<p>&nbsp;</p>' || yeuCau === '<p></p>') {
            showFieldError('[name="YeuCau"]', 'Vui lòng nhập yêu cầu công việc', stepNumber);
            isValid = false;
        }

        // Quyền lợi
        var quyenLoi = $step.find('[name="QuyenLoi"]').val().trim();
        if (!quyenLoi || quyenLoi === '<p>&nbsp;</p>' || quyenLoi === '<p></p>') {
            showFieldError('[name="QuyenLoi"]', 'Vui lòng nhập quyền lợi', stepNumber);
            isValid = false;
        }

        // Ưu tiên
        var uuTien = $step.find('[name="UuTien"]').val().trim();
        if (!uuTien || uuTien === '<p>&nbsp;</p>' || uuTien === '<p></p>') {
            showFieldError('[name="UuTien"]', 'Vui lòng nhập ưu tiên', stepNumber);
            isValid = false;
        }
    }

    return isValid;
}

/**
 * Validate all steps
 * @returns {boolean} - True if all steps are valid
 */
function validateAllSteps() {
    var allValid = true;
    
    for (var i = 1; i <= totalSteps; i++) {
        if (!validateStep(i)) {
            allValid = false;
        }
    }
    
    return allValid;
}

/**
 * Show field error message
 * @param {string} selector - Field selector or error message selector
 * @param {string} message - Error message
 * @param {number} stepNumber - Step number (optional, defaults to current step)
 */
function showFieldError(selector, message, stepNumber) {
    stepNumber = stepNumber || currentStep;
    var $step = $('.form-step[data-step="' + stepNumber + '"]');
    var $field = $step.find(selector);
    
    if ($field.length === 0) {
        // Try global selector as fallback
        $field = $(selector);
    }
    
    if ($field.is('[data-valmsg-for]')) {
        // It's already the error message span
        $field.text(message);
        var fieldName = $field.data('valmsg-for');
        $step.find('[name="' + fieldName + '"]').addClass('is-invalid');
    } else if ($field.is('input, select, textarea')) {
        // It's a form field
        $field.addClass('is-invalid');
        $field.siblings('.text-danger').text(message);
        var nextError = $field.next('[data-valmsg-for]');
        if (nextError.length) {
            nextError.text(message);
        }
    }
}

/**
 * Update CKEditor textareas with current content
 */
function updateCKEditorData() {
    if (typeof ckeditorInstances !== 'undefined') {
        for (var key in ckeditorInstances) {
            if (ckeditorInstances[key]) {
                ckeditorInstances[key].updateElement();
            }
        }
    }
}

