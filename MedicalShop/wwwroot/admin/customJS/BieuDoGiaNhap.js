var _labelsTD = [];
var _valuesTD = [];
var _myChartTD = null;
document.addEventListener("DOMContentLoaded", function () {
  BieuDoGiaNhap();

});
function BieuDoGiaNhap() {
  $.ajax({
    url: 'BieuDoGiaNhap',
    method: 'POST',
    data: "TuNgay=" + $('#NgaySX').val() + "&DenNgay=" + $('#HanDung').val() + "&idHH=" + $('#selectHHX').val(),
    success: function (data) {
      console.log(data);
      _labelsTD = [];
      _valuesTD = [];
      // Chuyển dữ liệu từ JSON sang mảng để cấu hình đồ thị
      data.forEach(function (item, i) {
        _labelsTD.push(item.label); // Định dạng ngày tháng
        _valuesTD.push(item.value);
      });
      // Xóa biểu đồ cũ trước khi vẽ biểu đồ mới
      if (_myChartTD !== null) {
        _myChartTD.destroy();
      }
      renderDoThi(_labelsTD, _valuesTD);
    },
    error: function (error) {
      console.log(error);
    }
  });
}

function renderDoThi(labels, values) {
  var ctx = document.getElementById('myChart').getContext('2d');
  console.log(labels);
  var doanhThuData = {
    labels: labels,
    datasets: [{
      label: 'Nhà cung cấp',
      data: values,
      backgroundColor: 'rgba(0, 128, 255, 0.8)',
      borderColor: 'rgba(75, 192, 192, 1)',
      borderWidth: 1
    }]
  };
  _myChartTD = new Chart(ctx, {
    type: 'bar',
    data: doanhThuData,
    options: {
      scales: {
        y: {
          beginAtZero: true,
          ticks: {
            callback: function (value, index, values) {
              return value; // Định dạng các giá trị trên trục y thành kiểu tiền tệ
            }
          }
        }
      }
    }
  });
}
function getRandomColor() {
  var letters = '0123456789ABCDEF';
  var color = '#';
  for (var i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}