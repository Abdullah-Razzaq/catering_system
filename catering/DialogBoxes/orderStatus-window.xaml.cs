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
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using Wpf_Catering_Db_system.Sections;

namespace Wpf_Catering_Db_system.DialogBoxes
{
    /// <summary>
    /// Interaction logic for orderStatus_window.xaml
    /// </summary>
    /// 
    public partial class orderStatus_window : Window
    {
        public Orders ordersGrid;
        private int orderId;
        public orderStatus_window(Orders _ordersGrid, int _orderId)
        {
            ordersGrid = _ordersGrid;
            orderId = _orderId;
            InitializeComponent();
            
        }

        

        private void updateStatus_Click(object sender, RoutedEventArgs e)
        {


            ComboBoxItem selectedStatusObj = (ComboBoxItem)StatusComboBox.SelectedItem;

            string selectedStatus = selectedStatusObj.Content.ToString();

            //MessageBox.Show(selectedStatus.Content.ToString());

            if (string.IsNullOrWhiteSpace(selectedStatus))
            {
                MessageBox.Show("Please Enter Valid data", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
                SqlCommand command;
                
                command = new SqlCommand("UpdateOrderStatus", con);
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                command.Parameters.AddWithValue("@Status", selectedStatus);
                command.Parameters.AddWithValue("@OrderID", orderId);
               

                con.Open();
                command.ExecuteNonQuery();

                ordersGrid.getOrders(); //refreshing grid
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
    }
}
