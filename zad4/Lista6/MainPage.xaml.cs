using Lista6.UserContollPanel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        
        bool MusicIsPlaying = false;
        SearchPanel sp;
        PlaylistUserControll puc = new PlaylistUserControll();
        HomePageUserContorl hp = new HomePageUserContorl();
        AppOperator ao;
        private MediaPlayer mp = new MediaPlayer();
        DispatcherTimer dt = new DispatcherTimer();
        
        public MainPage()
        {
            InitializeComponent();
            ao = new AppOperator();
            sp = new SearchPanel(ao,this);
            this.DataContext = ao;
            UserControlGird.Children.Add(hp);
            if (File.Exists("D://test.xml"))
            {
                Music music = Serialization.DeserializeToObject<Music>("D://test.xml");
                ao.MusicTileAndAuthor[1] = music.MusicAuthors;
                ao.MusicTileAndAuthor[0] = music.MusicTitle;
            }
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

        private void VolumeBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(VolumeBar).X;
            this.VolumeBar.Value = SetProgressBarValue(MousePosition);
            mp.Volume = VolumeBar.Value/100;
            //if (VolumeBar.Value < 5)
            //{
            //    bmpImg.UriSource = new Uri("C:\\Users\\mateu\\source\\repos\\Test\\zad4\\Lista6\\img\\mute.png");
            //}
            //else
            //{
            //    bmpImg.UriSource = new Uri("C:\\Users\\mateu\\source\\repos\\Test\\zad4\\Lista6\\img\\unmute.png");
            //}
        }

        private double SetProgressBarValue(double MousePosition)
        {
            double ratio = MousePosition / VolumeBar.ActualWidth;
            double ProgressBarValue = ratio * VolumeBar.Maximum;
            return ProgressBarValue;
        }

        public void MediaOpener()
        {
           
            mp = new MediaPlayer();
            mp.Open(new Uri("D:\\test.mp3"));
            PlayPauseBtn.Content = "||";
            MusicIsPlaying = true;
            mp.Play();
            dt.Start();
        }

        public void MediaPause()
        {
            PlayPauseBtn.Content = "▶";
            MusicIsPlaying = false;
            mp.Pause();
            dt.Stop();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Music music = new Music(ao.MusicTileAndAuthor[1], ao.MusicTileAndAuthor[0]);
            Serialization.SerializeToXml(music, "D:/test.xml");
        }
    }
}