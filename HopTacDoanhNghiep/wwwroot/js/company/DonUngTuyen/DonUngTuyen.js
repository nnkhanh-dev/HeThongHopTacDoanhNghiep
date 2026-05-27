function getAntiForgeryToken() {
	var el = document.querySelector('input[name="__RequestVerificationToken"]');
	return el ? el.value : null;
}

document.addEventListener('click', function (e) {
	var btn = e.target.closest && e.target.closest('.btn-change-status');
	if (!btn) return;

	var maUT = btn.getAttribute('data-maut');
	var trangThai = btn.getAttribute('data-trangthai');
	var actionText = btn.classList.contains('btn-success') ? 'chấp nhận' : 'từ chối';

	Swal.fire({
		title: 'Xác nhận',
		text: 'Bạn có chắc muốn ' + actionText + ' đơn ứng tuyển này?',
		icon: 'question',
		showCancelButton: true,
		confirmButtonText: 'Có',
		cancelButtonText: 'Hủy'
	}).then(function (result) {
		if (!result.isConfirmed) return;

		var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
		var token = getAntiForgeryToken();
		if (token) headers['RequestVerificationToken'] = token;

		fetch('/doanh-nghiep/don-ung-tuyen/thay-doi-trang-thai', {
			method: 'POST',
			headers: headers,
			body: new URLSearchParams({ maUT: maUT, trangThai: trangThai })
		})
		.then(function (r) { return r.json(); })
		.then(function (data) {
			if (data && data.success) {
				Swal.fire({ icon: 'success', title: 'Thành công', text: data.message }).then(function () { location.reload(); });
			} else {
				Swal.fire({ icon: 'error', title: 'Lỗi', text: (data && data.message) || 'Cập nhật thất bại' });
			}
		})
		.catch(function () {
			Swal.fire({ icon: 'error', title: 'Lỗi', text: 'Không thể kết nối tới server' });
		});
	});
});

