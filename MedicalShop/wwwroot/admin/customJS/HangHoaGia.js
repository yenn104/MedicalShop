//var _idCN = null;
var _isCheck = 0;
$(document).ready(function () {

  $("#btn-save").click(function () {
    updatePrices();
  });

 // ok
  //$(".gia, .tile").each(function () {
  //  var gia = $(this).val();

  //  if (gia != 0) {
  //    $(this).val(toDecimal(getValue(gia)));
  //  }
  //});
  formatNumberFloat();

  //$(document).on(click, '.gia, .tile', function () {
  //  var id = $(this).closest('tr').find('input.idvt').val();
  //  console.log(id);
  //  //loadDSNhap(id);
  //});

  $("input.gia, input.tile").on('click', function () {
    var id = $(this).closest('tr').find('input.idvt').val();
    loadDSNhap(id);
  });


  $('#switch-1').on('change', function () {
    if ($(this).is(':checked')) {
      _isCheck = 1;
    } else {
      _isCheck = 0;
    }
    loadTableLT();
  });

  var lastClickedTr;

  $("input").click(function (event) {
    event.stopPropagation();
    var tr = $(this).closest("tr");
    if (lastClickedTr) {
      lastClickedTr.removeClass("color-clicked");
    }
    tr.addClass("color-clicked");
    lastClickedTr = tr;
  });

  $("#giaban tr").click(function () {
    if (lastClickedTr && lastClickedTr != $(this)) {
      lastClickedTr.removeClass("color-clicked");
    }
    $(this).addClass("color-clicked");
    lastClickedTr = $(this);
  });
  //formatFloat();
  //formatFloatInput();


  $("#btn-save-ctg").click(function () {
    var id = $('#idCTG').val();
    var isMax = $('#Max').prop('checked');
    var isMedium = $('#Medium').prop('checked');
    var isMin = $('#Min').prop('checked');
    var model = {
      Id : Number(id),
      Max : isMax,
      Medium : isMedium,
      Min: isMin,
      IdCN: _idCN
    }
    console.log(model);

    $.ajax({
      type: "POST",
      url: "/updateCachTinhGia",
      dataType: 'JSON',
      contentType: 'application/json',
      data: JSON.stringify(model),
      success: function (result) {
        showToast(result.message, result.statusCode);
      },
      error: function () {
        console.log(error);
      }
    });



  });
  
  //_idCN = $('#cbChiNhanhHHLT')[0].selectize.getValue();



});

$('#cbChiNhanhHHLT').on('change', function () {
  _idCN = $(this).val();
  loadTableLT();
  console.log(_idCN);
})


function formatNumberFloat() {
  $(".gia, .tile").each(function () {
    var gia = $(this).val();

    if (gia != 0) {
      $(this).val(toDecimal(getValue(gia)));
    }
  });
}



function loadTableLT() {
  $.ajax({
    type: "POST",
    url: "/loadGiaLT",
    data: "check=" + _isCheck + "&idCN=" + _idCN,
    success: function (result) {
      console.log(result);
      $('#giaban').replaceWith(result);
      formatNumberFloat();
      //formatFloat();
      //formatFloatInput();
    },
    error: function () {
      console.log(error);
    }
  });
}


//ok
function updatePrices() {
  var listOfPriceModel = new Array();
  $("#table2 tr.hasChange").each(function () {
    var idhh = $(this).find("td:eq(0) input[type='hidden']").val();
    //var idvt = $(this).find("td:eq(1) input[type='hidden']").val();
    var tile = getValue($(this).find("td:eq(1) input[type='text'] ").val());
    var gia = getValue($(this).find("td:eq(2) input[type='text'] ").val());
    console.log(tile, gia);
    if (tile == 0 && gia == 0) {
      tile = null;
      gia = null;
    }
    //alert(gia);
    var PriceModel = {};
    PriceModel.Idhh = Number(idhh);
    PriceModel.TiLe = tile;
    PriceModel.Price = gia;
    PriceModel.Idcn = Number(_idCN);
    listOfPriceModel.push(PriceModel);
  });
  console.log(listOfPriceModel);

  $.ajax({
    url: '/updatePrices',
    type: 'POST',
    dataType: 'JSON',
    contentType: 'application/json',
    data: JSON.stringify(listOfPriceModel),
    success: function (result) {
      showToast(result.message, result.statusCode);
      if (result.statusCode == 200) {
      }
    },
    error: function () {
      console.log(error);
    }
  });
}



//ok



//ok
function loadDSNhap(id) {
  if (id > 0) {
    $('#giaban').val(id);
    $.ajax({
      type: "post",
      url: "/loadDSNhap", // loadTableCN
      data: "idhh=" + id,
      success: function (result) {
        $('#gianhap').replaceWith(result);
        $('#table3').removeClass('d-none');
        updateDouble();
      },
      error: function () {
        console.log(error);
      }
    });
  }
}

//ok
function updateDouble() {
  $("#table3").find("tr:gt(0)").each(function () {
    var gia = $(this).find("td:eq(2)").text();
    /* $(this).text(toDecimal(gia));*/
    $(this).find("td:eq(2)").text(toDecimal(gia));
  });

}



//$("#table2").on("change", ".gia, .tile", function () {
//  var gia = $(this).val();
//  if (gia != 0) {
//    /* $(this).text(toDecimal(gia));*/
//    $(this).val(toDecimal(gia));
//  }
//});





//$('.tile').on('blur', setval);

//function setval() {

//}

//ok

$(".form-control.gia").on("keydown keyup", function (e) {
  var inputValue = $(this).val();
  inputValue = inputValue.replace(/[^0-9\.]/g, '');
  $(this).val(inputValue);
});


//ok

$(".form-control.tile").on("keydown keyup", function (e) {
  var inputValue = $(this).val();
  // Loại bỏ các ký tự không phải số hoặc nằm ngoài khoảng 0-9
  inputValue = inputValue.replace(/[^0-9\.]/g, '');
  // Giới hạn giá trị nhập vào trong khoảng từ 0-100
  if (parseInt(inputValue) > 100) {
    inputValue = 100;
  } else if (parseInt(inputValue) < 0) {
    inputValue = 0;
  }
  // Cập nhật giá trị của input
  $(this).val(inputValue);
});




$("#table2").on("change", ".gia, .tile", function () {
  var val1 = $(this).val();
  if ($(this).hasClass("gia") && val1 != 0) {
    $(this).val(toDecimal(val1));
  } else if ($(this).hasClass("tile") && val1 != 0) {
    $(this).val(toDecimal(val1));
  }
  $(this).closest('tr').addClass('hasChange');
});




//$("#table2").on("load", ".gia", function () {
//  var idvt = $(this).parents('tr').find("td:eq(0) input[type='hidden']").val();
//  var tenvt = $(this).parents('tr').find("td:eq(0) input[type='text']").val();

//});




$('.form-control').on('change', select1in2);

function select1in2() {
  var val1 = $(this).val();
  if (val1 >= 0) {
    var parentTr = $(this).closest('tr');
    parentTr.find('.form-control').not(this).val('');
  }

}


      //$('.giadouble').on('blur', format);

      //function format() {
      //  if (checkNumber(this.value)) {
      //    const value = this.value.replace(/,/g, '');
      //    this.value = parseFloat(value).toLocaleString('en-US', {
      //      style: 'decimal',
      //      maximumFractionDigits: 2,
      //      minimumFractionDigits: 2
      //    })
      //  }
      //}
