var hangHoaXuat = [];
var hangHoaTon = [];
var _isEdit = null;
var _idHHDangSua = null;


document.addEventListener("DOMContentLoaded", function () {

  //document.getElementById("temp_ct").remove();
  $('input').on('focus', function (event) {
    this.select();
  });


  $("#ThemChiTietPX").click(function () {
    addChiTietPhieuTam();
    updateTable2();
  });


  $("#btn-create").click(function () {
    CreatePX();
  });



  $('#ChietKhau').on('input', function () {
    if (getValue(this.value) > 100) {
      $(this).addClass('is-invalid');
      $(this).val('');
      showToast('Chiết khấu không được quá 100%', 500);
    }
    if (getValue(this.value) > 100) {
    } else {
      $(this).removeClass('is-invalid');
    }
  });


  $('.number-input').bind("cut copy paste drag drop", function (e) {
    e.preventDefault();
  });


});



$(document).ready(function () {
  $('#selectHHX').selectize({
    render: {
      option: function (item, escape) {
        return '<div class="d-flex p-2 "><span style="width: 80%;">' + escape(item.text) + '</span><span style="width: 20%; white-space: nowrap; overflow: hidden;text-overflow: ellipsis;text-align: end;" class="ms-auto text-muted">[' + escape(item.data) + ']</span></div>';
      },
      no_results: function (data, escape) {
        return '<div class="no-results">Không tìm thấy dữ liệu </div>';
      },
    }
  });
});



function deleteRow(index) {
  var row = document.getElementById(index);
  row.parentNode.removeChild(row);
  updateTable2();

  //hàm cập nhật số lượng

}


// ok
$(function () {
  $("#tableChiTietPhieuXuat").on("click", ".edit", function () {

    var index = $(this).closest('tr').attr('id');
    var idHH = $(this).parents('tr').find("td:eq(0) input[type='hidden']").val();
    var dvt = $(this).parents('tr').find("td:eq(1)").text();
    var SL = $(this).parents('tr').find("td:eq(2)").html();
    var DonGia = $(this).parents('tr').find("td:eq(3)").html();
    var ThanhTien = $(this).parents('tr').find("td:eq(4)").html();
    var ChietKhau = $(this).parents('tr').find("td:eq(5)").html();
    var ThueXuat = Number($(this).parents('tr').find("td:eq(6)").html());

    _isEdit = true;
    _idHHDangSua = idHH;

    ($('#selectHHX')[0].selectize).setValue(idHH);

    var elmX = hangHoaXuat.find(function (item) {
      return item.IdHH == idHH;
    });

    var elmT = hangHoaTon.find(function (item) {
      return item.IdHH == idHH;
    });

    if (elmX != null && elmT != null) {
      var slCon = elmX.SL - SL;
      var slConT = elmT.SL - slCon;
      $('#SLcon').val(toDecimal(slConT))
     // setTimeout(function () {  }, 1000)
    }


    //$("#selectHHX option:selected").text(tenHH);
    $('#DVT').val(dvt);

    ($('#ThueXuat')[0].selectize).setValue(ThueXuat);
    $('#SLHH').val(SL);
    $('#DonGia').val(DonGia);
    $('#ThanhTien').val(ThanhTien);
    $('#ChietKhau').val(ChietKhau);


    changeEdit(index);



  });
})

//ok
function changeEdit(index) {
  var active = '<button class="btn btn-primary" id="ThemChiTietPX" onclick="onemoretime2(' + index + ')" type="button">Lưu</button>'
    + '<button class="btn btn-secondary" style="margin-left:5px;" onclick="changeOff()" type="button">Hủy</button>';

  $('#areabtn').html(active);
  _isEdit = true;
}

//ok
function changeOff() {
  var off = '<button class="btn btn-primary" id="ThemChiTietPX" onclick="onemoretime()" type="button">Thêm</button>';
  $('#areabtn').html(off);
  //$('#selectHHX').val("");
  clearFormChon();
  _isEdit = false;
  _idHHDangSua = 0;
}

//ok
function onemoretime() {
  addChiTietPhieuTam();
  updateTable2();
}

function onemoretime2(index) {
  editChiTietPhieuTam(index);
  updateTable2();
  changeOff();
}



//ok
//function updateTable2() {
//  //11111111111111111111111111
//  if ($("#tableChiTietPhieuXuat").find("tr:gt(0)").length < 1) {
//    $('#TienHang').val("");
//    $('#TienCK').val("");
//    $('#TienThue').val("");
//    $('#TienThanhToan').val("");
//    return;
//  }
//  var tienhang = 0;
//  var tienck = 0;
//  var tienthue = 0;
//  $("#tableChiTietPhieuXuat").find("tr:gt(0)").each(function () {
//    var SL = $(this).find("td:eq(2)").text();
//    var DonGia = $(this).find("td:eq(3)").text();
//    var ChietKhau = $(this).find("td:eq(5)").text();
//    var ThueXuat = $(this).find("td:eq(6)").text();
//    tienhang = tienhang + (SL * DonGia);
//    tienck = tienck + ((SL * DonGia * ChietKhau) / 100);
//    tienthue = tienthue + ((((SL * DonGia) - ((SL * DonGia * ChietKhau) / 100)) * ThueXuat) / 100);
//  });

//  var tienthanhtoan = tienhang - tienck + tienthue;
//  $('#TienHang').val(toDecimal(tienhang));
//  $('#TienCK').val(toDecimal(tienck));
//  $('#TienThue').val(toDecimal(tienthue));
//  $('#TienThanhToan').val(toDecimal(tienthanhtoan));

//}

function updateTable2() {
  //11111111111111111111111111
  var tienhang = 0;
  var tienck = 0;
  var tienthue = 0;
  $("#tableChiTietPhieuXuat").find("tr:gt(0)").each(function () {
    var SL = getValueNumbers($(this).find("td:eq(2)").text());
    var DonGia = getValueNumbers($(this).find("td:eq(3)").text());
    var ChietKhau = getValueNumbers($(this).find("td:eq(5)").text());
    var ThueXuat = getValueNumbers($(this).find("td:eq(6)").text());
    //console.log(SL, DonGia, ChietKhau, ThueXuat);

    tienhang = tienhang + (SL * DonGia);
    tienck = tienck + ((SL * DonGia * ChietKhau) / 100);
    tienthue = tienthue + ((((SL * DonGia) - ((SL * DonGia * ChietKhau) / 100)) * ThueXuat) / 100);
  });

  var tienthanhtoan = tienhang - tienck + tienthue;
  $('#TienHang').val(toDecimal(tienhang));
  $('#TienCK').val(toDecimal(tienck));
  $('#TienThue').val(toDecimal(tienthue));
  $('#TienThanhToan').val(toDecimal(tienthanhtoan));

}




function loadDVT() {
  var idHH = $('#selectHHX').val();
  console.log(idHH);
  var idKH = $('#IdKh').val();
  $('#DonGia').val("");
  $('#SLHH').val("");
  $('#ThanhTien').val("");


  $('#selectHHX').nextAll('.form-select2').removeClass('is-invalid');



  if (idHH != 0) {
    $.ajax({
      type: "post",
      url: "/getDonViTinhPX",
      data: "idHH=" + idHH,
      success: function (result) {
        $('#donvitinh').val(result.dvt);
        $('#DVT').val(result.tenDVT);
        // cập nhật số lượng tồn
        var slCon = result.slCon;
        if (slCon <= 0) {
          showToast('Vui lòng nhập thêm hàng hoá!', 500);
          $('#selectHHX').nextAll('.form-select2').addClass('is-invalid');
          $('#SLcon').val('');
          $('#SLHH').val('');
          $('#SLcon').addClass('is-invalid');
          $('#SLHH').addClass('is-invalid');
          $('#ThemChiTietPX').prop("disabled", true);
        } else {
          $('#ThemChiTietPX').prop("disabled", false);
          $('#SLcon').removeClass('is-invalid');
          $('#SLHH').removeClass('is-invalid');
          hamCapNhatSoLuongTon(idHH, Number(slCon));

          //cập nhật số lượng còn
          var elm = hangHoaXuat.find(function (item) {
            return item.IdHH == idHH;
          });
          if (elm != null) {
            slCon = result.slCon - elm.SL;
          }
          if (_isEdit) {
            if (_idHHDangSua != idHH) {
              $('#SLcon').val(toDecimal(slCon));
            } 
          } else {
            $('#SLcon').val(toDecimal(slCon));
          }
        }
       
        //$('#SLcon').val(toDecimal(result.slCon));

        //$('#SLHH').prop('readonly', false);
        //if (result.setgia == 1 || result.setgia == 2) {
        //  $('#DonGia').addClass('is-invalid');
        //  $('#ThanhTien').addClass('is-invalid');
        //  alert("Hàng hóa chưa được áp đặt giá bán!");
        //  $('#SLHH').prop('readonly', true);
        //}
      },
      error: function () {
        console.log("Thất bại!");
      }
    });
  }
  //if (idHH == "") {
  //  $('#selectHHX').nextAll('.form-select2').addClass('is-invalid');
  //}
  //if (idKH == "") {
  //  $('#KH').nextAll('.form-select2').addClass('is-invalid');
  //}
};

//
function inputSLHH() {
  $('#SLHH').removeClass('is-invalid');
  var SL = $('#SLHH').val();
  var SLcon = $('#SLcon').val();
  // alert(SLcon)
  var idHH = $('#selectHHX').val();

  var idDVT = $('#selectDVT').val();
  if (idHH != "" && SL > 0) {
    $.ajax({
      type: "post",
      url: "/checkgia",
      data: "idHH=" + idHH + "&SL=" + SL,
      success: function (result) {
        if (result.statusCode == 200) {
          if (getValue(SL) > SLcon) {
            $('#SLHH').addClass('is-invalid');
            $('#DonGia').val("");
            $('#ThanhTien').val("");
            $('#ThemChiTietPX').prop("disabled", true);
          } else {
            var model = result.model;
            $('#DonGia').val(toDecimal(model.donGia));
            $('#ThanhTien').val(toDecimal(model.thanhTien));
            $('#ThemChiTietPX').prop("disabled", false);
          }
        } else {
          showToast(result.message, result.statusCode);
          $('#DonGia').val("");
          $('#ThanhTien').val("");
          $('#ThemChiTietPX').prop("disabled", true);
        }

      },
      error: function () {
        console.log("Thất bại!");
      }
    });
  }
  //if (SL > slCon) {
  //  $('#SLHH').addClass('is-invalid');
  //}
  if (SL <= 0 || SL == "") {
    $('#DonGia').val("");
    $('#ThanhTien').val("");
  }



  //var DonGia = $('#DonGia').val();
  //var thanhTien = toDecimal(getValue(DonGia) * getValue(SL));
  //if (checkNumber(thanhTien)) {
  //  $('#ThanhTien').val(thanhTien);
  //}
  if (idHH == "") {
    $('#selectHHX').nextAll('.form-select2').addClass('is-invalid');
  }
  if (idDVT == "") {
    $('#selectDVT').addClass('is-invalid');
  }
};


function checkNumber(str) {
  return /[0-9,.\-$]+/.test(str);
}


$('#SLHH,#ThanhTien,#DonGia,#GiaBan,#ChietKhau').on('blur', format);
function format() {
  if (checkNumber(this.value)) {
    const value = this.value.replace(/,/g, '');
    this.value = parseFloat(value).toLocaleString('en-US', {
      style: 'decimal',
      maximumFractionDigits: 2,
      minimumFractionDigits: 2
    })
  }
}


var index = 1;
//thêm chi tiết phiếu xuất tạm
function addChiTietPhieuTam() {

  var idHH = $('#selectHHX').val();
  var tenHH = $("#selectHHX option:selected").text();
  var idDVT = $('#DVT').val();
  var dvt = $('#donvitinh').val();
  var ThueXuat = $('#ThueXuat').val();
  var SL = getValue($('#SLHH').val());
  var SLcon = getValue($('#SLcon').val());
  var DonGia = getValue($('#DonGia').val());
  var ChietKhau = getValue($('#ChietKhau').val());
  var ThanhTien = getValue($('#ThanhTien').val());

  if (idHH == "") {
    $('#selectHHX').nextAll('.form-select2').addClass('is-invalid');
    if (SL == "") {
      $('#SLHH').addClass('is-invalid');
    }
    return;
  } if (SL == "" || SL > SLcon) {
    $('#SLHH').addClass('is-invalid');
    return;
  }

  ////kiểm tra xem hàng hoá có tồn tại trong mảng chưa
  //var isExist = hangHoaXuat.some(function (item) {
  //  return item.IdHHX == idHH;
  //});

  //if (!isExist) {
  //  hangHoaXuat.push({ IdHHX: Number(idHH), SLX: Number(SL) });
  //} else {
  //  //cộng SLX nếu tồn tại
  //  var elm = hangHoaXuat.find(function (item) {
  //    return item.IdHHX == idHH;
  //  });
  //  elm.SLX += SL;
  //}

  //console.log(hangHoaXuat);

 // hangHoaXuat.push(model);



  var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input class="IDCT" type="hidden" value="' + idHH + '"/>' + tenHH + '</td><td class="text-left">'
    + '<input id="dvtc" type="hidden" value="' + dvt + '"/>' + idDVT + '</td><td class="text-right soLuong">' + toDecimal(SL) + '</td><td class="text-right">' + toDecimal(DonGia)
    + '</td><td class="text-right">' + toDecimal(ThanhTien) + '</td><td class="text-right">' + toDecimal(ChietKhau) + '</td><td class="text-right">' + toDecimal(ThueXuat)
    + '</td><td class="last-td-column"> <button type="button" class="btn btn-table p-0 edit"><i class="far fa-edit lighter pr-2" ></i></button><button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')">'
    + '<i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'
  //onclick="UpdateRow(' + index + ')"
  $('#body_ctpx').prepend(ChitietRecord);

  //cập nhật SLC
  hamCapNhatSoLuongCon(idHH, SL);
  var slConOld = getValue($('#SLcon').val());
  var slConNew = slConOld - SL;
  $('#SLcon').val(toDecimal(slConNew));


  // updateTable2();
  index++;


}



//OK
function editChiTietPhieuTam(index) {
  var idHH = $('#selectHHX').val();
  var tenHH = $("#selectHHX option:selected").text();
  var idDVT = $('#DVT').val();
  var dvt = $('#donvitinh').val();
  var ThueXuat = $('#ThueXuat').val();
  var SL = getValue($('#SLHH').val());
  var DonGia = getValue($('#DonGia').val());
  var ChietKhau = getValue($('#ChietKhau').val());
  var ThanhTien = getValue($('#ThanhTien').val());

  if (idHH == "" || idHH == null) {
    showToast('Vui lòng chọn hàng hoá!', 500);
    return;
  }

  if (SL == "" || SL <= 0) {
    if (SL < 0) {
      showToast('Số lượng không hợp lệ!', 100);
      return;
    } else {
      showToast('Vui lòng nhập số lượng hàng hoá!', 100);
      return;
    }
  }

  if (DonGia == "" || DonGia <= 0) {
    if (DonGia < 0) {
      showToast('Đơn giá không hợp lệ!', 500);
      return;
    } else {
      showToast('Vui lòng nhập đơn giá hoặc thành tiền!', 100);
      return;
    }
  }

  if (ThanhTien < 0) {
    showToast('Thành tiền không hợp lệ!', 500);
    return;
  }
  var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input class="IDCT" type="hidden" value="' + idHH + '"/>' + tenHH + '</td><td class="text-left">'
    + '<input id="dvtc" type="hidden" value="' + dvt + '"/>' + idDVT + '</td><td class="text-right soLuong">' + toDecimal(SL) + '</td><td class="text-right">' + toDecimal(DonGia)
    + '</td><td class="text-right">' + toDecimal(ThanhTien) + '</td><td class="text-right">' + toDecimal(ChietKhau) + '</td><td class="text-right">' + toDecimal(ThueXuat)
    + '</td><td class="last-td-column"> <button type="button" class="btn btn-table p-0 edit"><i class="far fa-edit lighter pr-2" ></i></button>' + '<button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')">'
    + '<i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'
  $("tr#" + index).replaceWith(ChitietRecord);
  hamLamMoiSoLuongCon(idHH);
  _isEdit = false;
  _idHHDangSua = 0;
}



//tạo phiếu xuất
function CreatePX() {

  if ($('#body_ctpx tr').length < 1) {
    showToast('Vui lòng nhập thông tin phiếu!', 500);
    return;
  }
  console.log(Number($('#idKH').val()));
  var idKH = Number($('#idKH').val());
  if (idKH == 0) {
    showToast('Vui lòng chọn khách hàng!', 100);
    return;
  }

  var listOfCTPXT = new Array();
  var NgayHd = $('#NgayHD').val();
  var SoHd = $('#SoHd').val();
  var Note = $('#Note').val();

  $("#tableChiTietPhieuXuat").find("tr:gt(0)").each(function () {
    //var index = $(this).closest('tr').attr('id');
    var idHH = $(this).find("td:eq(0) input[type='hidden']").val();
    var IDDVT = $(this).find("td:eq(1) input[type='hidden']").val();
    var SL = $(this).find("td:eq(2)").text();
    var DonGia = $(this).find("td:eq(3)").text();
    var ThanhTien = $(this).find("td:eq(4)").text();
    var ChietKhau = $(this).find("td:eq(5)").text();
    var ThueXuat = $(this).find("td:eq(6)").text();
    var ChiTietPhieuXuatTam = {};
   // ChiTietPhieuXuatTam.Id = Number(index);
    ChiTietPhieuXuatTam.Idhh = Number(idHH);
    ChiTietPhieuXuatTam.IdDvt = Number(IDDVT);
    ChiTietPhieuXuatTam.Slg = getValue(SL);
    ChiTietPhieuXuatTam.DonGia = getValue(DonGia);
    ChiTietPhieuXuatTam.ThanhTien = getValue(ThanhTien);
    ChiTietPhieuXuatTam.Cktm = getValue(ChietKhau);
    ChiTietPhieuXuatTam.Thue = getValue(ThueXuat);
    listOfCTPXT.push(ChiTietPhieuXuatTam);
  });

  var PhieuXuatModel = {};
  PhieuXuatModel.NgayHd = NgayHd;
  PhieuXuatModel.SoHd = SoHd;
  PhieuXuatModel.KH = Number(idKH);
  PhieuXuatModel.Note = Note;
  PhieuXuatModel.listOfCTPXT = listOfCTPXT;
  console.log(PhieuXuatModel);
  $.ajax({
    url: '/listCTPXT',
    type: 'POST',
    dataType: 'JSON',
    contentType: 'application/json; charset=utf-8',
    data: JSON.stringify(PhieuXuatModel),
    success: function (result) {
      showToast(result.message, result.statusCode);
      if (result.statusCode == 200) {
        console.log(result.data);
        //$('#SoPN').val(result.data);
        document.getElementById("SoPx").value = result.data;
        refreshFormTaoPhieu();
      }
      //location.reload();
    },
    error: function () {
      showToast("Thất bại!", 500);
    }
  });

}


function refreshFormTaoPhieu() {
  $('#idKH')[0].selectize.clear();
  $('#NgayNhap').val(moment().format('DD-MM-YYYY HH:mm'));
  $('#NgayHD').val(moment().format('DD-MM-YYYY'));
  $('#body_ctpx tr').remove();
  $('#TienHang').val("");
  $('#TienCK').val("");
  $('#TienThue').val("");
  $('#TienThanhToan').val("");
  clearFormChon();
}


function clearFormChon() {
  $('#selectHHX')[0].selectize.clear();
  $('#ThueXuat')[0].selectize.setValue(0);
  $('#DVT').val("");
  $('#ThueXuat').val("");
  $('#SLHH').val("");
  $('#SLcon').val("");
  $('#DonGia').val("");
  $('#ThanhTien').val("");
  $('#ChietKhau').val(0);
  $('#HanDung').val(moment().format("DD-MM-YYYY"));
  $('#NgaySX').val(moment().format("DD-MM-YYYY"));
}




function ViewPhieuXuat(idPhieuXuat) {
  $('#bordered-justified-profile').removeClass('active');
  $('#bordered-justified-profile').removeClass('show');
  $('#tabXemPhieu').removeClass('d-lg-none');

  $.ajax({
    type: "post",
    url: "/ViewThongTinPhieuXuat",
    data: "idPX=" + idPhieuXuat,
    success: function (result) {
      $('#tabXemPhieu').replaceWith(result);
    },
    error: function () {
      showToast("Thất bại!", 500);
    }
  });
}

function offTabXuat() {
  $('#tabXemPhieu').addClass('d-lg-none');
}
function cancelXemPhieu() {
  $('#tabXemPhieu').addClass('d-lg-none');
  $('#bordered-justified-profile').addClass('active');
  $('#bordered-justified-profile').addClass('show');
}

//function appendDVT() {
//  var idKH = $('#KH').val();
//  $('#selectHHX').nextAll('.custom-select').removeClass('is-invalid');
//  var idHH = $('#selectHHX').val();
//  if (idKH != "" && idHH != "") {
//    $.ajax({
//      type: "post",
//      url: "/loadListDVT",
//      data: "idHH=" + idHH + "&idKH=" + idKH,
//      success: function (result) {
//        $('#selectDVT').empty();
//        $('#selectDVT').append(result.options);
//        if (result.giaBan == 0) {
//          alert('Hết hàng');
//        } else {
//          $('#DonGia').val(toDecimal(result.giaBan));
//          $('#SLcon').val(toDecimal(result.slCon));
//        }
//        $('#imageHH').attr("src", "/lib/imagesHH/" + result.hinhAnh);
//        /*$('#imageHH').attr("alt",$('#selectHHX').children(':selected').text());*/

//      },
//      error: function () {
//        alert("Fail");
//      }
//    });
//  }
//  if (idHH == "") {
//    $('#selectHHX').nextAll('.custom-select').addClass('is-invalid');
//  }
//  if (idKH == "") {
//    $('#KH').nextAll('.custom-select').addClass('is-invalid');
//  }
//}


//function appendDVTKH() {
//  var idKH = $('#KH').val();
//  $('#KH').nextAll('.custom-select').removeClass('is-invalid');
//  var idHH = $('#selectHHX').val();
//  if (idHH != "" && idKH != "") {
//    $.ajax({
//      type: "post",
//      url: "/loadListDVT",
//      data: "idHH=" + idHH + "&idKH=" + idKH,
//      success: function (result) {
//        console.log(result);
//        $('#selectDVT').empty();
//        $('#selectDVT').append(result.options);
//        $('#selectDVT').removeClass('is-invalid');
//        if (result.giaBan == 0) {
//          alert('Hết hàng');
//        } else {
//          $('#DonGia').val(toDecimal(result.giaBan));
//          $('#SLcon').val(toDecimal(result.slCon));
//        }
//      },
//      error: function () {
//        alert("Fail");
//      }
//    });
//  }
//  if (idHH == "") {
//    $('#selectHHX').nextAll('.custom-select').addClass('is-invalid');
//  }
//  if (idKH == "") {
//    $('#KH').nextAll('.custom-select').addClass('is-invalid');
//  }
//}

//function loadTienDVT() {
//  var idKH = $('#KH').val();
//  var idHH = $('#selectHHX').val();
//  var idDVT = $('#selectDVT').val();
//  $('#selectDVT').removeClass('is-invalid');
//  if (idHH != "" && idKH != "") {
//    $.ajax({
//      type: "post",
//      url: "/loadTienDVT",
//      data: "idHH=" + idHH + "&idKH=" + idKH + "&idDVT=" + idDVT,
//      success: function (result) {
//        if (result == 0) {
//          alert('Hết hàng');
//        } else {
//          $('#DonGia').val(toDecimal(result.giaBan));
//          $('#SLcon').val(toDecimal(result.slCon));
//        }
//      },
//      error: function () {
//        alert("Fail");
//      }
//    });
//  }
//}

//// Get the modal
//var modal = document.getElementById("imageModal");

//// Get the image and insert it inside the modal - use its "alt" text as a caption
//var img = document.getElementById("imageHH");
//var modalImg = document.getElementById("imageHHModal");
//var captionText = document.getElementById("imageModelCaption");
//img.onclick = function () {
//  modal.style.display = "block";
//  modalImg.src = this.src;
//  captionText.innerHTML = this.alt;
//}

//// Get the <span> element that closes the modal
//var span = document.getElementsByClassName("closeImageModal")[0];

//// When the user clicks on <span> (x), close the modal
//span.onclick = function () {
//  modal.style.display = "none";
//}


function loadTableLichSuXuat() {
  var toDay = $('#toDay').val();
  var fromDay = $('#fromDay').val();
  var x = new Date(toDay.substring(3, 5) + "-" + toDay.substring(0, 2) + "-" + toDay.substring(6, 10));
  var y = new Date(fromDay.substring(3, 5) + "-" + fromDay.substring(0, 2) + "-" + fromDay.substring(6, 10));
  if (x < y) {
    showToast('Giới hạn ngày không hợp lệ!', 500);
    return;
  }
  $.ajax({
    type: "post",
    url: "/loadTableLichSuXuat",
    data: "toDay=" + toDay + "&fromDay=" + fromDay,
    success: function (result) {
      $('#tableLichSuXuat').replaceWith(result);
    },
    error: function () {
      showToast("Thất bại", 500);
    }
  });
}



function hamCapNhatSoLuongCon(idHH, SL) {
  //kiểm tra xem hàng hoá có tồn tại trong mảng chưa
  var isExist = hangHoaXuat.some(function (item) {
    return item.IdHH == idHH;
  });

  if (!isExist) {
    hangHoaXuat.push({ IdHH: Number(idHH), SL: Number(SL) });
  } else {
    //cộng SLX nếu tồn tại
    var elm = hangHoaXuat.find(function (item) {
      return item.IdHH == idHH;
    });
    elm.SL += SL;
  }



}


function hamLamMoiSoLuongCon(idHHCurrent) {
  hangHoaXuat = [];

  $('#tableChiTietPhieuXuat tr:gt(0)').each(function () {
    var idHH = $(this).find('input.IDCT').val();
    var SL = getValue($(this).find('td.soLuong').text());
    console.log(idHH, SL);
    hamCapNhatSoLuongCon(idHH, SL);
  })
  var elmX = hangHoaXuat.find(function (item) {
    return item.IdHH == idHHCurrent;
  });

  var elmT = hangHoaTon.find(function (item) {
    return item.IdHH == idHHCurrent;
  });

  if (elmT != null || elmX != null) {
    var slConNew = elmT.SL - elmX.SL;
    $('#SLcon').val(toDecimal(slConNew));
  }
}



function hamCapNhatSoLuongTon(idHH, SL) {
  //hangHoaTon = [];
  //kiểm tra xem hàng hoá có tồn tại trong mảng chưa
  var isExist = hangHoaTon.some(function (item) {
    return item.IdHH == idHH;
  });

  if (!isExist) {
    hangHoaTon.push({ IdHH: Number(idHH), SL: Number(SL) });
  } else {
    //cộng SLX nếu tồn tại
    var elm = hangHoaTon.find(function (item) {
      return item.IdHH == idHH;
    });
    elm.SL = Number(SL);
  }
}