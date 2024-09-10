using Dapper;
using ApiDoctorCare.Models;
using Microsoft.Data.SqlClient;
using ApiDoctorCare.Services;

namespace ApiDoctorCare.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder builder)
        {
            // Check Đăng nhập phân quyền
            builder.MapPost("login", async (LoginDto loginDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Kiểm tra tài khoản và mật khẩu
                const string sqlAccount = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                var userExists = await connection.ExecuteScalarAsync<int>(sqlAccount, new { loginDto.TenDangNhap, loginDto.MatKhau });

                if (userExists == 0)
                {
                    return Results.Unauthorized(); // Hoặc dùng Results.Problem() nếu muốn có thông điệp
                }

                // Kiểm tra quyền hạn
                // Kiểm tra Admin
                const string sqlAdmin = "SELECT COUNT(*) FROM Admin WHERE ID_TaiKhoan = (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)";
                var isAdmin = await connection.ExecuteScalarAsync<int>(sqlAdmin, new { loginDto.TenDangNhap });
                if (isAdmin > 0)
                {
                    return Results.Ok(new { RoleId = 1, Message = "Đăng nhập thành công." });
                }

                // Kiểm tra Bác sĩ
                const string sqlDoctor = "SELECT COUNT(*) FROM BacSi WHERE ID_TaiKhoan = (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)";
                var isDoctor = await connection.ExecuteScalarAsync<int>(sqlDoctor, new { loginDto.TenDangNhap });
                if (isDoctor > 0)
                {
                    return Results.Ok(new { RoleId = 2, Message = "Đăng nhập thành công." });
                }

                // Kiểm tra Bệnh nhân
                const string sqlPatient = "SELECT COUNT(*) FROM BenhNhan WHERE ID_TaiKhoan = (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)";
                var isPatient = await connection.ExecuteScalarAsync<int>(sqlPatient, new { loginDto.TenDangNhap });
                if (isPatient > 0)
                {
                    return Results.Ok(new { RoleId = 3, Message = "Đăng nhập thành công." });
                }

                return Results.Unauthorized(); // Hoặc dùng Results.Problem() nếu muốn có thông điệp
            });


            // Check Đăng ký với tài khoản user
            builder.MapPost("register/partient", async (RegisterPartientDto registerPartientDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Kiểm tra tài khoản đã tồn tại chưa
                const string sqlCheckAccount = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                var accountExists = await connection.ExecuteScalarAsync<int>(sqlCheckAccount, new { registerPartientDto.TenDangNhap });

                if (accountExists > 0)
                {
                    return Results.BadRequest(new { Message = "Tài khoản đã tồn tại." });
                }

                // Thêm tài khoản
                const string sqlAddAccount = @"
                    INSERT INTO TaiKhoan(TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar) 
                    VALUES(@TenDangNhap, @MatKhau, @Email, @HoTen, @NgaySinh, @GioiTinh, @SDT, @DiaChi, @Avatar)";
                await connection.ExecuteAsync(sqlAddAccount, new
                {
                    registerPartientDto.TenDangNhap,
                    registerPartientDto.MatKhau,
                    registerPartientDto.Email,
                    registerPartientDto.HoTen,
                    registerPartientDto.NgaySinh,
                    registerPartientDto.GioiTinh,
                    registerPartientDto.SDT,
                    registerPartientDto.DiaChi,
                    registerPartientDto.Avatar
                });

                // Lấy ID_TaiKhoan vừa thêm
                const string sqlGetID = "SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                var idTaiKhoan = await connection.ExecuteScalarAsync<int>(sqlGetID, new { registerPartientDto.TenDangNhap });

                // Thêm thông tin bệnh nhân
                const string sqlAddPatient = @"
                    INSERT INTO BenhNhan(ID_TaiKhoan, TienSuBenh, DiUng) 
                    VALUES(@ID_TaiKhoan, @TienSuBenh, @DiUng)";
                await connection.ExecuteAsync(sqlAddPatient, new
                {
                    ID_TaiKhoan = idTaiKhoan,
                    registerPartientDto.TienSuBenh,
                    registerPartientDto.DiUng
                });

                return Results.Created("login", new { Message = "Đăng ký thành công." });
            });

        }
    }
}
