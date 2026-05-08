// ========================================
// DELETE OPERATION
// ========================================

/**
 * Delete nganh with confirmation
 */
function DeleteNganh(id, tenNganh) {
    if (!id) {
        showError('ID không hợp lệ');
        return;
    }

    var confirmMessage = 'Bạn có chắc chắn muốn xóa ngành "' + escapeHtml(tenNganh) + '"?';

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
        url: '/admin/nganh/xoa/' + id,
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
                showError(response.message || 'Không thể xóa ngành');
            }
        },
        error: function (xhr, status, error) {
            console.error('Delete error:', error);
            showError('Có lỗi xảy ra khi xóa ngành');
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
