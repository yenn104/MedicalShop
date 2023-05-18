
$(document).ready(function () {

  $('#example1').DataTable({
    lengthChange: false,
    pageLength: 5,
    info: false,
    language: {
      emptyTable: "Không có dữ liệu.",
      paginate: {
        previous: 'Trước',
        next: 'Sau'
      }
    }
  });

  // Ẩn ô tìm kiếm
  $('.dataTables_filter').hide();


  var dataTableSearch = $('#example1_filter').find('input');


  // Lấy đối tượng ô tìm kiếm của bạn
  var mySearch = $('#timkiem');

  // Áp dụng phương thức hoạt động của ô tìm kiếm của DataTable vào ô tìm kiếm của bạn
  mySearch.on('keyup', function () {
    dataTableSearch.val($(this).val()).keyup();
  });


});
