using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Lista6.UserContollPanel;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        bool MusicIsPlaying = false;
        byte[] bytes; 
        SearchPanel sp = new SearchPanel();
        PlaylistUserControll puc = new PlaylistUserControll();
        HomePageUserContorl hp = new HomePageUserContorl();
        AppOperator ao = new AppOperator();
        private MediaPlayer mp = new MediaPlayer();
        DispatcherTimer dt = new DispatcherTimer();
        
        public MainPage()
        {
            InitializeComponent();
            UserControlGird.Children.Add(hp);
            bytes = getMusic();
            File.WriteAllBytes("D:\\test.mp3", bytes);
            mp.Open(new Uri("D:\\test.mp3"));
            dt.Interval = TimeSpan.FromMilliseconds(100);
            dt.Tick += timer_Tick;

        }

        private void timer_Tick(object s,EventArgs e)
        {
            if(mp.Source != null)
            {
                lbStats.Content = String.Format(mp.Position.ToString(@"mm\:ss"));
                lbSoundTime.Content = String.Format(mp.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                MusicProgresBar.Value = (mp.Position.TotalSeconds * 100 )/mp.NaturalDuration.TimeSpan.TotalSeconds;
            }
            else
            {
                lbStats.Content = String.Empty;
                lbSoundTime.Content = String.Empty;
                MusicProgresBar.Value = 0;
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            UserControlGird.Children.Clear();
            UserControlGird.Children.Add(sp);


        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            UserControlGird.Children.Clear();
            UserControlGird.Children.Add(hp);
        }

        private void PlayListButton_Click(object sender, RoutedEventArgs e)
        {
            UserControlGird.Children.Clear();
            UserControlGird.Children.Add(puc);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (!MusicIsPlaying)
            {
                btn.Content = "||";
                MusicIsPlaying = true;
                mp.Play();
                dt.Start();
            }
            else
            {
                btn.Content = "▶";
                MusicIsPlaying= false;
                mp.Pause();
                dt.Stop();
            }
        }

        private byte[] getMusic()
        {
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True"))
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandText = "Select BLOBdata From BLOBMusic Where BLOBauthors = 'Slawomir'";
                cn.Open();
                return cm.ExecuteScalar() as byte[];
            }


        }

        private void VolumeBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(VolumeBar).X;
            this.VolumeBar.Value = SetProgressBarValue(MousePosition);
            mp.Volume = VolumeBar.Value/100;
            if (VolumeBar.Value < 5)
            {
                bmpImg.UriSource = new Uri("C:\\Users\\mateu\\source\\repos\\Test\\zad4\\Lista6\\img\\mute.png");
            }
            else
            {
                bmpImg.UriSource = new Uri("C:\\Users\\mateu\\source\\repos\\Test\\zad4\\Lista6\\img\\unmute.png");
            }
        }

        private double SetProgressBarValue(double MousePosition)
        {
            double ratio = MousePosition / VolumeBar.ActualWidth;
            double ProgressBarValue = ratio * VolumeBar.Maximum;
            return ProgressBarValue;
        }

    }
}
//using (var connection = new SqlConnection("Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True"))
//{
//    connection.Open();
//    using (var cmd = new SqlCommand("Select BLOBdata From BLOBMusic Where BLOBauthors = 'Slawomir'", connection))
//    {
//        using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess))
//        {
//            if (dr.FieldCount > 0)
//            {
//                while (dr.Read())
//                {
//                    var columnIndex = dr.GetOrdinal("BLOBmusic");
//                    var stream = dr.GetSqlBinary(columnIndex);
//                    var memStream = new MemoryStream(stream.Value);
//                    IRandomAccessStream randomAccessStream = memStream.AsRandomAccessStream();
//                    mp.Source = MediaSource.CreateFromStream(randomAccessStream, "audio/mpeg");
//                    mp.Play();
//                }
//            }
//        }
//    }
//}