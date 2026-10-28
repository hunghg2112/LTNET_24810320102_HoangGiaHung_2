# BÁO CÁO BÀI TẬP THỰC HÀNH LẬP TRÌNH C# WINFORMS

## I. THÔNG TIN SINH VIÊN
- **Họ và tên:** Hoàng Gia Hưng
- **Mã số sinh viên:** 24810320102
- **Lớp học phần:** D19QTANM1
- **Link Repository:** https://github.com/hunghg2112/LTNET_24810320102_HoangGiaHung_2

---

## II. DANH SÁCH BÀI TẬP HOÀN THÀNH & TỰ ĐÁNH GIÁ

| Bài tập | Tiêu đề bài tập | Trạng thái | Mức độ hoàn thành (%) |
| :---: | :--- | :---: | :---: |
| **Bài 1** | Form Đăng Nhập Hệ Thống & Kết Nối CSDL (Validation & ADO.NET) | Hoàn thành | 100% |
| **Bài 2** | Form Đăng Ký Khóa Học (MaskedTextBox, DateTimePicker, ComboBox Binding & CSDL) | Hoàn thành | 100% |

---

## III. TỔNG QUAN KỸ THUẬT NỔI BẬT

### 1. Bài 1: Form Đăng Nhập Hệ Thống & Kết Nối CSDL
- **Mục tiêu:** Xây dựng Form đăng nhập an toàn, xác thực thông tin tài khoản từ cơ sở dữ liệu SQL Server.
- **Kỹ thuật áp dụng:**
  - **Validation:** Sử dụng `ErrorProvider` để kiểm tra và cảnh báo trực quan khi để trống Tên đăng nhập hoặc Mật khẩu.
  - **Tương tác UI:** Gán `AcceptButton` (phím Enter kích hoạt Đăng nhập) và `CancelButton` (phím Esc kích hoạt Thoát).
  - **Ẩn/Hiện Mật khẩu:** Xử lý sự kiện `CheckedChanged` của CheckBox để bật/tắt `UseSystemPasswordChar`.
  - **Kết nối CSDL (ADO.NET):** Sử dụng `SqlConnection`, `SqlCommand` và `ExecuteScalar` kết hợp với SQL Parameters (`@User`, `@Pass`) để thực thi truy vấn `SELECT COUNT(*)` chống tấn công SQL Injection.

---

### 2. Bài 2: Form Đăng Ký Khóa Học (WinForms Controls & CSDL)
- **Mục tiêu:** Tổng hợp quản lý thông tin đăng ký học viên sử dụng đa dạng các Controls WinForms và lưu trữ vào CSDL.
- **Kỹ thuật áp dụng:**
  - **MaskedTextBox (`mtxtPhone`):** Cấu hình mặt nạ nhập liệu số điện thoại chuẩn `(000) 000-0000` và kiểm tra tính hợp lệ bằng `MaskCompleted`.
  - **DateTimePicker (`dtpBirthDate`):** Định dạng hiển thị ngày sinh theo chuẩn Việt Nam `CustomFormat = "dd/MM/yyyy"`.
  - **ComboBox DataBinding (`cboCourse`):** Đọc danh sách khóa học từ bảng `KhoaHoc` trong SQL Server qua `SqlDataReader`, thiết lập `DisplayMember = "CourseName"` (hiển thị tên) và `ValueMember = "CourseId"` (lấy mã ẩn).
  - **RadioButton & CheckBox:** Xử lý logic chọn giới tính, tính ưu đãi sinh viên và xác nhận điều khoản.
  - **Lưu CSDL:** Thực thi câu lệnh `INSERT INTO` để lưu thông tin học viên mới vào bảng `DangKyHocVien` an toàn qua SQL Parameters.# LTNET_24810320102_HoangGiaHung_1
