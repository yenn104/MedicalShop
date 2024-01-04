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



var groupColumn = 0;
var table = $('#table5').DataTable({
  columnDefs: [{ visible: false, targets: groupColumn }],
  lengthChange: false,
  pageLength: false,
  paginate: false,
  info: false,
  order: [],
  //ordering: false,
  language: {
    emptyTable: "Không có dữ liệu.",
    zeroRecords: "Không tìm thấy kết quả phù hợp"
  },
  drawCallback: function (settings) {
    var api = this.api();
    var rows = api.rows({ page: 'current' }).nodes();
    var last = null;

    api.column(groupColumn, { page: 'current' })
      .data()
      .each(function (group, i) {
        if (last !== group) {
          $(rows)
            .eq(i)
            .before( '<tr class="group font-weight-bold"><td colspan="7" class="text-primary">' +  group + '</td></tr>' );
          last = group;
        }
      });
  }
});

// Order by the grouping
$('#table5 tbody').on('click', 'tr.group', function () {
  var currentOrder = table.order()[0];
  if (currentOrder[0] === groupColumn && currentOrder[1] === 'asc') {
    table.order([groupColumn, 'desc']).draw();
  }
  else {
    table.order([groupColumn, 'asc']).draw();
  }
});