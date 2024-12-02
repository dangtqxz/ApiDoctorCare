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
                    return Results.Ok(new { tenDangNhap = loginDto.TenDangNhap, matKhau = loginDto.MatKhau, RoleId = 1, Message = "Đăng nhập thành công." });
                }

                // Kiểm tra Bác sĩ
                const string sqlDoctor = "SELECT COUNT(*) FROM BacSi WHERE ID_TaiKhoan = (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)";
                var isDoctor = await connection.ExecuteScalarAsync<int>(sqlDoctor, new { loginDto.TenDangNhap });
                if (isDoctor > 0)
                {
                    return Results.Ok(new { tenDangNhap = loginDto.TenDangNhap, matKhau = loginDto.MatKhau, RoleId = 2, Message = "Đăng nhập thành công." });
                }

                // Kiểm tra Bệnh nhân
                const string sqlSupport = "SELECT COUNT(*) FROM HoTro WHERE ID_TaiKhoan = (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)";
                var isSupport = await connection.ExecuteScalarAsync<int>(sqlSupport, new { loginDto.TenDangNhap });
                if (isSupport > 0)
                {
                    return Results.Ok(new { tenDangNhap = loginDto.TenDangNhap, matKhau = loginDto.MatKhau, RoleId = 3, Message = "Đăng nhập thành công." });
                }

                return Results.Unauthorized(); // Hoặc dùng Results.Problem() nếu muốn có thông điệp
            });

            //Lấy thông tin tài khoản theo tên đăng nhập
            builder.MapPost("account", async (string TenDangNhap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    SELECT * FROM TaiKhoan
                    WHERE TenDangNhap = @TenDangNhap";
                var account = await connection.QueryFirstOrDefaultAsync<TaiKhoan>(sql, new
                {
                    TenDangNhap
                });
                return Results.Ok(account);
            });

            //Quên mật khẩu
            builder.MapPost("forgotpassword", async (string email, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string check = @"
                    SELECT COUNT(*) FROM TaiKhoan
                    WHERE Email = @Email";

                var checkEmail = await connection.ExecuteScalarAsync<int>(check, new
                {
                    Email = email
                });

                if (checkEmail == 0)
                {
                    return Results.NotFound();
                }

                const string sql = @"
                    SELECT TenDangNhap, MatKhau FROM TaiKhoan
                    WHERE Email = @Email";

                var taiKhoan = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new
                {
                    Email = email
                });

                if (taiKhoan == null)
                {
                    return Results.NotFound();
                }

                // Gửi thông tin tài khoản qua email
                EmailServices.SendAccout(taiKhoan.TenDangNhap, taiKhoan.MatKhau, email );
                return Results.Ok(taiKhoan);
            });
        }
    }
}
