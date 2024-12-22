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
using Wpf_Catering_Db_system.Types;
using System.Data;
using Wpf_Catering_Db_system.DialogBoxes;
using System.IO.Packaging;

namespace Wpf_Catering_Db_system.Sections
{
    /// <summary>
    /// Interaction logic for Order_form.xaml
    /// </summary>
    public partial class Order_form : Page
    {
        
        public static List<MenuItemType> allMenuItems = new List<MenuItemType>();
        public static List<TextBlock> globalCounts = new List<TextBlock>();
        public static TextBlock globalTextBlock;
        public Order_form()
        {
            InitializeComponent();
    
            List<Customer> customers = GetCustomersFromDB();

            DataContext = this;
            
            Console.WriteLine("Customers List:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.ID}, Name: {customer.Name}");
            }

            customersComboBox.ItemsSource = customers; 
        }

        public List<Customer> GetCustomersFromDB()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"))
            {
                con.Open();

                string query = "SELECT ID, NAME FROM Customers";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        
                    });
                }
            }

            return customers;
        }

        private void customersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)customersComboBox.SelectedValue;

            if (selectedCustomer != null)
            {
                // Debugging output
                Console.WriteLine($"Selected Customer ID: {selectedCustomer.ID}, Name: {selectedCustomer.Name}");

                MessageBox.Show($"Selected Customer ID: {selectedCustomer.ID}, Name: {selectedCustomer.Name}");
            }
            else
            {
                MessageBox.Show("No customer selected.");
            }
        }

        private void AddMenuItemsButton_Click(object sender, RoutedEventArgs e)
        {
            menuItems dialog = new menuItems();
            dialog.Show();
        }

       
        public void populateAddedProducts(MenuItemType menuItem)
        {
            allMenuItems.Add(menuItem);

            var existingTotalPriceBlock = mainStackPanel.Children.OfType<TextBlock>()
                .FirstOrDefault(tb => tb.Name == "TotalPriceAmount");

            int itemCount = menuItem.quantity;

            MessageBox.Show("Populating...Not null");

            DockPanel dockPanel = new DockPanel();
            dockPanel.Style = (Style)Application.Current.FindResource("AddMenusPanel");




            TextBlock dishName = new TextBlock();
            
            dishName.Text = $"  {menuItem.Name} - {menuItem.price}";
            dishName.Style = (Style)Application.Current.FindResource("AddMenusCaption");

            StackPanel stackPanel = new StackPanel();
            stackPanel.Style = (Style)Application.Current.FindResource("PlusMinusButtonsPanel");

            TextBlock TotalPriceAmount = new TextBlock();
            TotalPriceAmount.Text = $"Total: Rs {menuItem.price}";
            TotalPriceAmount.HorizontalAlignment = HorizontalAlignment.Right;
            TotalPriceAmount.Margin = new Thickness(0, 5, 10, 0);
            TotalPriceAmount.FontSize = 20;
            TotalPriceAmount.FontWeight = FontWeights.SemiBold;
            TotalPriceAmount.Name = "TotalPriceAmount";

            Button plusButton = new Button();
            plusButton.Content = "+";


            plusButton.Style = (Style)Application.Current.FindResource("PlusMinusButtons");

            TextBlock count = new TextBlock();
            count.Tag = menuItem.ID;

            globalCounts.Add(count);
            
            count.VerticalAlignment = VerticalAlignment.Center;
            count.HorizontalAlignment = HorizontalAlignment.Center;
            count.Text = $" {menuItem.quantity.ToString()} ";

            plusButton.Click += (sender, e) =>
            {
                count.Text = $" {itemCount.ToString()} ";
                
                menuItem.quantity++;
                count.Text = $" {menuItem.quantity.ToString()} ";

                decimal sum = 0;

                foreach (MenuItemType dish in allMenuItems)
                {
                    var itemPrice = dish.price * dish.quantity;
                    sum += itemPrice;

                }

                TotalPriceAmount.Text = $"Total: Rs {sum.ToString()}";

                //after first item is added, when we add second item, we find a reference to the 
                //total price text block at the start, after that we do create a text block instance but as
                //the total price text block already exists we dont add it to the ui, to update the ui for rows
                // > 1 we have to use the existing price text block which we found at the start

                if (existingTotalPriceBlock != null)
                {
                    existingTotalPriceBlock.Text = $"Total: Rs {sum.ToString()}";
                }
            };

            Button minusButton = new Button();
            minusButton.Content = "-";
            


            minusButton.Style = (Style)Application.Current.FindResource("PlusMinusButtons");

            minusButton.Click += (sender, e) =>
            {
                if (menuItem.quantity > 0)
                {
                    
                    count.Text = $" {itemCount.ToString()} ";

                   
                    menuItem.quantity--;
                    count.Text = $" {menuItem.quantity.ToString()} ";


                    decimal sum = 0;

                    foreach (MenuItemType dish in allMenuItems)
                    {
                        var itemPrice = dish.price * dish.quantity;
                        sum += itemPrice;

                    }

                    TotalPriceAmount.Text = $"Total: Rs {sum.ToString()}";

                    //after first item is added, when we add second item, we find a reference to the 
                    //total price text block at the start, after that we do create a text block instance but as
                    //the total price text block already exists we dont add it to the ui, to update the ui for rows
                    // > 1 we have to use the existing price text block which we found at the start

                    if (existingTotalPriceBlock != null)
                    {
                        existingTotalPriceBlock.Text = $"Total: Rs {sum.ToString()}";
                    }

                }
            };

            stackPanel.Children.Add(plusButton);
            stackPanel.Children.Add(count);
            stackPanel.Children.Add(minusButton);

            dockPanel.Children.Add(dishName);
            dockPanel.Children.Add(stackPanel);

            AddMenusPanel.Children.Add(dockPanel);

            

            if(existingTotalPriceBlock == null)
            {
                globalTextBlock = TotalPriceAmount;
                mainStackPanel.Children.Add(TotalPriceAmount);

            }
            else
            {
                decimal sum = 0;
                globalTextBlock = existingTotalPriceBlock;
                foreach(MenuItemType dish in allMenuItems)
                {
                    var itemPrice = dish.price * dish.quantity;
                    sum = sum + itemPrice;
                }

                existingTotalPriceBlock.Text = $"Total: Rs {sum.ToString()}";

            }
            



        }


        private void populateOrderDetails(int orderId)
        {
            SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            con.Open();

            
            foreach(MenuItemType item in allMenuItems)
            {

                SqlCommand cmd = new SqlCommand("AddOrderDetails", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                cmd.Parameters.AddWithValue("OrderID", orderId);
                cmd.Parameters.AddWithValue("MenuID", item.ID);
                cmd.Parameters.AddWithValue("@Quantity", item.quantity);
                cmd.Parameters.AddWithValue("@PriceAtTimeOfOrder", item.price);

                cmd.ExecuteNonQuery();

            }

            con.Close();

        }

        private void SubmitOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = customersComboBox.SelectedItem as Customer;

            decimal Totalprice = 0;

            foreach(MenuItemType dish in allMenuItems)
            {
                var itemPrice = dish.quantity * dish.price;
                Totalprice = Totalprice + itemPrice;
            }

            SqlConnection sqlConnection = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("AddOrder", sqlConnection);
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }

            sqlCommand.Parameters.AddWithValue("@CustomerId", customer.ID);
            sqlCommand.Parameters.AddWithValue("@TotalAmount", Totalprice);
            
            SqlParameter orderId = new SqlParameter("@OrderID", SqlDbType.Int);
            {
                orderId.Direction = ParameterDirection.Output;
            }

            MessageBox.Show($"{orderId}");

            sqlCommand.Parameters.Add(orderId);

            sqlCommand.ExecuteNonQuery();

            int finalOrderId = Convert.ToInt32(orderId.Value);
            MessageBox.Show($"{orderId.Value}");
            MessageBox.Show($"finalOrderId: {finalOrderId}");

            sqlConnection.Close();

            populateOrderDetails(finalOrderId);

        }


    }


}
