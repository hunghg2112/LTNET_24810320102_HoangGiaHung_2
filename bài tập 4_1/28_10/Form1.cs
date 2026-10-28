using System;
using System.Data;
using System.Data.SqlClient; // Nếu dùng MySQL thì đổi thành MySql.Data.MySqlClient
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyDangNhap
{
    public partial class Form1 : Form
    {
        // Khai báo các Controls theo đúng tên yêu cầu
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Button btnExit;
        private ErrorProvider errorProvider;

        // Chuỗi kết nối CSDL (Ví dụ dùng SQL Server LocalDB hoặc SQL Express)
        // Nếu dùng MySQL (XAMPP): "Server=localhost;Database=QuanLyDB;Uid=root;Pwd=;"
        private string connectionString = @"Server=localhost\SQLEXPRESS;Database=QLTK;User Id=sa;Password=_Hung2112;TrustServerCertificate=True;"; public Form1()
        {
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            // Thiết lập Form
            this.Text = "Đăng nhập hệ thống";
            this.Size = new Size(380, 240);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9.5F);

            // Khởi tạo ErrorProvider
            errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            // --- 1. NHẬP TÊN ĐĂNG NHẬP ---
            this.Controls.Add(new Label { Text = "Tên đăng nhập:", Location = new Point(30, 30), AutoSize = true });
            txtUsername = new TextBox { Location = new Point(140, 27), Width = 180 };
            this.Controls.Add(txtUsername);

            // --- 2. NHẬP MẬT KHẨU ---
            this.Controls.Add(new Label { Text = "Mật khẩu:", Location = new Point(30, 70), AutoSize = true });
            txtPassword = new TextBox
            {
                Location = new Point(140, 67),
                Width = 180,
                UseSystemPasswordChar = true // Mặc định ẩn mật khẩu dạng (*)
            };
            this.Controls.Add(txtPassword);

            // --- 3. CHECKBOX ẨN/HIỆN MẬT KHẨU ---
            chkShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Location = new Point(140, 100),
                AutoSize = true
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            this.Controls.Add(chkShowPassword);

            // --- 4. NÚT ĐĂNG NHẬP & THOÁT ---
            btnLogin = new Button { Text = "Đăng nhập", Location = new Point(140, 135), Width = 85, Height = 30 };
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            btnExit = new Button { Text = "Thoát", Location = new Point(235, 135), Width = 85, Height = 30 };
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);

            // --- YÊU CẦU PHÍM TẮT: Enter = Login, Esc = Exit ---
            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
        }

        // --- XỬ LÝ CHÍNH NĂNG ẨN/HIỆN MẬT KHẨU ---
        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Tick chọn -> hiện mật khẩu (false). Bỏ chọn -> ẩn mật khẩu (true)
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        // --- XỬ LÝ NÚT THOÁT ---
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --- XỬ LÝ NÚT ĐĂNG NHẬP + VALIDATION + KẾT NỐI CSDL ---
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra Validation bằng ErrorProvider
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(txtUsername, "Tên đăng nhập không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Mật khẩu không được để trống!");
                isValid = false;
            }

            if (!isValid) return; // Nếu trống thì dừng lại, không gọi CSDL

            // 2. Truy vấn Cơ sở dữ liệu kiểm tra tài khoản
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (KiemTraDangNhapCSDL(user, pass))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Mở Form chính ứng dụng tại đây (nếu có)
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // --- HÀM TRUY VẤN CSDL BẰNG ADO.NET (CHỐNG SQL INJECTION) ---
        private bool KiemTraDangNhapCSDL(string username, string password)
        {
            // Cấu trúc câu lệnh SQL kiểm tra sự tồn tại của User
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @User AND Password = @Pass";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Truyền tham số Param để bảo mật
                        cmd.Parameters.AddWithValue("@User", username);
                        cmd.Parameters.AddWithValue("@Pass", password);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0; // Trả về true nếu tìm thấy 1 tài khoản khớp
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu chưa tạo CSDL hoặc sai chuỗi kết nối, báo lỗi ra màn hình
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}