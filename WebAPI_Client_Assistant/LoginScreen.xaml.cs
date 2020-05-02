using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace WebAPI_Client_Assistant
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connect = new SqlConnection(@"Data Source=(localdb)\mssqllocaldb;Database=ClientLogin;Initial Catalog=ClientLogin; Integrated Security=True;");
            try
            {
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    connect.Open();
                }
                string query = "SELECT COUNT(1) FROM [Table] WHERE UserName=@Username AND Password=@Password";
                SqlCommand command = new SqlCommand(query, connect);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Username", userName.Text);
                command.Parameters.AddWithValue("@Password", passWord.Password);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow mainwin = new MainWindow(userName.Text.ToString());
                    mainwin.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect Credentials!");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }
    }
}
