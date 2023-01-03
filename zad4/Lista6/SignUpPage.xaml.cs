using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Window
    {
      

        public SignUpPage()
        {
            InitializeComponent();
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

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if(CheckTextBoxes() && 
               IsEmail(textboxemail.Text) &&
               UniqeLogin() &&
               UniqeEmail() &&
               PaswordEquals()) {

                SqlConnection sc = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True");
                sc.Open();
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                PasswordHashMethods phw = new PasswordHashMethods();
                string salt = phw.GenerateSalt();
                byte[] bytehashedPassword = phw.GetHash(PBHaslo.Password, salt);
                string hashedPassword = Convert.ToBase64String(bytehashedPassword);
                string sqlcommand = "Insert into UserData (login,password,salt,email) values('"+ tblogin.Text+"','"+hashedPassword+"','"+salt+"','" + textboxemail.Text+"')";
                command = new SqlCommand(sqlcommand, sc);
                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
                sc.Close();

            }
            else
            {
                MessageBox.Show("0");
            }
           
        }

        private bool PaswordEquals()
        {
            SecureString password = new SecureString();
            SecureString confimpassword = new SecureString();
            foreach(char c in PBHaslo.Password)
            {
                password.AppendChar(c);
            }

            foreach(char c in PbConfirmHaslo.Password)
            {
                confimpassword.AppendChar(c);
            }
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(password);
                bstr2 = Marshal.SecureStringToBSTR(confimpassword);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        private void AddUserToDataBase()
        {
            throw new NotImplementedException();
        }

        private bool CheckTextBoxes()
        {
            if(textboxemail.Text != string.Empty && 
               tblogin.Text != string.Empty && 
               PbConfirmHaslo.SecurePassword.Length != 0 &&
               PBHaslo.SecurePassword.Length != 0) {

                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool IsEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Podaj poprawny adres e-mail");
                return false;
            }
        }

        private bool UniqeLogin()
        {
            SqlConnection sc = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True");
            sc.Open();
            string sqlcommand = "Select login From UserData Where login = ";
            sqlcommand += "'" + tblogin.Text + "'";
            SqlCommand command = new SqlCommand(sqlcommand,sc);
            SqlDataReader dataReader = command.ExecuteReader();
            
            if(dataReader.Read())
            {
                if ((string)dataReader.GetValue(0) == tblogin.Text)
                {
                    MessageBox.Show("Konto z tym loginem już istnieje");
                    dataReader.Close();
                    command.Dispose();
                    sc.Close();
                    return false;
                }
                else
                {
                    dataReader.Close();
                    command.Dispose();
                    sc.Close();
                    return true;
                }
            }
            else
            {
                dataReader.Close();
                command.Dispose();
                sc.Close();
                return true;
            }

        }

        private bool UniqeEmail()
        {
            SqlConnection sc = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True");
            sc.Open();
            string sqlcommand = "Select email From UserData Where email = ";
            sqlcommand += "'" + textboxemail.Text + "'";
            SqlCommand command = new SqlCommand(sqlcommand, sc);
            SqlDataReader dataReader = command.ExecuteReader();
            if(dataReader.Read())
            {
                if ((string)dataReader.GetValue(0) == textboxemail.Text)
                {
                    MessageBox.Show("Ten email jest już zajęty");
                    dataReader.Close();
                    command.Dispose();
                    sc.Close();
                    return false;
                }
                else
                {
                    dataReader.Close();
                    command.Dispose();
                    sc.Close();
                    return true;
                }
            }
            else
            {
                dataReader.Close();
                command.Dispose();
                sc.Close();
                return true;
            }

        }
    }
}
