function showToast(message, statusCode) {
  var backgrounColor;
  document.getElementById('toast').className = 'toast align-items-center text-white border-0 position-fixed top-0 end-0 p-3';
  $("#toastContent").text(message);
  if (statusCode === 200) {
    backgrounColor = "bg-success";
    $("#toast").addClass(backgrounColor);
  } else {
    if (statusCode === 500) {
      backgrounColor = "bg-danger";
      $("#toast").addClass(backgrounColor);
    } else {
      backgrounColor = "bg-warning";
      $("#toast").addClass(backgrounColor);
    }
  }
  $("#toast").show();
  setTimeout(function () {
    $("#toast").hide();
  }, 2000);
}


function formatMoney(amount) {
  // Nếu giá trị là 0.00, trả về chuỗi "0", ngược lại sử dụng định dạng tiền tệ
  return amount === 0 ? "0" : amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

//function toDecimal(str) {
//  return parseFloat(str).toLocaleString('en-US', {
//    style: 'decimal',
//    maximumFractionDigits: 2,
//    minimumFractionDigits: 2
//  });
//}

function toDecimal(str) {
  const numberValue = parseFloat(str);

  // Kiểm tra nếu giá trị là NaN hoặc là 0.00
  if (isNaN(numberValue) || numberValue === 0) {
    return "0";
  }

  // Chuyển đổi số thành chuỗi với định dạng decimal
  const formattedNumber = numberValue.toLocaleString('en-US', {
    style: 'decimal',
    maximumFractionDigits: 2,
    minimumFractionDigits: 2
  });

  return formattedNumber;
}

//function toDecimal(str) {
//  return parseFloat(str).toLocaleString('en-US', {
//    style: 'decimal',
//    maximumFractionDigits: 2,
//    minimumFractionDigits: 2
//  });
//}


function formatDay(data) {
  return new Date(data.substring(3, 5) + "-" + data.substring(0, 2) + "-" + data.substring(6, 10));
}


function formatDateTimeShort(data) {
  return new Date(data.substring(3, 5) + "-" + data.substring(0, 2) + "-" + data.substring(6, 10));
}


// Chuyển số thành chuỗi và định dạng để có dấu phẩy phân tách hàng nghìn
function formatNumbers(number) {
  if (number != "" || number != null) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  } else {
    return '';
  }
}


function getValueNumbers(str) {
  return Number(str.replace(/,/g, ''));
}

function getValue(str) {
  return Number(str.replace(/[^0-9.-]+/g, ""));
}


function configDateDefault() {
  var today = new Date();
  $('.input-date-default').datetimepicker({
    locale: 'vi',
    useStrict: true,
    defaultDate: today,
    format: "DD-MM-yyyy",
    extraFormats: ["DD-MM-yyyy", "DD/MM/yyyy", "yyyy"],
    icons: {
      date: "ti ti-calendar",
      up: "ti ti-chevron-up",
      down: "ti ti-chevron-down",
      previous: 'ti ti-chevron-left',
      next: 'ti ti-chevron-right',
      time: "ti ti-alarm"
    },
    keyBinds: {
      left: null,
      right: null,
    }
  });
}


function configDateTime() {
  $('.input-date-time').datetimepicker({
    locale: 'vi',
    useStrict: true,
    format: "DD-MM-yyyy HH:mm",
    extraFormats: ["DD-MM-yyyy HH:mm", "DD/MM/yyyy HH:mm", "yyyy"],
    icons: {
      date: "ti ti-calendar",
      up: "ti ti-chevron-up",
      down: "ti ti-chevron-down",
      previous: 'ti ti-chevron-left',
      next: 'ti ti-chevron-right',
      time: "ti ti-alarm"
    },
    keyBinds: {
      left: null,
      right: null,
    }
  });
}



function configDateShortMask() {
  $('.input-date-short-mask').inputmask({ alias: "datetime", inputFormat: 'dd-mm-yy', placeholder: '__-__-__' });
}

function configDateLongMask() {
  $('.input-date-long-mask').inputmask({ alias: "datetime", inputFormat: 'dd-mm-yyyy', placeholder: '__-__-__' });
}


//$(document).on('focus', '.input-date-default, .input-date, .input-date-long-mask, .input-date-short-mask, .input-date-time-short-mask, .input-time-short-mask', function () {
$(document).on('focus', '.input-date-short-mask, .input-date-long-mask', function () {
  if ($(this).val() != "") {
    this.setSelectionRange(0, 2);
  }
})


$(document).on('click', '.input-date-default, .input-date, .input-date-long-mask, .input-time-short-mask', function () {
  var index = this.selectionStart
  if (index >= 0 && index <= 2) {
    this.setSelectionRange(0, 2);
  } else if (index >= 3 && index <= 5) {
    this.setSelectionRange(3, 5);
  }
  else if (index >= 3 && index <= 5) {
    this.setSelectionRange(3, 5);
  }
  else if (index >= 6 && index <= 10) {
    this.setSelectionRange(6, 10);
  }
})
$(document).on('input', '.input-date-default, .input-date, .input-date-long-mask, .input-time-short-mask', function () {
  if (this.selectionStart == 2) {
    this.setSelectionRange(3, 5);
  }
  if (this.selectionStart == 5) {
    this.setSelectionRange(6, 10);
  }
})
$(document).on('keydown', '.input-date-default, .input-date, .input-date-long-mask, .input-time-short-mask', function (event) {
  var input = this;
  if (event.key === 'ArrowLeft') {
    var index = input.selectionStart;
    if (index === 3) {
      input.setSelectionRange(0, 2);
      event.preventDefault();
    }
    else if (index === 6) {
      input.setSelectionRange(3, 5);
      event.preventDefault();
    }

  }
  if (event.key === 'ArrowRight') {
    var index = input.selectionEnd;

    if (index === 2) {
      input.setSelectionRange(3, 5);
      event.preventDefault();
    }
    else if (index === 5) {
      input.setSelectionRange(6, 10);
      event.preventDefault();
    }
  }
})


function formatDate(inputDate) {
  // Tạo một đối tượng Date từ chuỗi ngày tháng
  const date = new Date(inputDate);

  // Lấy các thành phần ngày, tháng, năm
  const day = date.getDate();
  const month = date.getMonth() + 1; // Tháng bắt đầu từ 0
  const year = date.getFullYear();
  // Định dạng lại thành chuỗi "dd-mm-yyyy"
  const formattedDate = `${formatNumber(day)}-${formatNumber(month)}-${year}`;
  return formattedDate;
}

// Hàm để thêm số 0 phía trước nếu số chỉ có một chữ số
function formatNumber(number) {
  return number.toString().padStart(2, '0');
}



//function formatNumberInput() {
//  // Áp dụng inputmask cho các phần tử có lớp 'formatted-number' và giá trị khác 0
//  $(".formatted-number").each(function () {
//    var value = $(this).val();
//    console.log(value);
//    $(this).inputmask({
//      alias: "numeric",
//      groupSeparator: ",",
//      autoGroup: true,
//      digits: 0,
//      allowMinus: false,
//      digitsOptional: false,
//      // Định dạng đặc biệt nếu giá trị là 0
//      onBeforeMask: function (value, opts) {
//        if (value === "0") {
//          return "0\\";
//        }
//        return value;
//      },
//    });

//  });
//}



function formatNumberInput() {
  // Áp dụng inputmask cho các phần tử có lớp 'formatted-number' và giá trị khác 0
  $(".formatted-number").each(function () {
    var value = $(this).val();

    $(this).inputmask({
      alias: "numeric",
      groupSeparator: ",",
      autoGroup: true,
      digits: 0,
      allowMinus: false,
      digitsOptional: false,
      // Định dạng đặc biệt nếu giá trị là 0
      onBeforeMask: function (value, opts) {
        if (value === "0") {
          return "0\\";
        }
        return value;
      },
    });

  });
}


function formatFloat() {
  $(".formatted-number-float").inputmask({
    alias: "numeric",
    radixPoint: ".",
    groupSeparator: ",",
    autoGroup: true,
    digits: 2,
    digitsOptional: true,
    allowMinus: false,
    prefix: "",
  });
}
function formatFloatInput() {
  $(".formatted-number-float").each(function () {
    var value = $(this).val();
    if (value !== "0") {
      $(this).inputmask({
        alias: "numeric",
        groupSeparator: ",",
        autoGroup: true,
        digits: 2,
        allowMinus: false,
        placeholder: '0',
        digitsOptional: true,
        // Định dạng đặc biệt nếu giá trị là 0
        onBeforeMask: function (value, opts) {
          if (value === "0") {
            return "0\\";
          }
          console.log(value);
          return value;
        },
      });
    }
  });
}


function formatNumberFloatWithElement(inputs) {
  inputs.each(function () {
    var value = $(this).val();
    if (value !== "0") {
      $(this).inputmask({
        alias: "numeric",
        groupSeparator: ",",
        autoGroup: true,
        digits: 0,
        allowMinus: false,
        placeholder: '0',
        digitsOptional: false,
        // Định dạng đặc biệt nếu giá trị là 0
        onBeforeMask: function (value, opts) {
          if (value === "0") {
            return "0\\";
          }
          return value;
        },
      });
    }
  });
}
// format lại số lúc nhập thành dạng 100,000,000.00
function formatNumberWithElement(inputs) {
  inputs.each(function () {
    var min = $(this).attr('min');
    var input = $(this).inputmask({
      alias: "numeric",
      radixPoint: ".",
      groupSeparator: ",",
      autoGroup: true,
      digits: 2,
      digitsOptional: true,
      allowMinus: false,
      prefix: "",
      min: min ? parseFloat(min) : 0
    });
    input.on("blur", function () {
      $(this).trigger('keyup');
    });
  });
}

function formatNumberWithElementDk(inputs) {
  inputs.each(function () {

    $(this).inputmask({
      alias: "numeric",
      radixPoint: ".",
      groupSeparator: ",",
      autoGroup: true,
      digits: 2,
      digitsOptional: true,
      allowMinus: false,
      prefix: "",
      placeholder: '0',
      min: 100
    });
  });
}
// 1,000,000
function formatEvenNumber(number) {
  if (number == null) return "";
  return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}