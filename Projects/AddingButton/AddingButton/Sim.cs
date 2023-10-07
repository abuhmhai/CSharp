using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddingButton
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
        public static void XoaDuLieu(MySqlConnection connection, string sosim)
        {
            string query = "DELETE FROM Sim WHERE SoSim = @SoSim";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@SoSim", sosim);
            command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }//Delete
        
        public static void GhiMotSoSim(MySqlConnection connection, string simID, string soSim, string ngayKichHoat, string ngayHetHan, string khuyenMai)
        {
            try
            {
                string query = "INSERT INTO sim (SimID, SoSim, NgayKichHoat, NgayHetHan, KhuyenMai) VALUES (@SimID, @SoSim, @NgayKichHoat, @NgayHetHan, @KhuyenMai)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@SimID", simID);
                command.Parameters.AddWithValue("@SoSim", soSim);
                command.Parameters.AddWithValue("@NgayKichHoat", ngayKichHoat);
                command.Parameters.AddWithValue("@NgayHetHan", ngayHetHan);
                command.Parameters.AddWithValue("@KhuyenMai", khuyenMai);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }


        private static string SafeGetString(MySqlDataReader reader, int v)
        {
            if (!reader.IsDBNull(v))
                return reader.GetString(v);
            return "NULL";
        }
    }
}
    
