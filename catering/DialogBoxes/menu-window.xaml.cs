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
using System.Windows.Shapes;
using System.Data;
using Wpf_Catering_Db_system.Sections;

namespace Wpf_Catering_Db_system.DialogBoxes
{
    /// <summary>
    /// Interaction logic for menu_window.xaml
    /// </summary>
    public partial class menu_window : Window
    {
        private Menus _mainWindow;
        bool isUpdateMode = false;
        private int menuId;
        public menu_window(Menus mainWindow, bool _isUpdateMode = false, int _menuId = -1, string menuName = "", string category = "", decimal price = -1, string description = ""   ) //receiving this when the dialogBox is opened
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            isUpdateMode = _isUpdateMode;
            menuId = (int)_menuId;

            if(_isUpdateMode)
            {
                MenuNameTextBox.Text = menuName;
                CategoryTextBox.Text = category;
                PriceTextBox.Text = price.ToString();
                DescriptionTextBox.Text = description;

                SubmitButton.Content = "Update";
            }
        }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                string menuName = MenuNameTextBox.Text;
                string Category = CategoryTextBox.Text;
                decimal price; //if parsed, stored in price, if not then error
                string description = DescriptionTextBox.Text;

                if(string.IsNullOrWhiteSpace(menuName) || string.IsNullOrWhiteSpace(Category) || string.IsNullOrWhiteSpace(description) || !decimal.TryParse(PriceTextBox.Text, out price))
                {
                    MessageBox.Show("Please Enter Valid data", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            
                try
                {
                    SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
                    SqlCommand command;
                    if (isUpdateMode)
                    {
                        command = new SqlCommand("UpdateMenu", con);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }
                        command.Parameters.AddWithValue("@ID", menuId);
                    }
                    else
                    {
                        command = new SqlCommand("AddMenu", con);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }

                    }

                    command.Parameters.AddWithValue("@MenuName", menuName);
                    command.Parameters.AddWithValue("@Category", Category);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Description", description);

                    con.Open();
                    command.ExecuteNonQuery();

                    _mainWindow.getMenus(); //refreshing grid
                    this.Close();

                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        
            }
    }

    

}
