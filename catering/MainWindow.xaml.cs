using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
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
using Wpf_Catering_Db_system.Sections;

namespace Wpf_Catering_Db_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainSection.Content = new Menus();
        }

        //private string connectionString = "Data Source=ENIIM\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True";

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    isMaximized = true;
                }
            }
        }

        private void MenuSectionButton_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Content = new Menus();
            Order_form.allMenuItems.Clear();
        }

        private void CustomerSectionButton_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Content = new Customers();
            Order_form.allMenuItems.Clear();
        }

        private void OrderFormButton_Click(object sender, RoutedEventArgs e)
        {

            Page formPage = (Application.Current as App)?.m_window as Order_form;
            formPage = new Order_form();
            var app = (App)Application.Current;
            app.m_window = formPage;

            MainSection.Content = formPage;


            //MainSection.Content = (Application.Current as App)?.m_window as Order_form;


        }

        private void ordersListButton_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Content = new order_management();
            Order_form.allMenuItems.Clear();
        }

        private void TransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Content = new Transactions();
            Order_form.allMenuItems.Clear();

        }



       

    }
}