using Dapper;
using ApiDoctorCare.Models;
using Microsoft.Data.SqlClient;
using ApiDoctorCare.Services;


namespace ApiDoctorCare.Endpoints
{
    public static class PartientEndPoints
    {
        public static void MapPartientEndPoints(this IEndpointRouteBuilder builder)
        {
            // Lấy thông tin bệnh nhân theo ID_TaiKhoan
            builder.MapGet("partient/{id}", async (int idTaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT t.TenDangNhap, t.Email, t.HoTen, t.NgaySinh, t.GioiTinh, t.SDT, t.DiaChi, t.Avatar, b.TienSuBenh, b.DiUng
                    FROM BenhNhan b
                    JOIN TaiKhoan t ON b.ID_TaiKhoan = t.ID_TaiKhoan
                    WHERE b.ID_TaiKhoan = @ID_TaiKhoan";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_TaiKhoan = idTaiKhoan });

                if (result == null || !result.Any())
                {
                    return Results.NotFound("Không tìm thấy bệnh nhân có id này.");
                }

                return Results.Ok(result);
            });

            // Cập nhật thông tin tài khoản bệnh nhân theo ID_TaiKhoan
            builder.MapPut("partient/{id}", async (int idTaiKhoan, UpdatePartientDto updatePartientDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    UPDATE TaiKhoan
                    SET
                        Email = @Email,
                        HoTen = @HoTen,
                        NgaySinh = @NgaySinh,
                        GioiTinh = @GioiTinh,
                        SDT = @SDT,
                        DiaChi = @DiaChi,
                        Avatar = @Avatar
                    WHERE ID_TaiKhoan = @ID_TaiKhoan";

                await connection.ExecuteAsync(sql, new
                {
                    ID_TaiKhoan = idTaiKhoan,
                    updatePartientDto.Email,
                    updatePartientDto.HoTen,
                    updatePartientDto.NgaySinh,
                    updatePartientDto.GioiTinh,
                    updatePartientDto.SDT,
                    updatePartientDto.DiaChi,
                    updatePartientDto.Avatar
                });

                return Results.Ok("Cập nhật thành công");
            });

            //Hiển thị thông tin bác sĩ rút gọn
            builder.MapGet("bacsi/short", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn thông tin bác sĩ cùng với tài khoản, bằng cấp
                const string sql = @"
                    SELECT  
                        tk.HoTen,
                        tk.GioiTinh,
                        tk.Avatar,
                        bc.TenBangCap
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN 
                        BangCap bc ON b.ID_BangCap = bc.ID_BangCap";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Hiển thị các chuyên khoa
            builder.MapGet("chuyenkhoa", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT 
                        ID_ChuyenKhoa,
                        TenChuyenKhoa
                    FROM 
                        ChuyenKhoa";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Hiển thị thông tin bác sĩ theo id chuyên khoa
            builder.MapGet("bacsi/chuyen-khoa/{idChuyenKhoa}", async (int idChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn bác sĩ theo chuyên khoa
                const string sql = @"
                    SELECT
                        tk.HoTen,
                        tk.GioiTinh,
                        tk.Avatar,
                        b.MoTa,
                        bc.TenBangCap
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN 
                        BangCap bc ON b.ID_BangCap = bc.ID_BangCap
                    WHERE 
                        b.ID_ChuyenKhoa = @ID_ChuyenKhoa";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_ChuyenKhoa = idChuyenKhoa });

                if (result == null || !result.Any())
                {
                    return Results.NotFound("Không tìm thấy bác sĩ cho chuyên khoa này.");
                }

                return Results.Ok(result);
            });

            // Lấy thông tin lịch làm việc của bác sĩ theo ID và ngày
            builder.MapGet("bacsi/{id}/lichlamviec", async (int id, DateTime ngayLamViec, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT
                        lv.NgayLamViec,
                        lv.ThoiGianBatDau,
                        lv.ThoiGianKetThuc,
                        lv.TrangThai
                    FROM 
                        LichLamViec lv
                    JOIN 
                        BacSi b ON lv.ID_BacSi = b.ID_BacSi
                    WHERE 
                        b.ID_BacSi = @ID_BacSi AND 
                        lv.NgayLamViec = @NgayLamViec";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_BacSi = id, NgayLamViec = ngayLamViec });

                if (result == null || !result.Any())
                {
                    return Results.NotFound("Không tìm thấy lịch làm việc của bác sĩ vào ngày này.");
                }

                return Results.Ok(result);
            });

            // Nhập thông tin lịch hẹn
            builder.MapPost("lichhen", async (LichHenDto lichHenDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Kiểm tra lịch hẹn đã tồn tại chưa
                const string sqlCheckLichHen = "SELECT COUNT(*) FROM LichHen WHERE ID_BacSi = @ID_BacSi AND NgayHen = @NgayHen AND GioHen = @GioHen";
                var lichHenExists = await connection.ExecuteScalarAsync<int>(sqlCheckLichHen, new { lichHenDto.ID_BacSi, lichHenDto.NgayHen, lichHenDto.GioHen });

                if (lichHenExists > 0)
                {
                    return Results.BadRequest("Lịch hẹn đã tồn tại.");
                }

                // Thêm lịch hẹn
                const string sqlAddLichHen = @"
                    INSERT INTO LichHen(ID_BacSi, ID_BenhNhan, NgayHen, GioHen, TrangThai, GhiChu, TrieuChung)
                    VALUES(@ID_BacSi, @ID_BenhNhan, @NgayHen, @GioHen, @TrangThai, @GhiChu, @TrieuChung)";
                await connection.ExecuteAsync(sqlAddLichHen, new
                {
                    lichHenDto.ID_BacSi,
                    lichHenDto.ID_BenhNhan,
                    lichHenDto.NgayHen,
                    lichHenDto.GioHen,
                    lichHenDto.TrangThai,
                    lichHenDto.GhiChu,
                    lichHenDto.TrieuChung
                });

                return Results.Ok("Thêm lịch hẹn thành công.");
            });

            //Xem thông tin lịch hẹn theo ID_BenhNhan
            builder.MapGet("lichhen/{id}", async (int idBenhNhan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT 
                        l.NgayHen, l.GioHen, t.HoTen as TenBacSi, l.TrieuChung, l.GhiChu, l.TrangThai FROM LichHen l 
                        Join BacSi b On l.ID_BacSi = b.ID_BacSi
                        Join TaiKhoan t On t.ID_TaiKhoan = b.ID_TaiKhoan
                        Where ID_BenhNhan = @ID_BenhNhan";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_BenhNhan = idBenhNhan });

                if (result == null || !result.Any())
                {
                    return Results.NotFound("Không tìm thấy lịch hẹn của bệnh nhân này.");
                }

                return Results.Ok(result);
            });

            //Cập nhật trạng thái lịch hẹn
            builder.MapPut("lichhen/{id}", async (int id, UpdateLichHenDto updateLichHenDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    UPDATE LichHen
                    SET
                        TrangThai = @TrangThai
                    WHERE ID_LichHen = @ID_LichHen";

                await connection.ExecuteAsync(sql, new
                {
                    ID_LichHen = id,
                    updateLichHenDto.TrangThai
                });

                return Results.Ok("Cập nhật trạng thái lịch hẹn thành công.");
            });
        }
    }
}
