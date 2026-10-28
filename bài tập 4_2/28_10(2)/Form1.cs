using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyDangKyHoc
{
    public class Course
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }

    public class frmRegister : Form
    {
        private TextBox txtFullName;
        private MaskedTextBox mtxtPhone;
        private DateTimePicker dtpBirthDate;
        private RadioButton rdoMale, rdoFemale;
        private CheckBox chkStudent, chkAgree;
        private ComboBox cboCourse;
        private Button btnRegister, btnExit;

        // Chuỗi kết nối CSDL SQL Server (Đổi localhost\SQLEXPRESS nếu máy bạn dùng Server khác)
        private string connectionString = @"Server=localhost\SQLEXPRESS;Database=QuanLyDangKyDB;Integrated Security=True;TrustServerCertificate=True;";

        public frmRegister()
        {
            TaoGiaoDien();
            NapDanhSachKhoaHocTuCSDL();
        }

        private void TaoGiaoDien()
        {
            this.Text = "Form Đăng Ký Khóa Học (Kết nối CSDL)";
            this.Size = new Size(460, 430);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9.5F);

            int labelX = 25, controlX = 150, width = 250;

            // --- 1. Họ và tên ---
            this.Controls.Add(new Label { Text = "Họ và tên:", Location = new Point(labelX, 25), AutoSize = true });
            txtFullName = new TextBox { Location = new Point(controlX, 22), Width = width };
            this.Controls.Add(txtFullName);

            // --- 2. Số điện thoại (MaskedTextBox) ---
            this.Controls.Add(new Label { Text = "Số điện thoại:", Location = new Point(labelX, 65), AutoSize = true });
            mtxtPhone = new MaskedTextBox
            {
                Location = new Point(controlX, 62),
                Width = width,
                Mask = "(000) 000-0000"
            };
            this.Controls.Add(mtxtPhone);

            // --- 3. Ngày sinh (DateTimePicker: Short Format dd/MM/yyyy) ---
            this.Controls.Add(new Label { Text = "Ngày sinh:", Location = new Point(labelX, 105), AutoSize = true });
            dtpBirthDate = new DateTimePicker
            {
                Location = new Point(controlX, 102),
                Width = width,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };
            this.Controls.Add(dtpBirthDate);

            // --- 4. Giới tính (RadioButton) ---
            this.Controls.Add(new Label { Text = "Giới tính:", Location = new Point(labelX, 145), AutoSize = true });
            rdoMale = new RadioButton { Text = "Nam", Location = new Point(controlX, 143), AutoSize = true, Checked = true };
            rdoFemale = new RadioButton { Text = "Nữ", Location = new Point(controlX + 80, 143), AutoSize = true };
            this.Controls.Add(rdoMale);
            this.Controls.Add(rdoFemale);

            // --- 5. Khóa học (ComboBox Binding Data) ---
            this.Controls.Add(new Label { Text = "Khóa học:", Location = new Point(labelX, 185), AutoSize = true });
            cboCourse = new ComboBox
            {
                Location = new Point(controlX, 182),
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cboCourse);

            // --- 6. Tùy chọn bổ sung (CheckBox) ---
            chkStudent = new CheckBox { Text = "Đối tượng Sinh viên (Giảm 10%)", Location = new Point(controlX, 220), AutoSize = true };
            chkAgree = new CheckBox { Text = "Tôi đồng ý với điều khoản đăng ký", Location = new Point(controlX, 250), AutoSize = true };
            this.Controls.Add(chkStudent);
            this.Controls.Add(chkAgree);

            // --- 7. Nút Đăng ký & Thoát ---
            btnRegister = new Button { Text = "Đăng ký", Location = new Point(controlX, 300), Width = 110, Height = 35, BackColor = Color.LightGreen };
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            btnExit = new Button { Text = "Thoát", Location = new Point(controlX + 130, 300), Width = 110, Height = 35 };
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);

            this.AcceptButton = btnRegister;
            this.CancelButton = btnExit;
        }

        // --- NẠP DANH SÁCH KHÓA HỌC TỪ CSDL VÀO COMBOBOX ---
        private void NapDanhSachKhoaHocTuCSDL()
        {
            string query = "SELECT CourseId, CourseName FROM KhoaHoc";
            List<Course> listCourses = new List<Course>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listCourses.Add(new Course
                                {
                                    CourseId = reader["CourseId"].ToString(),
                                    CourseName = reader["CourseName"].ToString()
                                });
                            }
                        }
                    }
                }

                cboCourse.DataSource = listCourses;
                cboCourse.DisplayMember = "CourseName";
                cboCourse.ValueMember = "CourseId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách khóa học từ CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- XỬ LÝ LƯU ĐĂNG KÝ VÀO CSDL ---
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // 1. Validation cơ bản
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (!mtxtPhone.MaskCompleted)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ số điện thoại theo mẫu (000) 000-0000!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtxtPhone.Focus();
                return;
            }

            if (!chkAgree.Checked)
            {
                MessageBox.Show("Bạn cần tích chọn đồng ý điều khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy dữ liệu từ Form
            string fullName = txtFullName.Text.Trim();
            string phone = mtxtPhone.Text;
            DateTime birthDate = dtpBirthDate.Value;
            string gender = rdoMale.Checked ? "Nam" : "Nữ";
            string selectedCourseId = cboCourse.SelectedValue?.ToString();
            bool isStudent = chkStudent.Checked;

            // 3. Thực thi INSERT vào SQL Server
            string query = @"INSERT INTO DangKyHocVien (FullName, Phone, BirthDate, Gender, CourseId, IsStudent) 
                            VALUES (@FullName, @Phone, @BirthDate, @Gender, @CourseId, @IsStudent)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@CourseId", selectedCourseId);
                        cmd.Parameters.AddWithValue("@IsStudent", isStudent);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Đã lưu thông tin đăng ký của học viên '{fullName}' vào CSDL thành công!",
                                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Reset Form sau khi đăng ký thành công
                            txtFullName.Clear();
                            mtxtPhone.Clear();
                            chkAgree.Checked = false;
                            chkStudent.Checked = false;
                            txtFullName.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu vào CSDL: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}