using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;

namespace Lista6
{
    public class AppOperator
    {       
        private string connectionstring;
        private SqlConnection cn;
        private ObservableCollection<string> col = new ObservableCollection<string>();

        public SqlConnection sqlCon {
            get
            {
                return cn;
            } 
        }

        public ObservableCollection<string> MusicTileAndAuthor
        {
            get { return col; }
            set { col = value; }
        }

       

      

        public AppOperator()
        {
            connectionstring = "Data Source=DESKTOP-72V7OD8\\SQLEXPRESS;Initial Catalog=UnifiyDataBase;Integrated Security=True";
            cn = new SqlConnection(connectionstring);
            col.Add("");
            col.Add("");
        }

        public void Open() => cn.Open();
        public void Close() => cn.Close();

        public void getMusic()
        {
            
            using (SqlCommand cm = new SqlCommand("dbo.getMusic",cn))
            {

                Open();
                byte[] bytes = cm.ExecuteScalar() as byte[];
                Close();
                File.WriteAllBytes("D:\\test.mp3", bytes);
            }


        }


        public List<Music> MusicSearch(string txt)
        {
            List<Music> list = new List<Music>();
            Music music;
            using (SqlCommand cmd = new SqlCommand("dbo.SearchMusic", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@BLOBtrackName", System.Data.SqlDbType.VarChar, 50).Value = txt;
                cmd.Parameters.Add("@BLOBauthors", System.Data.SqlDbType.VarChar, 50).Value = txt;
                Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    music = new Music(dr.GetValue(0).ToString(),dr.GetValue(1).ToString());
                    list.Add(music);
                }
                Close();
            }

            return list;
        }    
    }
}
