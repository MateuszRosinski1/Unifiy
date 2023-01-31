using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Lista6.UserContollPanel
{
    /// <summary>
    /// Interaction logic for SearchPanel.xaml
    /// </summary>
    public partial class SearchPanel : UserControl
    {
        private string[] musicType = { "Pop", "Rap", "Rock", "K-pop", "Jazz", 
                                       "Trap", "Latin" , "Punk" , "Country" ,
                                       "Disco\n polo" , "R&B" , "Metal" ,"Hip-Hop" ,
                                       "Reggae" , "Techno" , "Podcast" ,
                                       "Rage" , "Drill"};

        AppOperator ao;
        UniformGrid ug = new UniformGrid { 
            
            Name = "MainGrid",

        };

        MainPage mainPage;
        public SearchPanel(AppOperator ao, MainPage m)
        {
            InitializeComponent();
            this.ao = ao;
            this.mainPage = m;
            RenderSearchView();
        }

        private void RenderSearchView()
        {
            
            Random rd = new Random();
            Button btn;
            Color color = new Color();
            LinearGradientBrush lgb;
            Binding b = new Binding
            {
                ElementName = ug.Name,
                Path = new PropertyPath(nameof(ug.ActualWidth)),
                Mode = BindingMode.OneWay
            };
            for (int i= 0; i<= musicType.Length-1; i++)
            {
               
                lgb = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1)
                };
                color = Color.FromArgb((byte)rd.Next(256), (byte)rd.Next(256), (byte)rd.Next(256), (byte)rd.Next(256));
                
                lgb.GradientStops.Add(new GradientStop(color,0.0));
                color = Color.FromArgb((byte)rd.Next(256), (byte)rd.Next(256), (byte)rd.Next(256), (byte)rd.Next(256));
                lgb.GradientStops.Add(new GradientStop(color, 1.0));
                btn = new Button
                {
                    Content = musicType[i],
                    Margin = new Thickness(10),
                    Height = 150,
                    Width = 150,
                    Padding = new Thickness(0, 0, 50, 80),
                    Style = (Style)Application.Current.Resources["RoundButton"],
                    Background = lgb,
                    Foreground = Brushes.White,
                    FontFamily = new FontFamily("Cascadia Mono"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Stretch, 
                    VerticalAlignment = VerticalAlignment.Stretch,
                };          
                ug.Children.Add(btn);
            }
            suggestedGrid.Children.Add(ug); 
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchTxtBox.Text != string.Empty)
            {
                 suggestedGrid.Children.Clear();
                RenderSuggested(ao.MusicSearch(SearchTxtBox.Text));
            }
            else
            {
                suggestedGrid.Children.Clear();
            }
        }

        private void RenderSuggested(List<Music> list)
        {
            Button btn;
            StackPanel sp;
            Grid grid = new Grid();
            foreach (Music music in list)
            {
                
                sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Height = 50,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center

                };
                btn = new Button
                {
                    Content = music.MusicAuthors.ToString() + " - " + music.MusicTitle.ToString(),
                    FontSize = 20,
                    FontWeight = FontWeights.DemiBold,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Style = (Style)Application.Current.Resources["OptionsLabels"],                 
                };
                btn.Click += SelectThis;
                sp.Children.Add(btn);
                suggestedGrid.Children.Add(sp);
            }
        }

        private void SelectThis(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string text = btn.Content.ToString();
            string Title = string.Empty;
            string author = string.Empty;
            for (int i = 1; i <= text.Length; i++)
            {
                if (text[i - 1].Equals(' ') && text[i].Equals('-') && text[i + 1].Equals(' '))
                {
                    for (int j = 0; j <= i - 2; j++)
                    {
                        author += text[j];
                    }

                    for (int j = i + 2; j <= text.Length - 1; j++)
                    {
                        Title += text[j];
                    }
                    break;
                }
            }
            mainPage.MediaPause();
            ao.Open();
            SqlDataReader dr = ao.getMusic(Title, author);
            dr.Read();
            byte[] soundtrack = dr.GetValue(0) as byte[];
            if(File.Exists("D:\\test.mp3"))
            {
                File.Delete("D:\\test.mp3");
            }
            File.WriteAllBytes("D:\\test.mp3", soundtrack);
            dr.Close();
            ao.Close();

            ao.MusicTileAndAuthor[0] = author;
            ao.MusicTileAndAuthor[1] = Title;
            
            mainPage.MediaOpener();
        }
    }
}
