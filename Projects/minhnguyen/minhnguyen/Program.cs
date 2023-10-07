using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minhnguyen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Console and GUI: ");

            List<Sims> listSim = null;
            MySqlConnection connection = null;
            connection = sim.MoKetNoi(sim.connectionString);
            if (connection == null)
                Console.WriteLine("Không kết nối được CSDL với xâu kết nối" + sim.connectionString);
            else
            {
                listSim = sim.DocDulieu(connection);
                foreach (Sims sim in listSim)
                {
                    Console.WriteLine("Mã sim " + sim.SimID + ", số sim " + sim.SoSim + " từ ngày " + sim.NgayKichHoat + " đến ngày " + sim.NgayHetHan + " Khuyến mãi (x/o) " + sim.KhuyenMai);
                }
                /*for (int i = 0; i < listSim.Count; i++)
                {
                    Console.WriteLine("Mã sim " + listSim[i].SimID + ", số sim " + listSim[i].SoSim + " từ ngày " + listSim[i].NgayKichHoat + " đến ngày " + listSim[i].NgayHetHan + " Khuyến mãi (x/o) " + listSim[i].KhuyenMai);
                }*/

            }//else


            DataTable table = sim.DocListSim(connection);
            //DataTable table_sim = sim.DocListSim(connection);
            Console.WriteLine("Nhấn Enter để tiếp tục");
            Console.ReadLine();

            // Gọi hàm xuất dữ liệu ra file CSV
            Sims.XuatDuLieuRaCSV(listSim, "data.csv");

            // Gọi hàm xuất dữ liệu ra file JSON
            //Sims.XuatDuLieuRaJSON(listSim, "data.json");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 frm = new Form1();
            frm.table_sim = table;
            //frm.table_sim = table_sim;
            Application.Run(frm);
        }
    }
}
