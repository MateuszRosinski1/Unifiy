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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sc = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True");
            sc.Open();
            SqlCommand com;
            SqlDataReader dataReader;
            string sqlcom = "Select login,password,salt From UserData Where login = '" + textboxlogin.Text + "'"; 
            com = new SqlCommand(sqlcom,sc);
            dataReader = com.ExecuteReader();
            if (dataReader.Read())
            {
                PasswordHashMethods phw = new PasswordHashMethods();
                byte[] bytehashedPassword = phw.GetHash(pbHaslo.Password, (string)dataReader.GetValue(2));
                string hashedPassword = Convert.ToBase64String(bytehashedPassword);
                if(hashedPassword == (string)dataReader.GetValue(1))
                {
                    sc.Close();
                    com.Dispose();
                    dataReader.Close();
                    MainPage mp = new MainPage();
                    this.Hide();
                    mp.Show();
                }
            }
            else
            {
                MessageBox.Show("Uzytkownik o takim loginie nie istnieje");
            }
            sc.Close();
        }

        private void MinimalizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximalizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Label_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            SignUpPage sup = new SignUpPage();
            sup.Show();
            

        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

    
    }
}
