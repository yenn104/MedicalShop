using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class FunctionModel
  {
    public int? Idmenu { get; set; }
    public int? Idvt { get; set; }
    public bool? Import { get; set; }
    public bool? Update { get; set; }
    public bool? Delete { get; set; }
    public bool? Print { get; set; }
    public bool? Export { get; set; }
    public bool? Person { get; set; }
  }
}
