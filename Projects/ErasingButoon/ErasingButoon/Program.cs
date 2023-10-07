using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErasingButoon
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Console and GUI");

            List<Sim> listSIM = null;
            MySqlConnection connection = null;
            connection = sim.MoKetNoi(sim.connectionString);
            if (connection == null)
                Console.WriteLine("Không kết nối được CSDL với xâu kết nối" + sim.connectionString);
            else
            {
                listSIM = sim.DocListSim(connection);
                // List<Sim> listSIM = DocListSim(connection);
                foreach (Sim sim in listSIM)
                {
                    DateTime ngayHetHan;
                    if (DateTime.TryParse(sim.ngayHetHan, out ngayHetHan) && ngayHetHan.Date == new DateTime(2023, 8, 20)) //&& !string.IsNullOrEmpty(sim.khuyenMai))
                    {
                        Console.WriteLine($"SimID: {sim.simID}, SoSim: {sim.soSim}, NgayKichHoat: {sim.ngayKichHoat}, NgayHetHan: {sim.ngayHetHan}, KhuyenMai: {sim.khuyenMai}");
                    }
                }

            }//else
             //duyet lenh sim
            connection = sim.MoKetNoi(sim.connectionString);
            DataTable table = sim.DocBangSim(connection);
            Console.WriteLine("Nhấn Enter để tiếp tục...");
            Console.ReadLine();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 frm = new Form1();
            frm.table_sim = table;
            Application.Run(frm);

        }
    }
}
