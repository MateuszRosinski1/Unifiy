using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lista6.UserContollPanel;

namespace Lista6
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        bool MusicIsPlaying = false;
        SearchPanel sp = new SearchPanel();
        PlaylistUserControll puc = new PlaylistUserControll();
        HomePageUserContorl hp = new HomePageUserContorl();
        AppOperator ao = new AppOperator();
        public MainPage()
        {
            InitializeComponent();
           
            UserControlGird.Children.Add(hp);
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
            }
            else
            {
                btn.Content = "▶";
                MusicIsPlaying= false;  
            }
        }
    }
}
