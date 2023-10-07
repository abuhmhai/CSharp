using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErasingButoon
{
    public class Sim
    {
        public String simID;
        public String soSim;
        public String ngayKichHoat;
        public String ngayHetHan;
        public String khuyenMai;


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
        public static void XoaDuLieu(MySqlConnection connection, string sosim)
        {
            string query = "DELETE FROM Sim WHERE SoSim = @SoSim";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@SoSim", sosim);
            command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }//Delete

        public static int DemSoSim(MySqlConnection connection)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            int count = 0;
            MySqlConnection connection1 = sim.MoKetNoi(connectionString);

            MySqlCommand command = new MySqlCommand("SELECT * FROM Sim", connection1);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Sim sm = new Sim()
                {
                    simID = sim.SafeGetString(reader, 0),
                    soSim = sim.SafeGetString(reader, 1),
                    ngayKichHoat = sim.SafeGetString(reader, 2),
                    ngayHetHan = sim.SafeGetString(reader, 3),
                    khuyenMai = sim.SafeGetString(reader, 4)
                };

                string[] values = { sm.soSim, sm.ngayKichHoat, sm.ngayHetHan, sm.khuyenMai };
                dict.Add(sm.simID, values);
            }
            connection1.Close();

            foreach (var item in dict.Values)
            {
                if (item[2].ToString() == "20/08/2023 0:00:00")
                {
                    count++;
                }
            }
            return count;
        }//DemSoSim
        private static string SafeGetString(MySqlDataReader reader, int v)
        {
            if (!reader.IsDBNull(v))
                return reader.GetString(v);
            return "NULL";
        }
    }

}
