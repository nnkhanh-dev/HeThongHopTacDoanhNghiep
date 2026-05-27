$(function () {
	const $donViSelect = $('#SelectedDonViIds');

	if (!$donViSelect.length || !$.fn.select2) {
		return;
	}

	$donViSelect.select2({
		theme: 'bootstrap-5',
		width: '100%',
		placeholder: $donViSelect.data('placeholder') || 'Chọn đơn vị nhận hợp tác',
		closeOnSelect: false,
		ajax: {
			url: '/don-vi-nhan-hop-tac',
			dataType: 'json',
			delay: 250,
			data: function (params) {
				return {
					q: params.term || ''
				};
			},
			processResults: function (data) {
				return {
					results: data.results || []
				};
			},
			cache: true
		}
	});
});
