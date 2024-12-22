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
    /// Interaction logic for customer_window.xaml
    /// </summary>
    public partial class customer_window : Window
    {
        private Customers _mainWindow;
        bool isUpdateMode = false;
        private int customerId;
        public customer_window(Customers mainWindow, bool _isUpdateMode = false, int _customerId = -1, string customerName = "", string phone = "", string email = "", string address = "") //receiving this when the dialogBox is opened
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            isUpdateMode = _isUpdateMode;
            customerId = (int)_customerId;

            if (_isUpdateMode)
            {
                CustomerNameTextBox.Text = customerName;
                PhoneNoTextBox.Text = phone.ToString();
                EmailTextBox.Text = email;
                AddressTextBox.Text = address;

                SubmitButton.Content = "Update";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string customerName = CustomerNameTextBox.Text;
            string PhoneNo = PhoneNoTextBox.Text;
            string email = EmailTextBox.Text;
            string Address = AddressTextBox.Text;

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(PhoneNo) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("Please Enter Valid data", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-VV2UQIC\\ABDULLAH;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
                SqlCommand command;
                if (isUpdateMode)
                {
                    command = new SqlCommand("UpdateCustomer", con);
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    command.Parameters.AddWithValue("@ID", customerId);
                }
                else
                {
                    command = new SqlCommand("AddCustomer", con);
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }

                }

                command.Parameters.AddWithValue("@CustomerName", customerName); //the parameters should be same with the parameters defined in the procedure in ssms    
                command.Parameters.AddWithValue("@Phone", PhoneNo);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Address", Address);

                con.Open();
                command.ExecuteNonQuery();

                _mainWindow.GetCustomers(); //refreshing grid
                this.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

    }
}
