using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class HHNList
  {
    string connectionString = "Data Source=LAPTOP-J4VFA095;Initial Catalog=MedicalShop;Integrated Security=True";
    public DataTable getHangHoa()
    {
      var dt = new DataTable();

      // MedicalShopContext.Database.Connection.ConnectionString
      // MedicalShopContext.Database.GetDbConnection().ConnectionString
      //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
      //var MedicalShopContext = new MedicalShopContext(SqlConnection, true);

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlCommand cmd = new SqlCommand("Sto_HHNhap", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        conn.Close();
      }
      return dt;
    }
  }
}
