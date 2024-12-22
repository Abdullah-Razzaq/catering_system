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
using Wpf_Catering_Db_system.DialogBoxes;

namespace Wpf_Catering_Db_system.Sections
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
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

                SqlCommand command = new SqlCommand("GetOrders", con);

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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int orderId = (int)button.Tag;

                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteOrder", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                MessageBox.Show($"Deleted Row: {orderId}", "Deletion Success", MessageBoxButton.OK, MessageBoxImage.Information);

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.ExecuteNonQuery();

                getOrders();    
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.Message);
                }



            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int orderId = (int)button.Tag;
            orderStatus_window statusWindow = new orderStatus_window(this, orderId);
            statusWindow.Show();
        }
    }
}
