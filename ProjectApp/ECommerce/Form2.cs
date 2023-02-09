using ECommerce.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerce
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Report2WithSqlConn()
        {
            string query = @"SELECT TOP (1000) em.[orderId]
                  ,em.[quantity]
                  ,em.[stock]
                  ,em.[orderDate]                  
                  ,em.[productPic]
                  ,dg.[categoryName] categoryId
                  ,pr.[productName] productId
                  ,cu.[customerName] customerId
              FROM [AdoProject].[dbo].[orderDetails] em
              left join categories dg on em.categoryId=dg.categoryId
              left join product pr on em.productId=pr.productId
              left join customer cu on em.customerId=cu.customerId WHERE em.[orderId] = " + Form1.orderId;
            string connectionString = "server=DESKTOP-03VU7SV;Initial Catalog=AdoProject;Integrated Security=True;";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, "orderDetails");
            for (var i = 0; i < ds.Tables["orderDetails"].Rows.Count; i++)
            {
                if (ds.Tables["orderDetails"].Rows[i]["productPic"] != null)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["orderDetails"].Rows[i]["productPic"].ToString()))
                    {
                        string strFilePath = Application.StartupPath + ds.Tables["orderDetails"].Rows[i]["productPic"].ToString();
                        if (File.Exists(strFilePath))
                        {
                            ds.Tables["orderDetails"].Rows[i]["productPic"] = strFilePath;
                        }
                    }
                }
            }

            CrystalReport1 cr2 = new CrystalReport1();
            cr2.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr2;
            con.Close();
            crystalReportViewer1.Refresh();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Report2WithSqlConn();
        }
    }
}
