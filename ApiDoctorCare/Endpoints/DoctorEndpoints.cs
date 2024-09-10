using ApiDoctorCare.Models;
using ApiDoctorCare.Services;
using Dapper;

namespace ApiDoctorCare.Endpoints
{
    public static class DoctorEndpoints
    {
        public static void MapDoctorEndpoints(this IEndpointRouteBuilder builder)
        {
            //Đăng ký lịch làm việc
            builder.MapPost("doctor/register-schedule", async (RegisterScheduleDto registerScheduleDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    INSERT INTO LichLamViec(ID_BacSi, NgayLamViec, ThoiGianBatDau, ThoiGianKetThuc, TrangThai)
                    VALUES(@ID_BacSi, @NgayLamViec, @ThoiGianBatDau, @ThoiGianKetThuc, @TrangThai)";

                await connection.ExecuteAsync(sql, new
                {
                    registerScheduleDto.ID_BacSi,
                    registerScheduleDto.NgayLamViec,
                    registerScheduleDto.ThoiGianBatDau,
                    registerScheduleDto.ThoiGianKetThuc,
                    registerScheduleDto.TrangThai
                });

                return Results.Ok("Đăng ký lịch làm việc thành công");
            });

            //Xem lịch làm việc theo id bác sĩ
            builder.MapGet("doctor/schedule", async (int ID_BacSi, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT * FROM LichLamViec
                    WHERE ID_BacSi = @ID_BacSi";

                var schedule = await connection.QueryAsync<LichLamViec>(sql, new
                {
                    ID_BacSi
                });

                return Results.Ok(schedule);
            });

            //Xem lịch làm việc theo ngày và id bác sĩ
            builder.MapGet("doctor/schedule/date", async (DateTime NgayLamViec, int ID_BacSi, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT * FROM LichLamViec
                    WHERE NgayLamViec = @NgayLamViec AND ID_BacSi = @ID_BacSi";

                var schedule = await connection.QueryFirstOrDefaultAsync<LichLamViec>(sql, new
                {
                    NgayLamViec,
                    ID_BacSi
                });

                if (schedule == null)
                {
                    return Results.NotFound("Không tìm thấy lịch làm việc");
                }

                return Results.Ok(schedule);
            });

            //Xem thông tin lịch hẹn theo ID_BacSi
            builder.MapGet("doctor/lichhen/{id}", async (int idBacSi, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT 
                        l.NgayHen, l.GioHen, t.HoTen as TenBenhNhan, l.TrieuChung, l.GhiChu, l.TrangThai FROM LichHen l 
                        Join BenhNhan b On l.ID_BenhNhan = b.ID_BenhNhan
                        Join TaiKhoan t On t.ID_TaiKhoan = b.ID_TaiKhoan
                        Where ID_BacSi = @ID_BacSi";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_BacSi = idBacSi });

                if (result == null || !result.Any())
                {
                    return Results.NotFound("Không tìm thấy lịch hẹn của bác sĩ này.");
                }

                return Results.Ok(result);
            });

            // Tạo hồ sơ bệnh án
            builder.MapPost("doctor/create-medical-record", async (CreateMedicalRecordDto createMedicalRecordDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    INSERT INTO HoSoBenhAn(ID_BenhNhan, NgayKham, ChuanDoan, HuongDieuTri, GhiChu, KetQuaXetNghiem)
                    VALUES(@ID_BenhNhan, @NgayKham, @ChuanDoan, @HuongDieuTri, @GhiChu, @KetQuaXetNghiem)";

                await connection.ExecuteAsync(sql, new
                {
                    createMedicalRecordDto.ID_BenhNhan,
                    createMedicalRecordDto.NgayKham,
                    createMedicalRecordDto.ChuanDoan,
                    createMedicalRecordDto.HuongDieuTri,
                    createMedicalRecordDto.GhiChu,
                    createMedicalRecordDto.KetQuaXetNghiem
                });

                return Results.Ok("Tạo hồ sơ bệnh án thành công");
            });
        }
    }
}
