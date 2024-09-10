namespace ApiDoctorCare.Models
{
    public class Admin
    {
        public int ID_Admin { get; set; }
        public string ChiTiet { get; set; }
        public int ID_TaiKhoan { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
    }
}