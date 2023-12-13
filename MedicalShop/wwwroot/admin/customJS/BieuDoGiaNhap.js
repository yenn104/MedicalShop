document.addEventListener("DOMContentLoaded", function () {
  $.ajax({
    url: '/BieuDoGiaNhap',
    data: {
      idHH: 1,
      TuNgay: '01-12-2022',
      DenNgay: '02-01-2024',
    },
    method: 'POST',
    success: function (data) {
      console.log(data);
      renderDoThi(data)
    },
    error: function (error) {
      console.log(error);
    }
  });

});
function renderDoThi(data) {
  // Dữ liệu từ đoạn mã JSON bạn cung cấp

  var dataArray = data[0];
  var labels = data.label;
  console.log(data);
  var datasets = data.nccs.map(ncc => ({
    label: `Nhà cung cấp ${ncc[0].ncc}`,
    data: ncc.map(entry => entry.gia),
    fill: false,
    borderColor: getRandomColor()
  }));


  // Tạo biểu đồ đường sử dụng Chart.js
  var ctx = document.getElementById('myChart').getContext('2d');
  var myChart = new Chart(ctx, {
    type: 'line',
    data: {
      labels: labels,
      datasets: datasets
    },
    options: {
      responsive: true,
      plugins: {
        title: {
          display: true,
          text: 'Biểu đồ giá vốn theo ngày của các nhà cung cấp'
        },
      },
      scales: {
        x: {
          display: true,
          title: {
            display: true,
            text: 'Ngày'
          }
        },
        y: {
          display: true,
          title: {
            display: true,
            text: 'Giá vốn'
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