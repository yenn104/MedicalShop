document.addEventListener("DOMContentLoaded", function () {
  $('#checkAll').on('change', function () {
    var checkboxId = 'checkAll';
    var checkboxClass = '.checkHangHoa';
    handleCheckAllChange(checkboxId, checkboxClass);
  })

  $(document).on('click', '#tbodyCanhBaoTon .checkHangHoa', function () {
    updateColumnCheckboxes();
  });

});


function handleCheckAllChange(checkboxId, checkboxClass) {
  var isChecked = $('#' + checkboxId).prop('checked');
  $(checkboxClass).prop('checked', isChecked);
}


function updateColumnCheckboxes() {
  if ($('.checkHangHoa').length == $('.checkHangHoa:checked').length) {
    $('#checkAll').prop('checked', true);
  } else {
    $('#checkAll').prop('checked', false);
  }
}



function chuyenDenNhapKho() {
  var listHH = [];
  var checkedRows = $('#tbodyCanhBaoTon tr input.checkHangHoa:checked');
  if (checkedRows.length > 0) {
    checkedRows.each(function () {
      var idHangHoa = $(this).closest('tr').data('id-hanghoa');
      var idDvt = $(this).closest('tr').data('id-donvitinh');
      var tenHangHoa = $(this).closest('tr').find('td.tenHangHoa span').html();
      var tenDvt = $(this).closest('tr').find('td.tenDvt span').html();
      if (!listHH.some(item => item.idHangHoa === idHangHoa)) {
        listHH.push({ idHangHoa: idHangHoa, tenHangHoa: tenHangHoa, idDvt: idDvt, tenDvt: tenDvt });
      }
    })

    console.log(listHH);


    var newWindow = window.open("/PhieuNhapKho", "_blank");
    newWindow.onload = function () {
      newWindow.loadDaTaFromTableCanhBaoTonKho(
        listHH
      );
    };


  }
}