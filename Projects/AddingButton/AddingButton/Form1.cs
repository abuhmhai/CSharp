using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddingButton
{
    public partial class Form1 : Form
    {
        public DataTable table_sim = null;
        public bool using_datasouce = false;
        bool allowMouseMove = true;
        private readonly MySqlConnection connection;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string SimID = this.textBox1.Text;
            string SoSim = this.textBox2.Text;
            string NgayKichHoat = this.textBox3.Text;
            string NgayHetHan = this.textBox4.Text;
            string KhuyenMai = this.textBox5.Text;

            // Kiểm tra xem các trường thông tin đã được nhập đầy đủ hay chưa
            if (string.IsNullOrEmpty(SimID) || string.IsNullOrEmpty(SoSim) || string.IsNullOrEmpty(NgayKichHoat) || string.IsNullOrEmpty(NgayHetHan))
            {
                MessageBox.Show("Các trường giá trị cần nhập liệu");
                return;
            }//if

            // Kiểm tra xem StudentID đã tồn tại trong DataGridView hay chưa
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (row.Cells["SimID"].Value == null)
                    continue;

                if (row.Cells["SimID"].Value.ToString() == SimID)
                {
                    MessageBox.Show("Đã tồn tại mã sinh viên này");
                    allowMouseMove = true;
                    return;
                }//if
            }//for

            if (using_datasouce == true)
            {
                DataTable table = (DataTable)dataGridView1.DataSource;
                DataRow row_data = table.NewRow();
                row_data["SimID"] = SimID;
                row_data["SoSim"] = SoSim;
                row_data["NgayKichHoat"] = NgayKichHoat;
                row_data["NgayHetHan"] = NgayHetHan;
                row_data["KhuyenMai"] = KhuyenMai;
                table.Rows.Add(row_data);
            }
            else
            {
                dataGridView1.Rows.Add(SimID, SoSim, NgayKichHoat, NgayHetHan, KhuyenMai);
            }

            // Thêm dữ liệu vào bảng Students của CSDL

            MySqlConnection connection = null;
            connection = sim.MoKetNoi(sim.connectionString);
            if (connection == null)
            {
                allowMouseMove = true;
                return;
            }//if
            sim.GhiMotSoSim(connection, SimID, SoSim, NgayKichHoat, NgayHetHan, KhuyenMai);
            connection.Close();
            connection.Dispose();

            allowMouseMove = true;

        }//button4click them

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (allowMouseMove == false)
                return;

            // Lấy dòng hiện tại khi di chuyển phím mũi tên lên xuống
            DataGridViewRow currentRow = dataGridView1.CurrentRow;
            // Kiểm tra xem có dòng nào đang được chọn hay không
            if (currentRow != null)
            {
                this.textBox1.Text = currentRow.Cells["SimID"].Value.ToString();
                this.textBox2.Text = currentRow.Cells["SoSim"].Value.ToString();
                this.textBox3.Text = currentRow.Cells["NgayKichHoat"].Value.ToString();
                this.textBox4.Text = currentRow.Cells["NgayHetHan"].Value.ToString();
                this.textBox5.Text = currentRow.Cells["KhuyenMai"].Value.ToString();
            }//if
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Kêu beep
                //Console.Beep();

                // Không nhận kí tự gõ sai
                e.Handled = true;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Back))
            {
                // Kêu beep
                Console.Beep();

                // Ngăn chặn sự kiện KeyDown lan ra các điều khiển khác
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            //Thực hiện tìm kiếm khi ta nhấn nút enter.
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Kêu beep
                //Console.Beep();

                // Không nhận kí tự gõ sai
                e.Handled = true;
            }
        }
    }
}
