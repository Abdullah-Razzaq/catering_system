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
    /// Interaction logic for Menus.xaml
    /// </summary>
    /// 
    public partial class Menus : Page
    {
        public Menus()
        {
            InitializeComponent();
            getMenus();
        }

        public void getMenus()
        {

            try
            {
                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");

                con.Open();

                SqlCommand command = new SqlCommand("exec GetAllMenus", con);

                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                sd.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MenuGridTable.ItemsSource = dt.DefaultView; // Bind data only if rows exist
                }
                else
                {
                    MenuGridTable.ItemsSource = null; // Clear the grid when there's no data
                    MessageBox.Show("No data yet");
                }
                //MessageBox.Show($"Rows retrieved: {dt.Rows.Count}");


                con.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void addNewMenuButton_Click(object sender, RoutedEventArgs e)
        {
            menu_window menu_Window = new menu_window(this); //passing the mainWindow class to the dialogBox constructor to refresh grid
            menu_Window.Show();

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag is DataRowView dataRowView)
            {
                int menuId = (int)dataRowView["ID"];
                string menuName = (string)dataRowView["MenuName"].ToString();
                string Category = (string)dataRowView["Category"].ToString();
                decimal price = (decimal)dataRowView["Price"];
                string Description = (string)dataRowView["Description"].ToString();

                menu_window menu_Window = new menu_window(this, true, menuId, menuName, Category, price, Description); //passing the mainWindow class to the dialogBox constructor to refresh grid
                menu_Window.Show();

                Console.WriteLine(menuId);
                Console.WriteLine(menuName);
                Console.WriteLine(Category);
                Console.WriteLine(Description);
                Console.WriteLine(price);
            }



            //menu_window menuWindow = new menu_window(this ,selectedMenu.menuName);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                int menuId = (int)button.Tag;

                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("DeleteMenu", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                MessageBox.Show($"Deleted Row: {menuId}", "Deletion Success", MessageBoxButton.OK, MessageBoxImage.Information);

                cmd.Parameters.AddWithValue("@ID", menuId);
                cmd.ExecuteNonQuery();

                getMenus();
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.Message);
                }



            }
        }


    }
}
