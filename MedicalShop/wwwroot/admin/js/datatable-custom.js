
  $(document).ready(function () {

    $('#table2').DataTable({
      /*searching: false,*/
      lengthChange: false,
      pageLength: false,
      paginate: false,
      info: false,
      language: {
        emptyTable: "Không có dữ liệu.",
        zeroRecords: "Không tìm thấy kết quả phù hợp"
      }
    });

    $('#table3').DataTable({
      /*searching: false,*/
      lengthChange: false,
      pageLength: false,
      paginate: false,
      info: false,
      language: {
        emptyTable: "Không có dữ liệu.",
        zeroRecords: "Không tìm thấy kết quả phù hợp"
      }
    });


    $('#example').DataTable({
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


    var dataTableSearch = $('#example_filter').find('input');

    var dataTableSearch1 = $('#table2_filter').find('input');

    var dataTableSearch2 = $('#table3_filter').find('input');

    // Lấy đối tượng ô tìm kiếm của bạn
    var mySearch = $('#timkiem');

    // Áp dụng phương thức hoạt động của ô tìm kiếm của DataTable vào ô tìm kiếm của bạn
    mySearch.on('keyup', function () {
      dataTableSearch1.val($(this).val()).keyup();
      dataTableSearch.val($(this).val()).keyup();
      dataTableSearch2.val($(this).val()).keyup();
    });


 });
