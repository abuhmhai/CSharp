using K4os.Compression.LZ4.Internal;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace MidTermTest
{
    public class Sim
    {
        public string simID;
        public string soSim;
        public string ngayKichHoat;
        public string ngayHetHan;
        public string khuyenMai;

    }
    public class sim
    {
        public static String connectionString = "Server=localhost;Database=simthe;UID=root;Password=Kidhai2k4!!;";

        public static MySqlConnection MoKetNoi(string connectionString)
        {
            MySqlConnection connection = null;
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                return null;
            }
        }// MoKetNoi
       
            
            
            public static List<Sim> DocListSim(MySqlConnection connection)
             {
            List<Sim> listSIM = new List<Sim>();
            MySqlCommand command = new MySqlCommand("SELECT * FROM sim", connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Sim ob1 = new Sim();
                ob1.simID = (string)reader["SimID"];
                ob1.soSim = (string)reader["SoSim"];
                ob1.ngayKichHoat = reader["NgayKichHoat"].ToString();
                ob1.ngayHetHan = reader["NgayHetHan"].ToString();

                if (!reader.IsDBNull(reader.GetOrdinal("KhuyenMai")))
                {
                    ob1.khuyenMai = reader["KhuyenMai"].ToString();
                }
                else
                {
                    ob1.khuyenMai = null;
                }
                listSIM.Add(ob1);
            }

            reader.Close();
            command.Dispose();
            return listSIM;
        }
        
        public static DataTable DocBangSim(MySqlConnection connection)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM sim", connection);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table_out;
            table_out = new DataTable();
            table_out.Columns.Add("SimID");
            table_out.Columns.Add("SoSim");
            table_out.Columns.Add("NgayKichHoat");
            table_out.Columns.Add("NgayHetHan");
            table_out.Columns.Add("KhuyenMai");
            while (reader.Read())
            {
                DataRow row = table_out.NewRow();
                row["SimID"] = reader["SimID"];
                row["SoSim"] = reader["SoSim"];
                row["NgayKichHoat"] = reader["NgayKichHoat"];
                row["NgayHetHan"] = reader["NgayHetHan"];
                row["KhuyenMai"] = reader["KhuyenMai"];
                table_out.Rows.Add(row);
            }//while

            reader.Close();
            command.Dispose();
            return table_out;
        }

        
    }
}

