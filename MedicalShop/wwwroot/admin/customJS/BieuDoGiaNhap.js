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



  const defaultLabels = ['Nhà cung cấp 1', 'Nhà cung cấp 2', 'Nhà cung cấp 3'];


  // If there is no data, create an empty dataset with default labels
  if (datasets.length === 0) {
    datasets.push({
      label: 'No Data',
      data: Array(defaultLabels.length).fill(0),
      fill: false,
      borderColor: 'rgba(0, 0, 0, 0)', // transparent color
      tension: 0
    });
  }

  // If there is no data, use default labels
  const chartLabels = formattedLabels.length > 0 ? formattedLabels : defaultLabels;




  let ctx = document.getElementById('myChart').getContext('2d');
  _myChartTD = new Chart(ctx, {
    type: 'line',
    data: {
      labels: chartLabels,
      datasets: datasets
    },
    options: {
      responsive: true,
      plugins: {
        title: {
          display: true,
          text: 'Biểu đồ giá theo nhà cung cấp'
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