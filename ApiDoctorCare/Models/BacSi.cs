namespace ApiDoctorCare.Models
{
    public class BacSi
    {
        public int ID_BacSi { get; set; }
        public string MoTa { get; set; }
        public int ID_BangCap { get; set; }
        public BangCap BangCap { get; set; }
        public int ID_TaiKhoan { get; set; }
        public TaiKhoan TaiKhoan { get; set; }
        public int ID_ChuyenKhoa { get; set; }
        public ChuyenKhoa ChuyenKhoa { get; set; }
    }
}