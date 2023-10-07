using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidTermTest
{
    public partial class Form1 : Form
    {
        /// <summary>
        public DataTable table_sim = null;
        public bool using_datasouce = false;
        bool allowMouseMove = true;
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
                if (this.textBox1.Text.Length == 10 && this.textBox1.Text.All(char.IsDigit))
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (row.Cells["SoSim"].Value == null)
                            continue;

                        if (row.Cells["SoSim"].Value.ToString() == this.textBox1.Text)
                        {
                            dataGridView1.CurrentCell = row.Cells[0];
                            MessageBox.Show("Tìm thấy số sim " + this.textBox1.Text);
                            allowMouseMove = true;
                            return;
                        }
                    }
                    MessageBox.Show("Không tìm thấy số sim" + this.textBox1.Text);
                }//if
                else
                {
                    MessageBox.Show("Số sim không hợp lệ");
                }
                allowMouseMove = true;
            }
        
    
    
        private void Form1_Load(object sender, EventArgs e)
        {
            if (using_datasouce == true)
            {
                dataGridView1.DataSource = table_sim;
            }
            else
            {
                dataGridView1.Columns.Add("SimID", "Mã sim");
                dataGridView1.Columns.Add("SoSim", "Số sim");
                dataGridView1.Columns.Add("NgayKichHoat", "Ngày Kích Hoạt");
                dataGridView1.Columns.Add("NgayHetHan", "Ngày Hết Hạn");
                dataGridView1.Columns.Add("KhuyenMai", "Khuyến Mại");

                for (int i = 0; i < table_sim.Rows.Count; i++)
                {
                    DataRow row = table_sim.Rows[i];
                    object[] rowData = row.ItemArray;
                    int rowIndex = dataGridView1.Rows.Add(rowData);
                }//for
            }//else
            this.textBox1.Focus();
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (allowMouseMove == false)
                return;

            DataGridViewRow currentRow = dataGridView1.CurrentRow;
            if (currentRow != null)
            {
                this.textBox1.Text = currentRow.Cells["SoSim"].Value.ToString();
            }//if
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                allowMouseMove = false;
                if (this.textBox1.Text.Length == 10 && this.textBox1.Text.All(char.IsDigit))
                {
                    bool find = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["SoSim"].Value == null)
                            continue;
                        if (dataGridView1.Rows[i].Cells["SoSim"].Value.ToString() == this.textBox1.Text)
                        {
                            find = true;
                            break;
                        }//if
                    }//for

                    if (find == false)
                    {
                        this.textBox1.Focus();
                    }//if
                }//if
                else
                {
                    Console.Beep();
                }
            }//if

        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Đóng form
            this.Close();
            // Đóng chương trình
            //Application.Exit();
            // Đóng console
            //Environment.Exit(0);
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = sim.MoKetNoi(sim.connectionString);

            if (connection != null)
            {
                List<Sim> listSIM = sim.DocListSim(connection);

                try
                {
                    using (StreamWriter writer = new StreamWriter("Sim.csv"))
                    {
                        foreach (Sim sim in listSIM)
                        {
                            string content = $"{sim.simID},{sim.soSim},{sim.ngayKichHoat},{sim.ngayHetHan},{sim.khuyenMai}";
                            writer.WriteLine(content);
                        }
                    }

                    MessageBox.Show("Dữ liệu đã được xuất ra file Sim.csv", "Xuất dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất dữ liệu ra file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                connection.Close();
            }
            else
            {
                MessageBox.Show("Không thể kết nối đến CSDL.", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn chặn kí tự không hợp lệ được hiển thị
                Console.Beep(); // Phát ra tiếng "beep"
            }
        }
    }
    
}