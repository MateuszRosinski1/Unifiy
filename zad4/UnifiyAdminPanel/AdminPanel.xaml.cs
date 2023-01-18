using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace UnifiyAdminPanel
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private byte[] bytes;
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        private void FindFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.InitialDirectory = "c:\\";
            ofp.Filter = "mp3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (ofp.ShowDialog() == true)
            {
                bytes = File.ReadAllBytes(ofp.FileName);
            }
        }

        private void AddMusic_Click(object s, RoutedEventArgs e)
        {
   
            SqlConnection sc = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("dbo.InsertMusic",sc);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@BLOBdata", System.Data.SqlDbType.VarBinary).Value = bytes;
            cmd.Parameters.Add("@BLOBauthors", System.Data.SqlDbType.VarChar,50).Value = textboxmusicauthors.Text;
            cmd.Parameters.Add("@BLOBtrackName", System.Data.SqlDbType.VarChar, 50).Value = textboxmusicname.Text;
            try
            {
                sc.Open();
                cmd.ExecuteNonQuery();
                sc.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }

        }

    }
}
