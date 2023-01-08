using System.Collections.ObjectModel;

namespace Lista6
{
    internal class AppOperator
    {
        ObservableCollection<double> width = new ObservableCollection<double> { };

        public ObservableCollection<double> ProgresBarWidth 
        { 
            get 
            {
               return width;
            } 
            set
            { 
                width = value; 
            }
        }

        public AppOperator()
        {
            width.Add(0);
        }
        private void ProgresbarWidthSetter()
        {
            MainPage mp = new MainPage();
            width[0] = mp.MusicBar.Width;
        }
    }
}
