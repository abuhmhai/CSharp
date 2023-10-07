using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minhnguyen
{
    public class Sims
    {
        public string SimID;
        public string SoSim;
        public string NgayKichHoat;
        public string NgayHetHan;
        public string KhuyenMai;


        public static void XuatDuLieuRaCSV(List<Sims> listSim, string tenTep)

        {
            try
            {
                using (StreamWriter sw = new StreamWriter(tenTep, false, Encoding.UTF8))
                {
                    sw.WriteLine("SimID,SoSim,NgayKichHoat,NgayHetHan,KhuyenMai");

                    foreach (Sims sim in listSim)
                    {
                        string line = $"{sim.SimID},{sim.SoSim},{sim.NgayKichHoat},{sim.NgayHetHan},{sim.KhuyenMai}";
                        sw.WriteLine(line);
                    }
                }
                Console.WriteLine("Xuất dữ liệu thành công ra file CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xuất dữ liệu ra file CSV: {ex.Message}");
            }
        }//FILE CSV
        /*
        public static void XuatDuLieuRaJSON(List<Sims> listSim, string tenTep)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(tenTep, false, Encoding.UTF8))
                {
                    sw.WriteLine("SimID,SoSim,NgayKichHoat,NgayHetHan,KhuyenMai");

                    foreach (Sims sim in listSim)
                    {
                        string line = $"{sim.SimID},{sim.SoSim},{sim.NgayKichHoat},{sim.NgayHetHan},{sim.KhuyenMai}";
                        sw.WriteLine(line);
                    }
                }
                Console.WriteLine("Xuất dữ liệu thành công ra file JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xuất dữ liệu ra file JSON: {ex.Message}");
            }
        }//FILEJSON */
    }
    public class sim
    {
        public static String connectionString = "Server=LOCALHOST;Database=simthe;UID=root;Password=Kidhai2k4!!;";

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
        }//MoKetNoi
        public static List<Sims> DocDulieu(MySqlConnection connection)
        {
            List<Sims> listSim = new List<Sims>();
            MySqlCommand command = new MySqlCommand("SELECT * FROM sim WHERE NgayHetHan = '20-10-2023'AND KhuyenMai = 'x'", connection);
            //MySqlCommand command = new MySqlCommand("SELECT * FROM sim WHERE NgayHetHan =" + "'" + "20-10-2023" + "'", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Sims ob = new Sims();
                ob.SimID = (string)reader["SimID"];
                ob.SoSim = (string)reader["SoSim"];
                ob.NgayKichHoat = (string)reader["NgayKichHoat"];
                ob.NgayHetHan = (string)reader["NgayHetHan"];
                ob.KhuyenMai = (string)reader["KhuyenMai"];
                listSim.Add(ob);

            }
            reader.Close();
            command.Dispose();
            return listSim;
        }//DocDulieu
        public static DataTable DocListSim(MySqlConnection connection)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM sim", connection);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table_out = new DataTable();
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
        }//DocBangSim
        public static void GhiMotSoSim(MySqlConnection connection, string SimID, string SoSim, string NgayKichHoat, string NgayHetHan, string KhuyenMai)
        {

            string query = "INSERT INTO sim (SimID, SoSim, NgayKichHoat, NgayHetHan, KhuyenMai) VALUES (@SimID, @SoSim, @NgayKichHoat, @NgayHetHan, @KhuyenMai)";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@SimID", SimID);
            command.Parameters.AddWithValue("@SoSim", SoSim);
            command.Parameters.AddWithValue("@NgayKichHoat", NgayKichHoat);
            command.Parameters.AddWithValue("@NgayHetHan", NgayHetHan);
            command.Parameters.AddWithValue("@KhuyenMai", KhuyenMai);
            command.ExecuteNonQuery();

            connection.Close();
            connection.Dispose();
        }// GhiMotSoSim

        public static void SuaDuLieu(MySqlConnection connection, string SimID, string SoSim, string NgayKichHoat, string NgayHetHan, string KhuyenMai)
        {
            string query = "UPDATE sim SET SoSim = @SoSim,NgayKichHoat =@NgayKichHoat, NgayHetHan=@NgayHetHan, KhuyenMai = @KhuyenMai WHERE SimID = @SimID";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@SimID", SimID);
            command.Parameters.AddWithValue("@SoSim", SoSim);
            command.Parameters.AddWithValue("@NgayKichHoat", NgayKichHoat);
            command.Parameters.AddWithValue("@NgayHetHan", NgayHetHan);
            command.Parameters.AddWithValue("@KhuyenMai", KhuyenMai);
            command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }//UpdateSV

        public static void XoaDuLieu(MySqlConnection connection, string m_sosim)
        {
            string query = "DELETE FROM sim WHERE SoSim = @SoSim";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@SoSim", m_sosim);
            command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }//Delete

        public static int DemSoSim()
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            int count = 0;
            MySqlConnection connection = sim.MoKetNoi(connectionString);

            MySqlCommand command = new MySqlCommand("SELECT * FROM sim", connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Sims sm = new Sims()
                {
                    SimID = sim.SafeGetString(reader, 0),
                    SoSim = sim.SafeGetString(reader, 1),
                    NgayKichHoat = sim.SafeGetString(reader, 2),
                    NgayHetHan = sim.SafeGetString(reader, 3),
                    KhuyenMai = sim.SafeGetString(reader, 4)
                };

                string[] values = { sm.SoSim, sm.NgayKichHoat, sm.NgayHetHan, sm.KhuyenMai };
                dict.Add(sm.SimID, values);
            }
            connection.Close();

            foreach (var item in dict.Values)
            {
                if (item[2].ToString() == "20-10-2023" && item[3].ToString() == "x")
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
