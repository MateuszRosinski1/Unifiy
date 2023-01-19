using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lista6.UserContollPanel
{
    /// <summary>
    /// Interaction logic for SearchPanel.xaml
    /// </summary>
    public partial class SearchPanel : UserControl
    {
        private string[] musicType = { "Pop", "Rap", "Rock", "K-pop", "Jazz", 
                                       "Trap", "Latin" , "Punk" , "Country" ,
                                       "Disco polo" , "R&B" , "Metal" ,"Hip-Hop" ,
                                       "Reggae" , "Techno" , "Klasyczna" , "Podcast" ,
                                       "Rage" , "Drill"};


        UniformGrid ug = new UniformGrid { 
            
            Name = "MainGrid",

        };

        public SearchPanel()
        {
            InitializeComponent();

            RenderSuggested();
        }

        private void RenderSuggested()
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
                    FontFamily = new FontFamily("Poppins"),
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
            }
            else
            {
                suggestedGrid.Children.Add(ug);
            }

        }
    }
}
