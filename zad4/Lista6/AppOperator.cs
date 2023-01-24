using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

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

        public SqlDataReader loginFun(string login)
        {
            Open();
            SqlCommand com = new SqlCommand("dbo.LoginProcedure", cn);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.Add("@login", System.Data.SqlDbType.VarChar, 50).Value = login;
            SqlDataReader sdr = com.ExecuteReader();
            com.Dispose();
            return sdr;
        }


        public SqlDataReader getMusic(string title,string author)
        {

            SqlCommand cmd = new SqlCommand("dbo.SelectMusic", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@BLOBtrackName", System.Data.SqlDbType.VarChar, 50).Value = title;
            cmd.Parameters.Add("@BLOBauthors", System.Data.SqlDbType.VarChar, 50).Value = author;
            SqlDataReader sdr= cmd.ExecuteReader();
            return sdr;
        }

        public bool UniqeLogin(string login)
        {
            Open();
            SqlCommand cmd = new SqlCommand("dbo.UniqeLogin", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@login", System.Data.SqlDbType.VarChar, 50).Value = login;
            SqlDataReader sdr = cmd.ExecuteReader();
            if(sdr.Read())
            {
                Close();            
                return false;
            }
            else
            {
                Close();
                return true;
            }
        }

        public bool UniqeEmail(string email)
        {
            Open();
            SqlCommand cmd = new SqlCommand("dbo.UniqeEmail", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 50).Value = email;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                Close();
                return false;
            }
            else
            {
                Close();
                return true;
            }

        }

        public SqlCommand InsertUser(string login,string password,string salt,string email)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.InsertUser", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@login", System.Data.SqlDbType.VarChar, 50).Value = login;
            cmd.Parameters.Add("@password", System.Data.SqlDbType.VarChar, int.MaxValue).Value = password;
            cmd.Parameters.Add("@salt", System.Data.SqlDbType.VarChar, int.MaxValue).Value = salt;
            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 50).Value = email;
            adapter.InsertCommand = cmd;
            Open();
            adapter.InsertCommand.ExecuteNonQuery();
            Close();
            return cmd;

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
