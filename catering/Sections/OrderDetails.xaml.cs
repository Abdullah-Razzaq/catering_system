using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace Wpf_Catering_Db_system.Sections
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Page
    {
        public OrderDetails()
        {
            InitializeComponent();
            getOrders();
        }

        public void getOrders()
        {

            try
            {
                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");

                con.Open();

                SqlCommand command = new SqlCommand("GetOrderDetails", con);

                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                sd.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    OrdersGridTable.ItemsSource = dt.DefaultView;
                }
                else
                {
                    OrdersGridTable.ItemsSource = null; // Clear the grid when there's no data
                    MessageBox.Show("No data yet");
                }

                con.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


    }
}
