document.addEventListener('click', function (e) {
	var btn = e.target.closest && e.target.closest('.btn-action-status');
	if (!btn) return;

	e.preventDefault();

	var url = btn.getAttribute('data-url');
	var trangThai = btn.getAttribute('data-status');
	var actionText = btn.getAttribute('data-action-text') || 'cập nhật trạng thái';

	Swal.fire({
		title: 'Xác nhận',
		text: 'Bạn có chắc muốn ' + actionText + ' không?',
		icon: 'question',
		showCancelButton: true,
		confirmButtonText: 'Xác nhận',
		cancelButtonText: 'Hủy'
	}).then(function (result) {
		if (!result.isConfirmed) return;

		$.ajax({
			url: url,
			type: 'POST',
			data: { trangThai: trangThai },
			success: function (data) {
				if (data && data.success) {
					Swal.fire({
						icon: 'success',
						title: 'Thành công',
						text: data.message
					}).then(function () {
						window.location.reload();
					});
					return;
				}

				Swal.fire({
					icon: 'error',
					title: 'Lỗi',
					text: (data && data.message) || 'Cập nhật trạng thái thất bại'
				});
			},
			error: function () {
				Swal.fire({
					icon: 'error',
					title: 'Lỗi',
					text: 'Không thể kết nối tới máy chủ'
				});
			}
		});
	});
});