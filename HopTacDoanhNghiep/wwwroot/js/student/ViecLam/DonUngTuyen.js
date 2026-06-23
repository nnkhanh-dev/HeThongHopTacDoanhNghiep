// DonUngTuyen.js - handles withdraw application confirmation
$(document).ready(function () {
    // Select the withdraw form by its action prefix
    $('form[action^="/sinh-vien/don-ung-tuyen/rut-ho-so/"]').on('submit', function (e) {
        e.preventDefault();
        var form = this;
        Swal.fire({
            title: 'Bạn có chắc muốn rút hồ sơ?',
            text: 'Hành động này không thể hoàn tác.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Có, rút hồ sơ',
            cancelButtonText: 'Hủy'
        }).then(function (result) {
            if (result.isConfirmed) {
                $.ajax({
                    url: $(form).attr('action'),
                    method: $(form).attr('method'),
                    data: $(form).serialize(),
                    success: function (resp) {
                        if (resp.success) {
                            Swal.fire('Thành công', resp.message, 'success').then(function () {
                                // Reload page to reflect new status
                                location.reload();
                            });
                        } else {
                            Swal.fire('Lỗi', resp.message, 'error');
                        }
                    },
                    error: function () {
                        Swal.fire('Lỗi', 'Có lỗi xảy ra khi rút hồ sơ.', 'error');
                    }
                });
            }
        });
    });
});
