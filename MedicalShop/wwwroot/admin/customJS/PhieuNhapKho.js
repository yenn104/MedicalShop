  //ok
  $(document).ready(function () {

    //document.getElementById("temp_ct").remove();
    $("#ThemChiTiet").click(function () {
      addChiTietPhieuTam();
      updateTable2();
    });


    $("#btn-create").click(function () {
      CreatePN();
    });

    $('#ChietKhau').on('input', function () {
      if (getValue(this.value) > 100) {
        $(this).val('');
        showToast('Chiết khấu không được quá 100%', 500);
      }
    });

    $("#tableChiTietPhieuNhap").on("click", ".edit", function () {
      var index = $(this).closest('tr').attr('id');
      var idHH = $(this).parents('tr').find("td:eq(0) input[type='hidden']").val();
      var dvt = $(this).parents('tr').find("td:eq(1)").html();
      var SoLo = $(this).parents('tr').find("td:eq(2)").html();
      var SL = $(this).parents('tr').find("td:eq(3)").html();
      var DonGia = $(this).parents('tr').find("td:eq(4)").html();
      var ThanhTien = $(this).parents('tr').find("td:eq(5)").html();
      var ChietKhau = $(this).parents('tr').find("td:eq(6)").html();
      var HanDung = $(this).parents('tr').find("td:eq(9)").html();
      var ngaySX = $(this).parents('tr').find("td:eq(8)").html();
      var ThueXuat = Number($(this).parents('tr').find("td:eq(7)").html());
      console.log(ThanhTien);

      //$('#selectHH').val(idHH);
      ($('#selectHH')[0].selectize).setValue(idHH);

      //$("#selectHH option:selected").text(tenHH);
      $('#DVT').val(dvt);
      $('#SoLo')[0].selectize.setValue(SoLo);

      // $("#ThueXuat").val(ThueXuat).change();

      ($('#ThueXuat')[0].selectize).setValue(ThueXuat);
      $('#SLHH').val(SL);
      $('#DonGia').val(DonGia);
      $('#ThanhTien').val(ThanhTien);
      $('#ChietKhau').val(ChietKhau);
      $('#ThueXuat').val(ThueXuat);
      $('#HanDung').val(HanDung);
      $('#NgaySX').val(ngaySX);

      changeEdit(index);

    });


    $('.number-input').bind("cut copy paste drag drop", function (e) {
      e.preventDefault();
    });

   

  });


  function isNumber(e) {
      var charCode = (e.which) ? e.which : e.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46 /*&& charCode != 43*/)
        return false;
      return true;
  }



  //ok
  function deleteRow(index) {
    var row = document.getElementById(index);
    row.parentNode.removeChild(row);
    updateTable2();
  }



  //ok
  function changeEdit(index) {
    var active = '<button class="btn btn-primary" id="ThemChiTiet" onclick="onemoretime2('+ index+')" type="button">Lưu</button>'
      +'<button class="btn btn-secondary" style="margin-left:5px;" onclick="changeOff()" type="button">Hủy</button>';

    $('#areabtn').html(active);
  }

  //ok
  function changeOff(){
    var off = '<button class="btn btn-primary" id="ThemChiTiet" onclick="onemoretime()" type="button">Thêm</button>';
    $('#areabtn').html(off);
    $('#selectHH')[0].selectize.clear();
    ($('#ThueXuat')[0].selectize).setValue(0);
    $('#DVT').val("");
    $('#SoLo')[0].selectize.clear();
    $('#ThueXuat').val("");
    $('#SLHH').val("");
    $('#DonGia').val("");
    $('#ThanhTien').val("");
    $('#ChietKhau').val(0);
    //$('#HanDung').val() = moment().ToString("dd-MM-yyyy");
    $('#HanDung').val(moment().format("DD-MM-YYYY"));
    //$('#NgaySX').val() = moment().ToString("dd-MM-yyyy");
    $('#NgaySX').val(moment().format("DD-MM-YYYY"));

  }

  //ok
  function onemoretime(){
    addChiTietPhieuTam();
    updateTable2();
  }

  function onemoretime2(index) {
    editChiTietPhieuTam(index);
    updateTable2();
    changeOff();
  }


 //ok
  var index = 1;

  function addChiTietPhieuTam() {
    var idHH = $('#selectHH').val();
    var tenHH = $("#selectHH option:selected").text();
    var dvt = $('#DVT').val();
    var SoLo = $('#SoLo').val();
    console.log(SoLo);
    var ThueXuat = $('#ThueXuat').val();
    var SL = getValueNumbers($('#SLHH').val());
    var DonGia = getValueNumbers($('#DonGia').val());
    var ThanhTien = getValue($('#ThanhTien').val());
    var ChietKhau = getValue($('#ChietKhau').val());
    var HanDung = $('#HanDung').val();
    var ngaySX = $('#NgaySX').val();
    var x = formatDay(HanDung);
    var y = formatDay(ngaySX);

    if (idHH == "" || idHH == null){
      showToast('Vui lòng chọn hàng hoá!', 100);
      return;
    }

    if (SoLo == "" || SoLo == null) {
      showToast('Vui lòng nhập số lô!', 100);
      return;
    }

    if (SL == "" || SL <= 0){
      if (SL < 0) {
        showToast('Số lượng không hợp lệ!', 500);
        return;
      }else{
        showToast('Vui lòng nhập số lượng hàng hoá!', 100);
        return;
      }
    }

    if (DonGia == "" || DonGia <= 0)
    {
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
    if (x < y) {
      showToast('Hạn sử dụng phải lớn hơn ngày sản xuất!', 500);
      return;
    }
    //thêm: giá tiền notnull

    else
    {
      var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input id="IDCT" type="hidden" value="' + idHH + '"]/>' + tenHH + '</td><td class="text-left">'
        + dvt + '</td><td class="text-left">' + SoLo + '</td><td class="text-right">' + formatNumbers(SL) + '</td><td class="text-right">' + formatNumbers(DonGia) 
        + '</td><td class="text-right">' + formatNumbers(ThanhTien) + '</td><td class="text-right">' + formatNumbers(ChietKhau) + '</td><td class="text-right">' + formatNumbers(ThueXuat)  + '</td><td>' + ngaySX + '</td><td>'
        + HanDung + '</td><td> <button type="button" class="btn btn-table p-0 edit"><i class="far fa-edit lighter pr-2" ></i></button>' + '<button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')"><i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'
      //onclick="UpdateRow(' + index + ')"
      $('#body_ctpn').prepend(ChitietRecord);

     // updateTable2();
      index++;
    }
  }


  function editChiTietPhieuTam(index) {   
    var idHH = $('#selectHH').val();
    var tenHH = $("#selectHH option:selected").text();
    var dvt = $('#DVT').val();
    var SoLo = $('#SoLo').val();
    var ThueXuat = $('#ThueXuat').val();
    var SL = getValue($('#SLHH').val());
    var DonGia = getValue($('#DonGia').val());
    var ThanhTien = getValue($('#ThanhTien').val());
    var ChietKhau = getValue($('#ChietKhau').val());
    var HanDung = $('#HanDung').val();
    var ngaySX = $('#NgaySX').val();
    var x = formatDay(HanDung);
    var y = formatDay(ngaySX);

    if (idHH == "" || idHH == null) {
      showToast('Vui lòng chọn hàng hoá!', 100);
      return;
    }

    if (SoLo == null) {
      showToast('Vui lòng nhập số lô!', 100);
      return;
    }

    if (SL == "" || SL <= 0) {
      if (SL < 0) {
        showToast('Số lượng không hợp lệ!', 500);
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

    if (x < y) {
      showToast('Hạn sử dụng phải lớn hơn ngày sản xuất!', 500);
      return;
    }

    else {
      var ChitietRecord = '<tr class="index" id="' + index + '">' + '<td class="text-left">' + '<input id="IDCT" type="hidden" value="' + idHH + '"]/>' + tenHH + '</td><td class="text-left">'
        + dvt + '</td><td class="text-left">' + SoLo + '</td><td class="text-right">' + formatNumbers(SL) + '</td><td class="text-right">' + formatNumbers(DonGia)
        + '</td><td class="text-right">' + formatNumbers(ThanhTien) + '</td><td class="text-right">' + formatNumbers(ChietKhau) + '</td><td class="text-right">' + formatNumbers(ThueXuat) + '</td><td>' + ngaySX +  '</td><td>'
        + HanDung + '</td><td> <button type="button" class="btn btn-table p-0 edit"><i class="far fa-edit lighter pr-2" ></i></button>' + '<button type="button" class="btn btn-table p-0" onclick="deleteRow(' + index + ')"><i class="fas fa-trash-alt lighter" ></i></button> </td></tr>'

      $("tr#" + index).replaceWith(ChitietRecord);
    }
  }







  //ok
  function updateTable2() {
  //11111111111111111111111111
    var tienhang = 0;
    var tienck = 0;
    var tienthue = 0;
    $("#tableChiTietPhieuNhap").find("tr:gt(0)").each(function () {
      var SL = getValueNumbers($(this).find("td:eq(3)").text());
      var DonGia = getValueNumbers($(this).find("td:eq(4)").text());
      var ChietKhau = getValueNumbers($(this).find("td:eq(6)").text());
      var ThueXuat = getValueNumbers($(this).find("td:eq(7)").text());
      console.log(SL, DonGia, ChietKhau, ThueXuat);

      tienhang = tienhang + (SL * DonGia);
      tienck = tienck + ((SL * DonGia * ChietKhau) / 100);
      tienthue = tienthue + ((((SL * DonGia) - ((SL * DonGia * ChietKhau) / 100)) * ThueXuat) / 100);
    });

    var tienthanhtoan = tienhang - tienck + tienthue;
    $('#TienHang').val(formatNumbers(tienhang));
    $('#TienCK').val(formatNumbers(tienck));
    $('#TienThue').val(formatNumbers(tienthue));
    $('#TienThanhToan').val(formatNumbers(tienthanhtoan));

  }




function CreatePN() {
    var NCC = Number($('#NCC').val());
    if (NCC == 0) {
      showToast('Vui lòng chọn nhà cung cấp!', 100);
      return;
    }

    var listOfCTPNT = new Array();
    var NgayHd = $('#NgayHD').val();
    var SoHd = $('#SoHd').val();
    var Note = $('#Note').val();

    $("#tableChiTietPhieuNhap").find("tr:gt(0)").each(function () {
      var index = $(this).closest('tr').attr('id');
      var idHH = $(this).find("td:eq(0) input[type='hidden']").val();
      var SoLo = $(this).find("td:eq(2)").text();
      var SL = $(this).find("td:eq(3)").text();
      var DonGia = $(this).find("td:eq(4)").text();
      var ThanhTien = $(this).find("td:eq(5)").text();
      var ChietKhau = $(this).find("td:eq(6)").text();
      var HanDung = $(this).find("td:eq(9)").text();
      var ngaySX = $(this).find("td:eq(8)").text();
      var ThueXuat = $(this).find("td:eq(7)").text();

      var ChiTietPhieuNhapTam = {};
      ChiTietPhieuNhapTam.Id = Number(index);
      ChiTietPhieuNhapTam.Idhh = Number(idHH);
      ChiTietPhieuNhapTam.Slg = Number(SL);
      ChiTietPhieuNhapTam.DonGia = Number(DonGia);
      ChiTietPhieuNhapTam.ThanhTien = Number(ThanhTien);
      ChiTietPhieuNhapTam.Cktm = Number(ChietKhau);
      ChiTietPhieuNhapTam.Thue = Number(ThueXuat);
      ChiTietPhieuNhapTam.SoLo = SoLo;
      ChiTietPhieuNhapTam.Nsx = ngaySX;
      ChiTietPhieuNhapTam.Hsd = HanDung;
      listOfCTPNT.push(ChiTietPhieuNhapTam);

    });

    var PhieuNhapModel = {};
    PhieuNhapModel.NgayHd = NgayHd;
    PhieuNhapModel.SoHd = SoHd;
    PhieuNhapModel.NCC = Number(NCC);
    PhieuNhapModel.Note = Note;
    PhieuNhapModel.listOfCTPNT = listOfCTPNT;

    $.ajax({
      url: '/listCTPNT',
      type: 'POST',
      dataType: 'JSON',
      contentType: 'application/json; charset=utf-8',
      data: JSON.stringify(PhieuNhapModel),
      success: function (result) {
        showToast(result.message, result.statusCode);
        //showToast(result, 500);
        location.reload();
      },
      error: function () {
        showToast('Thất bại!', 500);
      }
    });

  }

function refreshFormTaoPhieu() {

}



  //ok
  function loadDVT() {
    var idHH = $('#selectHH').val();
    if (idHH != 0) {
      $.ajax({
        type: "post",
        url: "/getDonViTinh",
        data: "idHH=" + idHH,
        success: function (result) {
          $('#DVT').val(result.tenDVT);
        //  $('#GiaBan').val(formatNumbers(result.giaBan));
        },
        error: function () {
          showToast('Thất bại!', 500);
        }
      });
    }
  };

  function inputSLHH() {
    var thanhTien = $('#ThanhTien').val();
    var SL = $('#SLHH').val();
      var DonGia = formatNumbers(getValue(thanhTien) / getValue(SL));
      if (checkNumber(DonGia)) {
        $('#DonGia').val(DonGia);
     }
  };

  function inputDonGia() {
    var SL = $('#SLHH').val();
    var DonGia = $('#DonGia').val();
    var thanhTien = formatNumbers(getValue(DonGia) * getValue(SL));
    if (checkNumber(thanhTien)) {
      $('#ThanhTien').val(thanhTien);
    }
  };

  function inputThanhTien() {
    var SL = $('#SLHH').val();
    var thanhTien = $('#ThanhTien').val();
    var DonGia = formatNumbers(getValue(thanhTien) / getValue(SL));
    console.log(SL, DonGia, thanhTien);
    if (checkNumber(DonGia)) {
      $('#DonGia').val(DonGia);
    }
  }


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
      this.value = formatNumbers(value);
      //this.value = parseFloat(value).toLocaleString('en-US', {
      //  style: 'decimal',
      //  maximumFractionDigits: 2,
      //  minimumFractionDigits: 2
      //})
    }
  }



  //xem chi tiết phiếu nhập (lịch sử)
  function ViewPhieuNhap(idPhieuNhap) {
    $('#bordered-justified-profile').removeClass('active');
    $('#bordered-justified-profile').removeClass('show');
    $('#tabXemPhieu').removeClass('d-lg-none');

    $.ajax({
      type: "post",
      url: "/ViewThongTinPhieuNhap",
      data: "idPN=" + idPhieuNhap,
      success: function (result) {
        $('#tabXemPhieu').replaceWith(result);
      },
      error: function () {
        showToast('Thất bại!', 500);
      }
    });
  }


  function offTabNhap() {
    $('#tabXemPhieu').addClass('d-lg-none');
  }
  function cancelXemPhieu() {
    $('#tabXemPhieu').addClass('d-lg-none');
    $('#bordered-justified-profile').addClass('active');
    $('#bordered-justified-profile').addClass('show');
  }


  function loadTableLichSuNhap() {
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
      url: "/loadTableLichSuNhap",
      data: "toDay=" + toDay + "&fromDay=" + fromDay,
      success: function (result) {
        $('#tableLichSuNhap').replaceWith(result);
      },
      error: function () {
        showToast('Thất bại!', 500);
      }
    });
  }

