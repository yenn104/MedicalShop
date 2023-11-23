var imgList = [];
var imgAvt = null;
var imgTemp = [];

$(document).ready(function () {
  $(".upload-area").click(function () {
    $('#upload-input').trigger('click');
  });

  $('#upload-input').change(event => {
    if (event.target.files) {
      let filesAmount = event.target.files.length;
      $('.upload-img').html("");

      for (let i = 0; i < filesAmount; i++) {
        let reader = new FileReader();
        reader.onload = function (event) {
          let html = `
                        <div class = "uploaded-img">
                            <img src = "${event.target.result}">
                            <input type="radio" class="form-input-check checkAvt remove-btn" name="checkAvt" ${i == 0 ? "checked" : ""}>
                        </div>
                    `;
          $(".upload-img").append(html);
          imgList.push(event.target.result);
          if (i == 0) {
            imgAvt = event.target.result;
          } else {
            imgTemp.push(event.target.result);
          }
        }
        reader.readAsDataURL(event.target.files[i]);
      }

      $('.upload-info-value').text(filesAmount);
      $('.upload-img').css('padding', "20px");
    }
  });

  // $(window).click(function(event){
  //     if($(event.target).hasClass('remove-btn')){
  //         $(event.target).parent().remove();
  //     } else if($(event.target).parent().hasClass('remove-btn')){
  //         $(event.target).parent().parent().remove();
  //     }
  // })

  // <button type = "button" class = "remove-btn">
  //                            <i class = "fas fa-times"></i>
  //                        </button>

  // $(window).click(function (event) {
  //     if ($(event.target).hasClass('checkAvt')) {
  //         let removedFileSrc = $(event.target).parent().find('img').attr('src');
  //         imgAvt = imgList.filter(file => file === removedFileSrc);
  //         imgList = imgList.filter(file => file !== removedFileSrc);
  //     } else if ($(event.target).parent().hasClass('checkAvt')) {
  //         let removedFileSrc = $(event.target).parent().parent().find('img').attr('src');
  //         imgAvt = imgList.filter(file => file === removedFileSrc);
  //         imgList = imgList.filter(file => file !== removedFileSrc);
  //     }
  //  //   $('.upload-info-value').text(imgList.length);
  // });


  $(document).on('click', 'input[name=checkAvt]', function (event) {
    if ($(event.target).hasClass('checkAvt')) {
      let removedFileSrc = $(event.target).parent().find('img').attr('src');
      imgAvt = imgList.filter(file => file === removedFileSrc);
      imgTemp = imgList.filter(file => file !== removedFileSrc);
    } else if ($(event.target).parent().hasClass('checkAvt')) {
      let removedFileSrc = $(event.target).parent().parent().find('img').attr('src');
      imgAvt = imgList.filter(file => file === removedFileSrc);
      imgTemp = imgList.filter(file => file !== removedFileSrc);
    }
  });



});



$('#FormCreate').submit(function (event) {
  event.preventDefault(); // Ngăn chặn hành vi mặc định của form submit
    var form = document.getElementById('FormCreate');
    if (!form.checkValidity()) {
      event.preventDefault();
      event.stopPropagation();
      form.classList.add('was-validated');
    } else {
      //var formData = $('#FormCreate').find(':input[name!=checkAvt]').serializeArray();

      //const fileFormData = new FormData(); // Đặt tên biến khác

      //// Thêm các tệp đã chọn vào FormData
      //for (let i = 0; i < imgTemp.length; i++) {
      //  fileFormData.append("ImgAvt", imgTemp[i]);
      //}
      //console.log(imgAvt);
      //console.log(imgTemp);
      //modelMap = {
      //  HangHoa: formData,
      //  ImgAvt: imgAvt,
      //  ImgList: imgList
      //};
      formData.append('HangHoa', JSON.stringify(modelMap.HangHoa));
      formData.append('ImgAvt', modelMap.ImgAvt);
      for (var i = 0; i < modelMap.ImgList.length; i++) {
        formData.append('ImgList', modelMap.ImgList[i]);
      }
      console.log(formData);
      $.ajax({
        url: '/HangHoa/insertHangHoa', // Đường dẫn đến action xử lý form
        method: 'POST',
        //data: JSON.stringify(modelMap1),
        //formData: "application/json",
        data: modelMap, // Sử dụng biến khác
        processData: false,
        contentType: false,
        success: function (response) {
          showToast(response.message, response.statusCode);
          //if (response.statusCode == 200) {
          //  var data = response.model;
          //  var tr = getRowTableTU(data);
          //  formatNumberInput();
          //  $('#modal-largel').modal('hide');
          //}
        },
        error: function (xhr, status, error) {
          // Xử lý lỗi (nếu có) khi gửi form
          console.error(error);
        }
      });
    }
});



//function UpdateHangHoa() {
//    var form = $('#FormCreate');
//    const files = $("#fileInput")[0].files;
//    const fileFormData = new FormData(); // Đặt tên biến khác

//    // Thêm các tệp đã chọn vào FormData
//    for (let i = 0; i < files.length; i++) {
//      fileFormData.append("files", files[i]);
//    }

//    $.ajax({
//      url: "/HangHoa/insertHangHoa",
//      type: "POST",
//      data: fileFormData, // Sử dụng biến khác
//      processData: false,
//      contentType: false,
//      success: function (response) {
//        window.location.href = "/HangHoa";
//      },
//      error: function (error) {
//        console.error("Lỗi khi cập nhật hàng hóa: " + error);
//      }
//    });
//}