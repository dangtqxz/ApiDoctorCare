namespace ApiDoctorCare.Models
{
    public class TaiKhoan
    {
        public int ID_TaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string HoTen { get; set; }
        public string? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string Avatar { get; set; }
    }
}