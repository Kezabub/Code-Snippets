using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
namespace Pie_Chart_Windows_Forms_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Fetch the Statistical data from database.
            string query = "SELECT ShipCity, COUNT(OrderId) [Total]";
            query += " FROM Orders WHERE ShipCountry = 'Brazil'";
            query += " GROUP BY ShipCity";
            DataTable dt = GetData(query);

            //Get the names of Cities.
            string[] x = (from p in dt.AsEnumerable()
                          orderby p.Field<string>("ShipCity") ascending
                          select p.Field<string>("ShipCity")).ToArray();

            //Get the Total of Orders for each City.
            int[] y = (from p in dt.AsEnumerable()
                       orderby p.Field<string>("ShipCity") ascending
                       select p.Field<int>("Total")).ToArray();

            Chart1.Series[0].ChartType = SeriesChartType.Pie;
            Chart1.Series[0].Points.DataBindXY(x, y);
            Chart1.Legends[0].Enabled = true;
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
        }

        private static DataTable GetData(string query)
        {
            string constr = @"Data Source=.\SQL2005;Initial Catalog=Northwind;User ID=sa;Password=pass@123";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
