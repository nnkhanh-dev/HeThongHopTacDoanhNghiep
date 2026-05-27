function initDonUngTuyenApply() {
    var $form = $('#applyDonUngTuyenForm');
    var $modal = $('#applyDonUngTuyenModal');

    if ($form.length === 0 || $modal.length === 0) {
        return;
    }

    $form.on('submit', function (e) {
        e.preventDefault();

        var form = this;
        var formData = new FormData(form);
        var token = $(form).find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: $(form).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            headers: {
                'RequestVerificationToken': token
            },
            success: function (response) {
                if (response && response.success) {
                    Swal.fire({
                        title: 'Thành công',
                        text: response.message || 'Ứng tuyển thành công',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        var modalInstance = bootstrap.Modal.getInstance($modal[0]);
                        if (modalInstance) {
                            modalInstance.hide();
                        }
                        form.reset();
                        window.location.href = '/sinh-vien/don-ung-tuyen';
                    });
                    return;
                }

                Swal.fire({
                    title: 'Không thành công',
                    text: (response && response.message) ? response.message : 'Ứng tuyển thất bại',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            },
            error: function (xhr) {
                var message = 'Có lỗi xảy ra khi ứng tuyển';

                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }

                Swal.fire({
                    title: 'Lỗi',
                    text: message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

    $modal.on('hidden.bs.modal', function () {
        $form[0].reset();
    });
}

$(document).ready(function () {
    initDonUngTuyenApply();
});
