
$(document).ready(function () {

  //document.getElementById("temp_ct").remove();
  $("#ThemChiTietPX").click(function () {
    addChiTietPhieuTam();
    updateTable2();
  });


  $("#btn-create").click(function () {
    CreatePX();
  });

});


function deleteRow(index) {
  var row = document.getElementById(index);
  row.parentNode.removeChild(row);
  updateTable2();
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
    var ThueXuat = $(this).parents('tr').find("td:eq(6)").html();

    ($('#selectHHX')[0].selectize).setValue(idHH);


    //$("#selectHHX option:selected").text(tenHH);
    $('#DVT').val(dvt);

    // $("#ThueXuat").val(ThueXuat).change();

    ($('#ThueXuat')[0].selectize).setValue(ThueXuat);
    $('#SLHH').val(toDecimal(SL));
    $('#DonGia').val(toDecimal(DonGia));
    $('#ThanhTien').val(toDecimal(ThanhTien));
    $('#ChietKhau').val(ChietKhau);


    changeEdit(index);



  });
})

//ok
function changeEdit(index) {
  var active = '<button class="btn btn-primary" id="ThemChiTietPX" onclick="onemoretime2(' + index + ')" type="button">Lưu</button>'
    + '<button class="btn btn-secondary" style="margin-left:5px;" onclick="changeOff()" type="button">Hủy</button>';

  $('#areabtn').html(active);
}

//ok
function changeOff() {
  var off = '<button class="btn btn-primary" id="ThemChiTietPX" onclick="onemoretime()" type="button">Thêm</button>';
  $('#areabtn').html(off);
  //$('#selectHHX').val("");
  ($('#ThueXuat')[0].selectize).setValue(0);
  $('#DVT').val("");
  $('#SoLo').val("");
  $('#ThueXuat').val("");
  $('#SLHH').val("");
  $('#DonGia').val("");
  $('#ThanhTien').val("");
  $('#ChietKhau').val("");
  $('#HanDung').val() = moment().ToString("dd-MM-yyyy");
  $('#NgaySX').val() = moment().ToString("dd-MM-yyyy");
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
function updateTable2() {
  //11111111111111111111111111
  if ($("#tableChiTietPhieuXuat").find("tr:gt(0)").length < 1) {
    $('#TienHang').val("");
    $('#TienCK').val("");
    $('#TienThue').val("");
    $('#TienThanhToan').val("");
    return;
  }
  var tienhang = 0;
  var tienck = 0;
  var tienthue = 0;
  $("#tableChiTietPhieuXuat").find("tr:gt(0)").each(function () {
    var SL = $(this).find("td:eq(2)").text();
    var DonGia = $(this).find("td:eq(3)").text();
    var ChietKhau = $(this).find("td:eq(5)").text();
    var ThueXuat = $(this).find("td:eq(6)").text();
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
        $('#SLcon').val(toDecimal(result.slCon));
        //$('#SLHH').prop('readonly', false);
        //if (result.setgia == 1 || result.setgia == 2) {
        //  $('#DonGia').addClass('is-invalid');
        //  $('#ThanhTien').addClass('is-invalid');
        //  alert("Hàng hóa chưa được áp đặt giá bán!");
        //  $('#SLHH').prop('readonly', true);
        //}
      },
      error: function () {
        showToast("Thất bại!", 500);
      }
    });
  }
  if (idHH == "") {
    $('#selectHHX').nextAll('.form-select2').addClass('is-invalid');
  }
  if (idKH == "") {
    $('#KH').nextAll('.form-select2').addClass('is-invalid');
  }
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
          } else {
            var model = result.model;
            // $('#SLHH').addClass('is-invalid');
            $('#DonGia').val(toDecimal(model.donGia));
            $('#ThanhTien').val(toDecimal(model.thanhTien));
            //alert(result.thanhTien);
          }
          //$('#ThemChiTietPX').prop("disabled", false);
        } else {
          showToast(result.message, result.statusCode);
          $('#DonGia').val("");
          $('#ThanhTien').val("");
          $('#ThemChiTietPX').prop("disabled", true);
        }

      },
      error: function () {
        showToast("Thất bại!", 500);
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



$('#ChietKhau').on('input', function () {

  if (getValue(this.value) > 100) {
    $(this).addClass('is-invalid');
  } else {
    $(this).removeClass('is-invalid');
  }
});



function getValue(str) {
  return Number(str.replace(/[^0-9.-]+/g, ""));
}


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


function toDecimal(str) {
  return parseFloat(str).toLocaleString('en-US', {
    style: 'decimal',
    maximumFractionDigits: 2,
    minimumFractionDigits: 2
  });
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

  var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input id="IDCT" type="hidden" value="' + idHH + '"]/>' + tenHH + '</td><td class="text-left">'
    + '<input id="dvtc" type="hidden" value="' + dvt + '"]/>' + idDVT + '</td><td class="text-right">' + toDecimal(SL) + '</td><td class="text-right">' + toDecimal(DonGia)
    + '</td><td class="text-right">' + toDecimal(ThanhTien) + '</td><td class="text-right">' + toDecimal(ChietKhau) + '</td><td class="text-right">' + toDecimal(ThueXuat)
    + '</td><td> <button type="button" class="btn btn-table p-0 edit"><i class="far fa-edit lighter pr-2" ></i></button><button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')">'
    + '<i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'
  //onclick="UpdateRow(' + index + ')"
  $('#body_ctpnx').prepend(ChitietRecord);

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

  else {
    var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input id="IDCT" type="hidden" value="' + idHH + '"]/>' + tenHH + '</td><td class="text-left">'
      + '<input id="dvtc" type="hidden" value="' + dvt + '"]/>' + idDVT + '</td><td class="text-right">' + SL + '</td><td class="text-right">' + toDecimal(DonGia)
      + '</td><td class="text-right">' + toDecimal(ThanhTien) + '</td><td class="text-right">' + toDecimal(ChietKhau) + '</td><td class="text-right">' + toDecimal(ThueXuat)
      + '</td><td> <button type="button" class="btn btn-table p-0 edit" onclick="UpdateRow(' + index + ')"><i class="far fa-edit lighter pr-2" ></i></button>' + '<button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')">'
      + '<i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'
    $("tr#" + index).replaceWith(ChitietRecord);
  }
}



//tạo phiếu xuất
function CreatePX() {
  var listOfCTPXT = new Array();
  var NgayHd = $('#NgayHD').val();
  var SoHd = $('#SoHd').val();
  var idKH = $('#idKH').val();
  var Note = $('#Note').val();

  $("#tableChiTietPhieuXuat").find("tr:gt(0)").each(function () {
    var index = $(this).closest('tr').attr('id');
    var idHH = $(this).find("td:eq(0) input[type='hidden']").val();
    var IDDVT = $(this).find("td:eq(1) input[type='hidden']").val();
    var SL = $(this).find("td:eq(2)").text();
    var DonGia = $(this).find("td:eq(3)").text();
    var ThanhTien = $(this).find("td:eq(4)").text();
    var ChietKhau = $(this).find("td:eq(5)").text();
    var ThueXuat = $(this).find("td:eq(6)").text();
    var ChiTietPhieuXuatTam = {};
    ChiTietPhieuXuatTam.Id = Number(index);
    ChiTietPhieuXuatTam.Idhh = Number(idHH);
    ChiTietPhieuXuatTam.IdDvt = Number(IDDVT);
    ChiTietPhieuXuatTam.Slg = Number(SL);
    ChiTietPhieuXuatTam.DonGia = Number(DonGia);
    ChiTietPhieuXuatTam.ThanhTien = Number(ThanhTien);
    ChiTietPhieuXuatTam.Cktm = Number(ChietKhau);
    ChiTietPhieuXuatTam.Thue = Number(ThueXuat);
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
      alert(result);
      //location.reload();
    },
    error: function () {
      showToast("Thất bại!", 500);
    }
  });

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
