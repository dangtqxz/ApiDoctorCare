namespace ApiDoctorCare.Models
{
    public class BenhNhan
    {
        public int ID_BenhNhan { get; set; }
        public string TienSuBenh { get; set; }
        public string DiUng { get; set; }
        public int ID_TaiKhoan { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
    }
}