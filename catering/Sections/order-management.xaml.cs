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
    /// Interaction logic for order_management.xaml
    /// </summary>
    public partial class order_management : Page
    {
        public order_management()
        {
            InitializeComponent();
            OrdersGrid.Content = new Orders();
        }


        

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.Content is OrderDetails)
            {
                OrdersGrid.Content = new Orders();
            }
        }

        private void DetailedOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if(OrdersGrid.Content is Orders)
            {
                OrdersGrid.Content = new OrderDetails();
            }
        }
    }
}
