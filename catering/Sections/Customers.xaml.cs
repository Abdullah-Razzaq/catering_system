using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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
using Wpf_Catering_Db_system.DialogBoxes;
using System.Data;

namespace Wpf_Catering_Db_system.Sections
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Page
    {
        public Customers()
        {
            InitializeComponent();
            GetCustomers();
        }

        private void addNewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            customer_window customerWindow = new customer_window(this);
            customerWindow.Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag is DataRowView dataRowView)
            {
                int customerId = (int)dataRowView["ID"];
                string customerName = (string)dataRowView["Name"].ToString();
                string PhoneNo = (string)dataRowView["Phone"].ToString();
                string Email = (string)dataRowView["Email"].ToString();
                string Address = (string)dataRowView["Address"].ToString();

                customer_window customerWindow = new customer_window(this, true, customerId, customerName, PhoneNo, Email, Address); 
                customerWindow.Show();

                Console.WriteLine(customerId);
                Console.WriteLine(customerName);
                Console.WriteLine(PhoneNo);
                Console.WriteLine(Email);
                Console.WriteLine(Address);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int customerId = (int)button.Tag;

                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteCustomer", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                MessageBox.Show($"Deleted Row: {customerId}", "Deletion Success", MessageBoxButton.OK, MessageBoxImage.Information);

                cmd.Parameters.AddWithValue("@ID", customerId);
                cmd.ExecuteNonQuery();

                GetCustomers();
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.Message);
                }



            }
        }

        public void GetCustomers()
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");
                con.Open();

                SqlCommand command = new SqlCommand("GetCustomers", con);
                {
                    command.CommandType = CommandType.StoredProcedure;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                
                if(dataTable.Rows.Count > 0)
                {
                    CustomerGridTable.ItemsSource = dataTable.DefaultView;
                }
                else
                {
                    CustomerGridTable.ItemsSource = null;
                    MessageBox.Show("No data yet");
                }

                con.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
