namespace ApiDoctorCare.Models
{
    public class LichLamViec
    {
        public int ID { get; set; }
        public int ID_BacSi { get; set; }
        public DateTime NgayLamViec { get; set; }
        public TimeSpan ThoiGianBatDau { get; set; }
        public TimeSpan ThoiGianKetThuc { get; set; }
        public string TrangThai { get; set; }
    }
}
