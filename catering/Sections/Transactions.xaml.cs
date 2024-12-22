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
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : Page
    {
        public Transactions()
        {
            InitializeComponent();
            getTransactions();
        }

        public void getTransactions()
        {

            try
            {
                SqlConnection con = new SqlConnection("Data Source=ENIIM\\AZAZ;Initial Catalog=AlamCaterers;Integrated Security=True;Trust Server Certificate=True");

                con.Open();

                SqlCommand command = new SqlCommand("GetTransactions", con);
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                }

                SqlDataAdapter sd = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                sd.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    TransactionsGrid.ItemsSource = dt.DefaultView; // Bind data only if rows exist
                }
                else
                {
                    TransactionsGrid.ItemsSource = null; // Clear the grid when there's no data
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
