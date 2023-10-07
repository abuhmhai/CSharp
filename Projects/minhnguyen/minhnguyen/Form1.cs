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

namespace minhnguyen
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

        private void button4_Click(object sender, EventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            if (using_datasouce == true)
            {
                dataGridView1.DataSource = table_sim;
            }//if
            else
            {
                dataGridView1.Columns.Add("SimID", "Mã Sim");
                dataGridView1.Columns.Add("SoSim", "Số sim");
                dataGridView1.Columns.Add("NgayKichHoat", "Ngày kích hoạt");
                dataGridView1.Columns.Add("NgayHetHan", "Ngày hết hạn");
                dataGridView1.Columns.Add("KhuyenMai", "Khuyến mãi");

                for (int i = 0; i < table_sim.Rows.Count; i++)
                {
                    DataRow row = table_sim.Rows[i];
                    object[] rowData = row.ItemArray; // Lấy dữ liệu từng dòng dưới dạng mảng object
                    // Thêm dữ liệu vào DataGridView bằng cách tạo mới hàng và thêm vào DataGridView
                    int rowIndex = dataGridView1.Rows.Add(rowData);
                }
            }//else

            //textBox1.Enabled = false;
            this.textBox1.Focus();
            // Gán sự kiện CellEnter cho DataGridView
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
                this.textBox1.Text = currentRow.Cells["SimID"].Value.ToString();
                this.textBox2.Text = currentRow.Cells["SoSim"].Value.ToString();
                this.textBox3.Text = currentRow.Cells["NgayKichHoat"].Value.ToString();
                this.textBox4.Text = currentRow.Cells["NgayHetHan"].Value.ToString();
                this.textBox5.Text = currentRow.Cells["KhuyenMai"].Value.ToString();
            }//if
        }// dataGridView1_CellMouseMove

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

        private void button1_Click(object sender, EventArgs e)
        {
            string sosim = textBox2.Text.Trim();
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
        }//button1 tim kiem

        private void button5_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.Close();
            // Đóng chương trình
            Application.Exit();
            // Đóng console
            Environment.Exit(0);
        }//btn5 thoat

        private void button2_Click(object sender, EventArgs e)
        {
            string soSim = textBox2.Text;

            if (soSim.Length == 10)
            {
                bool found = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string sim = row.Cells["SoSim"].Value.ToString();

                    if (sim == soSim)
                    {

                        // Sửa dòng tương ứng trong DataGridView
                        row.Cells["SimID"].Value = textBox1.Text;
                        row.Cells["NgayKichHoat"].Value = textBox3.Text;
                        row.Cells["NgayHetHan"].Value = textBox4.Text;
                        row.Cells["KhuyenMai"].Value = textBox5.Text;

                        // Sửa dữ liệu tương ứng trong bảng SIM của CSDL SimThe
                        UpdateSimDataInDatabase(soSim, textBox5.Text);

                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    MessageBox.Show("Đã sửa thông tin SIM thành công.");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy số SIM trong danh sách.");
                }

                textBox1.Focus();
            }
            else
            {
                MessageBox.Show("Số SIM không hợp lệ.");
                textBox1.Focus();
            }
        }//button2 sửa
        private void UpdateSimDataInDatabase(string soSim, string KhuyenMai)
        {
            // Kết nối đến CSDL
            MySqlConnection connection = sim.MoKetNoi(sim.connectionString);
            using (connection)
            {

                // Chuẩn bị câu truy vấn SQL để cập nhật dữ liệu
                string query = "UPDATE sim SET KhuyenMai = @KhuyenMai WHERE SoSim = @SoSim";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Thêm các tham số vào câu truy vấn
                    command.Parameters.AddWithValue("@KhuyenMai", KhuyenMai);
                    command.Parameters.AddWithValue("@SoSim", soSim);

                    // Thực thi câu truy vấn
                    command.ExecuteNonQuery();
                }

                // Đóng kết nối
                connection.Close();
            }
        }//update in database

        private void button3_Click(object sender, EventArgs e)
        {
            string sosim = textBox2.Text.Trim();

            if (sosim.Length == 10)
            {
                bool deleted = DeleteSimFromDatabase(sosim);

                if (deleted)
                {
                    // Xóa dòng tương ứng trong DataGridView
                    DataGridViewRow row = dataGridView1.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.Cells["SoSim"].Value != null && r.Cells["SoSim"].Value.ToString() == sosim);

                    if (row != null)
                    {
                        dataGridView1.Rows.Remove(row);
                        MessageBox.Show("Đã xóa số sim");
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy số sim");
                }
            }
            else
            {
                MessageBox.Show("Số sim không hợp lệ");
            }

            textBox1.Focus();
        }//button3 xóa

        private bool DeleteSimFromDatabase(string sosim)
        {
            bool deleted = false;

            // Kết nối CSDL và xóa dữ liệu trong bảng SIM
            MySqlConnection connection = sim.MoKetNoi(sim.connectionString);
            using (connection)
            {
                //connection.Open();

                string query = "DELETE FROM sim WHERE SoSim = @SoSim";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SoSim", sosim);
                    int rowsAffected = command.ExecuteNonQuery();
                    deleted = rowsAffected > 0;
                }
            }

            return deleted;
        } //Delete Sim From Database

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                // Kêu beep
                //Console.Beep();

                // Không nhận kí tự gõ sai
                e.Handled = true;
            }
        }//txtbox2 so sim keypress

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            string sosim = textBox2.Text.Trim();
            // Kiểm tra kí tự được gõ có phải là 'x', phím Enter hoặc phím Xóa (Backspace) hay không
            if ((e.KeyChar != 'x' && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Back))
            {
                // Ngăn chặn kí tự không hợp lệ từ được hiển thị trong TextBox
                e.Handled = true;

                // Phát ra âm thanh beep
                Console.Beep();
            }
        }//txtbox5 khuyen mai keypress

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.X && e.KeyCode != Keys.Back)
            {
                // Kêu beep
                Console.Beep();

                // Ngăn chặn sự kiện KeyDown lan ra các điều khiển khác
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            // Check if the length of the text is more than 1 character, and restrict input to 1 character.
            if (textBox5.Text.Length >= 1 && e.KeyCode != Keys.Back)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }//txtbox5 khuyen mai keydown

        private void button6_Click(object sender, EventArgs e)
        {
            
            if (table_sim != null)
            {
                List<Sims> simList = ConvertDataTableToList(table_sim);
                Sims.XuatDuLieuRaCSV(simList, "data.csv");

                MessageBox.Show("Xuất dữ liệu thành công ra file CSV.");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
            }//FILE CSV
            
            /*
            if (table_sim != null)
            {
                List<Sims> simList = ConvertDataTableToList(table_sim);
                Sims.XuatDuLieuRaJSON(simList, "data.json");

                MessageBox.Show("Xuất dữ liệu thành công ra file JSON.");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
            }//FILE JSON */
        }//button6 xuat file

        private List<Sims> ConvertDataTableToList(DataTable table_sim)
        {
            List<Sims> simList = new List<Sims>();

            foreach (DataRow row in table_sim.Rows)
            {
                Sims sim = new Sims
                {
                    SimID = row["SimID"].ToString(),
                    SoSim = row["SoSim"].ToString(),
                    NgayKichHoat = row["NgayKichHoat"].ToString(),
                    NgayHetHan = row["NgayHetHan"].ToString(),
                    KhuyenMai = row["KhuyenMai"].ToString()
                };

                simList.Add(sim);
            }
            return simList;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int appear = sim.DemSoSim();
            MessageBox.Show("Có " + appear.ToString() + " số sim hết hạn ngày 20-10-2023 trong danh sách.");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }// Form1
}//namespace
