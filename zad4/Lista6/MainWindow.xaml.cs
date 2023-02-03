using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppOperator ao = new AppOperator();
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {          
            SqlDataReader dataReader = ao.loginFun(textboxlogin.Text);
            if (dataReader.Read())
            {
                PasswordHashMethods phw = new PasswordHashMethods();
                if (phw.CompareHashedPasswords(pbHaslo.Password, (string)dataReader.GetValue(1), (string)dataReader.GetValue(2)))
                {
                    dataReader.Close();
                    MainPage mp = new MainPage();
                    this.Hide();
                    mp.Show();
                }
                else
                {
                    MessageBox.Show("Niepoprawny login lub haslo");

                }
            }
            else
            {
                MessageBox.Show("Niepoprawny login lub haslo");
            }
            ao.Close();
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
            
            SignUpPage sup = new SignUpPage();
            sup.ShowDialog();            
        }
      
    }
}
