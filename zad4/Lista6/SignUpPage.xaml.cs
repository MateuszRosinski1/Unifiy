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

        AppOperator ao = new AppOperator();
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
            this.Hide();
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

                PasswordHashMethods phw = new PasswordHashMethods();
                string salt = phw.GenerateSalt();
                byte[] bytehashedPassword = phw.GetHash(PBHaslo.Password, salt);
                string hashedPassword = Convert.ToBase64String(bytehashedPassword);
                ao.InsertUser(tblogin.Text,hashedPassword,salt,textboxemail.Text);               
            }
            else
            {
                PBHaslo.Clear();
                PbConfirmHaslo.Clear();
            }
           
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
            if(ao.UniqeLogin(tblogin.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Konto z tym loginem już istnieje");
                return false;
            }

        }

        private bool UniqeEmail()
        {
            if (ao.UniqeEmail(textboxemail.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Konto z tym e-mailem już istnieje");
                return false;
            }
        }

        private bool PaswordEquals()
        {
            SecureString password = new SecureString();
            SecureString confimpassword = new SecureString();
            foreach (char c in PBHaslo.Password)
            {
                password.AppendChar(c);
            }

            foreach (char c in PbConfirmHaslo.Password)
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
                        if (b1 != b2)
                        {
                            MessageBox.Show("Hasla sa od siebie rozne");
                            return false;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Hasla sa od siebie rozne");
                    return false;
                }
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }
}
