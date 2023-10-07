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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SearchingButton
{
    public partial class Form1 : Form
    {

        public DataTable table_sim = null;
        public bool using_datasouce = false;
        bool allowMouseMove = true;

        public Form1()
        {
            InitializeComponent();
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
                dataGridView1.Columns.Add("GhiChu", "Ghi Chus");

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

            // Lấy dòng hiện tại khi di chuyển phím mũi tên lên xuống
            DataGridViewRow currentRow = dataGridView1.CurrentRow;
            // Kiểm tra xem có dòng nào đang được chọn hay không
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
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Prevent invalid characters from being displayed
                Console.Beep(); // Emit a "beep" sound
            }
            else if (e.KeyChar == (char)Keys.Enter) // Compare e.KeyChar to the Enter key correctly
            {
                e.Handled = true; // Prevent Enter key from being displayed in the TextBox
                button1_Click(sender, e);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sosim = textBox1.Text.Trim();
            if (sosim.Length == 10)
            {
                bool found = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["SoSim"].Value != null && row.Cells["SoSim"].Value.ToString() == sosim)
                    {
                        // Tìm thấy số sim
                        found = true;

                        // Chọn cả hàng (dòng)
                        row.Selected = true;

                        // Cuộn để hiển thị cả hàng
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;

                        // Di chuyển đến ô đầu tiên của hàng
                        dataGridView1.CurrentCell = row.Cells[0];

                        // Hiển thị hộp thoại thông báo
                        MessageBox.Show("Tìm thấy số sim");
                        break;
                    }
                }

                if (!found)
                {
                    // Không tìm thấy số sim
                    MessageBox.Show("Không tìm thấy số sim");
                }
            }
            else
            {
                // Độ dài số sim không hợp lệ
                MessageBox.Show("Số sim không hợp lệ");
            }

            // Focus trở lại TextBox "sosim"
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();   
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
                            string content = $"{sim.simID},{sim.soSim},{sim.ngayKichHoat},{sim.ngayHetHan},{sim.ghiChu}";
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

        
    }
}
