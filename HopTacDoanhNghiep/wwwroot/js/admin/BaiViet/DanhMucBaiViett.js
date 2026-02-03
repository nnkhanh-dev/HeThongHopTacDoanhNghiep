function DeleteDanhMucBaiViet(id) {
    Swal.fire({
        title: 'Xác nhận xóa?',
        text: 'Danh mục bài viết sẽ bị xóa vĩnh viễn.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteDanhMuc(id);
        }
    });
}

function deleteDanhMuc(id) {
    $.ajax({
        url: '/admin/danh-muc-bai-viet/xoa/' + id,
        type: 'DELETE',
        success: function (res) {
            if (res.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: res.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    window.location.reload();
                });
            } else {
                Swal.fire('Không thể xóa', res.message, 'error');
            }
        },
        error: function () {
            Swal.fire('Lỗi', 'Có lỗi xảy ra, vui lòng thử lại.', 'error');
        }
    });
}