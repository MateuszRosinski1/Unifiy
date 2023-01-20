using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista6
{
    public class Music
    {

        private string musicTitle;
        private string musicauthors;
        private byte[] musicsound;
        private byte[] musicImg;       

        public byte[] MusicMp3
        {
            get { return musicsound; }
            set { musicsound = value; }
        }

        public byte[] MusicImg
        {
            get { return musicImg; }
            set { musicImg = value; }
        }

        public string MusicTitle
        {
            get { return musicTitle; }
            set { musicTitle = value; }
        }

        public string MusicAuthors
        {
            get { return musicauthors; }
            set { musicauthors = value; }
        }

        public Music()
        {

        }

        public Music(string musictitle,string musicauth, byte[] musicsound, byte[] musicimg)
        {
            this.musicTitle= musictitle;
            this.musicauthors = musicauth;
            this.musicImg = musicimg;
            this.musicsound = musicsound;          
        }

        public Music(string musicTitle, string musicauth)
        {
            this.musicTitle = musicTitle;
            this.musicauthors = musicauth;
        }
    }
}
