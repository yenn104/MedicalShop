using MedicalShop.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalShop.Models
{
  public class TonKhoList
  {
    string connectionString = "Data Source=LAPTOP-J4VFA095;Initial Catalog=MedicalShop;Integrated Security=True";
    public DataTable getTonKho()
    {
      var dt = new DataTable();
      
      // MedicalShopContext.Database.Connection.ConnectionString
      // MedicalShopContext.Database.GetDbConnection().ConnectionString
      //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
      //var MedicalShopContext = new MedicalShopContext(SqlConnection, true);

      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlCommand cmd = new SqlCommand("Sto_Nhap", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        conn.Close();
      }
      return dt;
    }

    //protected void GenerateReport()
    //{
    //  SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);

    //  conn.Open();
    //  SqlCommand comd;
    //  comd = new SqlCommand();
    //  comd.Connection = conn;


    //  comd.CommandType = CommandType.StoredProcedure;
    //  comd.CommandText = "Sto_Banhang02";
    //  comd.Parameters.Add("@Ngay", SqlDbType.Date);
    //  comd.Parameters[0].Value = "03/02/2022";


    //  SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //  sqlAdapter.SelectCommand = comd;

    //  DataSet Dset = new DataSet();

    //  sqlAdapter.Fill(Dset, "Dset");

    //  ReportDataSource MyDs = new ReportDataSource();
    //  MyDs.Name = "DataSetHangHoa";
    //  MyDs.Value = Dset.Tables[0];

    //  ReportViewer1.ProcessingMode = ProcessingMode.Local;
    //  ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/RPSale.rdlc");

    //  ReportViewer1.LocalReport.DataSources.Clear();
    //  ReportViewer1.LocalReport.DataSources.Add(MyDs);
    //  conn.Close();

    //}
  }
}
