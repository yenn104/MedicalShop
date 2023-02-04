﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class Dvt
  {
    public Dvt()
    {
      HangHoa = new HashSet<HangHoa>();
      HhDvt = new HashSet<HhDvt>();
    }

    public int Id { get; set; }

    [DisplayName("Mã ĐVT")]
    public string MaDvt { get; set; }

    [DisplayName("Tên ĐVT")]
    public string TenDvt { get; set; }

    [DisplayName("Ngày tạo")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
    public DateTime? CreatedDate { get; set; }

    [DisplayName("NV tạo")]
    public int? CreatedBy { get; set; }

    [DisplayName("Ngày sửa")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("NV sửa")]
    public int? ModifiedBy { get; set; }

    [DisplayName("Chi Nhánh")]
    public int? Idcn { get; set; }

    [DisplayName("Trạng thái")]
    public bool? Active { get; set; }

    public virtual ICollection<HhDvt> HhDvt { get; set; }
    public virtual ICollection<HangHoa> HangHoa { get; set; }
  }
}
