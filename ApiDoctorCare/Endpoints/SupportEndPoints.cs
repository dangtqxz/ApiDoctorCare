using Dapper;
using ApiDoctorCare.Models;
using Microsoft.Data.SqlClient;
using ApiDoctorCare.Services;

namespace ApiDoctorCare.Endpoints
{
    public static class SupportEndPoints
    {
        public static void MapSupportEndPoints(this IEndpointRouteBuilder builder)
        {
            //Lấy tất cả lịch hẹn
            builder.MapPost("hotro/lichhen", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT 
                        lh.ID_LichHen,
                        lh.NgayHen,
                        lh.GioHen,
                        lh.TrieuChung,
                        lh.GhiChu,
                        lh.TrangThai,
	                    tk.HoTen as TenBacSi,
	                    tk2.HoTen as TenBenhNhan,
                        tk2.SDT as SoDienThoai
                    FROM 
                        LichHen lh
                    JOIN
	                    BacSi bs ON bs.ID_BacSi = lh.ID_BacSi
                    JOIN
	                    BenhNhan bn ON bn.ID_BenhNhan = lh.ID_BenhNhan
                    JOIN
	                    TaiKhoan tk ON bs.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN
	                    TaiKhoan tk2 ON bn.ID_TaiKhoan = tk2.ID_TaiKhoan";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Sửa trạng thái lịch hẹn
            builder.MapPost("hotro/lichhen/trangthai", async (int idLichHen, UpdateLichHenDto updateLichHenDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn
                const string sql = @"
                    UPDATE LichHen 
                    SET
                        TrangThai = @TrangThai
                    WHERE
                        ID_LichHen = @ID_LichHen";

                await connection.ExecuteAsync(sql, new
                {
                    ID_LichHen = idLichHen,
                    updateLichHenDto.TrangThai
                });

                return Results.Ok("Cập nhật trạng thái lịch hẹn thành công.");
            });

        }
    }
}
