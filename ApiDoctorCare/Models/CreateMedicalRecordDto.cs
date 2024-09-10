namespace ApiDoctorCare.Models
{
    public class CreateMedicalRecordDto
    {
        public int ID_BenhNhan { get; set; }
        public string NgayKham { get; set; }
        public string ChuanDoan { get; set; }
        public string HuongDieuTri { get; set; }
        public string GhiChu { get; set; }
        public bool KetQuaXetNghiem { get; set; }

    }
}
