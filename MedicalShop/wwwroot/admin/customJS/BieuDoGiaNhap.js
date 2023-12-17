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
      if (_myChartTD !== null) {
        _myChartTD.destroy();
      }
      renderDoThi(data);
    },
    error: function (error) {
      console.log(error);
    }
  });
}
function renderDoThi(data) {
  let formattedLabels = data.labels.map(label => moment(label).format('DD-MM-YYYY'));
  let datasets = data.value.map((item, index) => {
    return {
      label: item[0].tenNCC,
      data: item.map(i => i.gia),
      fill: false,
      borderColor: `rgb(${Math.random() * 255}, ${Math.random() * 255}, ${Math.random() * 255})`,
      tension: 0.1
    };
  });

  let ctx = document.getElementById('myChart').getContext('2d');
  _myChartTD = new Chart(ctx, {
    type: 'line',
    data: {
      labels: formattedLabels,
      datasets: datasets
    },
    options: {
      responsive: true,
      plugins: {
        title: {
          display: true,
          text: 'Biểu đồ giá theo thời gian'
        },
        tooltip: {
          mode: 'index',
          intersect: false,
        },
        hover: {
          mode: 'nearest',
          intersect: true
        },
        scales: {
          x: {
            display: true,
            title: {
              display: true,
              text: 'Thời gian'
            }
          },
          y: {
            display: true,
            title: {
              display: true,
              text: 'Giá'
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