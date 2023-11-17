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

function toDecimal(str) {
  return parseFloat(str).toLocaleString('en-US', {
    style: 'decimal',
    maximumFractionDigits: 2,
    minimumFractionDigits: 2
  });
}


function formatDay(data) {
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