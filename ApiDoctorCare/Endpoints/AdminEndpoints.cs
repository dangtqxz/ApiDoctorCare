using Dapper;
using ApiDoctorCare.Models;
using Microsoft.Data.SqlClient;
using ApiDoctorCare.Services;

namespace ApiDoctorCare.Endpoints
{
    public static class AdminEndpoints
    {
        public static void MapAdminEndpoints(this IEndpointRouteBuilder builder)
        {
            //Lấy thông tin thuốc
            builder.MapPost("admin/thuoc", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin thuốc
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        Thuoc";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Thêm thuốc
            builder.MapPost("admin/thuoc/themmoi", async (string TenThuoc, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO
                        Thuoc (TenThuoc, TrangThai)
                    VALUES (@TenThuoc, 1) ";
                var parameters = new { TenThuoc };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy thông tin thuốc theo ID
            builder.MapPost("admin/thuoc/id", async (int ID_Thuoc, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        Thuoc
                    WHERE
                        ID_Thuoc = @ID_Thuoc";
                var parameters = new { ID_Thuoc };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Sửa thông tin thuốc
            builder.MapPost("admin/thuoc/sua", async (int ID_Thuoc, string TenThuoc, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE Thuoc 
                    SET
                        TenThuoc = @TenThuoc
                    WHERE
                        ID_Thuoc = @ID_Thuoc";
                var parameters = new { ID_Thuoc, TenThuoc };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Xóa thuốc
            builder.MapPost("admin/thuoc/xoa", async (int ID_Thuoc, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE Thuoc 
                    SET
                        TrangThai = 0
                    WHERE
                        ID_Thuoc = @ID_Thuoc";

                var parameters = new { ID_Thuoc };

                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Lấy thông tin chuyên khoa
            builder.MapPost("admin/chuyenkhoa", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin chuyên khoa
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        ChuyenKhoa";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Thêm chuyên khoa
            builder.MapPost("admin/chuyenkhoa/themmoi", async (string TenChuyenKhoa, string MoTa, string AnhURL, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO
                        ChuyenKhoa (TenChuyenKhoa, MoTa, AnhURL, TrangThai)
                    VALUES (@TenChuyenKhoa, @MoTa, @AnhURL, 1) ";
                var parameters = new { TenChuyenKhoa, MoTa, AnhURL };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy thông tin chuyên khoa theo ID
            builder.MapPost("admin/chuyenkhoa/id", async (int ID_ChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        ChuyenKhoa
                    WHERE
                        ID_ChuyenKhoa = @ID_ChuyenKhoa";
                var parameters = new { ID_ChuyenKhoa };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Sửa thông tin chuyên khoa
            builder.MapPost("admin/chuyenkhoa/sua", async (int ID_ChuyenKhoa, string TenChuyenKhoa, string MoTa, string AnhURL, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE ChuyenKhoa 
                    SET
                        TenChuyenKhoa = @TenChuyenKhoa,
                        MoTa = @MoTa,
                        AnhURL = @AnhURL
                    WHERE
                        ID_ChuyenKhoa = @ID_ChuyenKhoa";
                var parameters = new { ID_ChuyenKhoa, TenChuyenKhoa, MoTa, AnhURL };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Xóa chuyên khoa
            builder.MapPost("admin/chuyenkhoa/xoa", async (int ID_ChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE ChuyenKhoa 
                    SET
                        TrangThai = 0
                    WHERE
                        ID_ChuyenKhoa = @ID_ChuyenKhoa";
                var parameters = new { ID_ChuyenKhoa };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Lấy thông tin bằng cấp
            builder.MapPost("admin/bangcap", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin bằng cấp
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        BangCap";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Thêm bằng cấp
            builder.MapPost("admin/bangcap/themmoi", async (string TenBangCap, string MoTa, string HeSoLuong, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO
                        BangCap (TenBangCap, MoTa, HeSoLuong, TrangThai)
                    VALUES (@TenBangCap, @MoTa, @HeSoLuong, 1) ";
                var parameters = new { TenBangCap, MoTa, HeSoLuong };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy thông tin bằng cấp theo ID
            builder.MapPost("admin/bangcap/id", async (int ID_BangCap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        BangCap
                    WHERE
                        ID_BangCap = @ID_BangCap";
                var parameters = new { ID_BangCap };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Sửa thông tin bằng cấp
            builder.MapPost("admin/bangcap/sua", async (int ID_BangCap, string TenBangCap, string MoTa, string HeSoLuong, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE BangCap 
                    SET
                        TenBangCap = @TenBangCap,
                        MoTa = @MoTa,
                        HeSoLuong = @HeSoLuong
                    WHERE
                        ID_BangCap = @ID_BangCap";
                var parameters = new { ID_BangCap, TenBangCap, MoTa, HeSoLuong };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Xoá bằng cấp
            builder.MapPost("admin/bangcap/xoa", async (int ID_BangCap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE BangCap 
                    SET
                        TrangThai = 0
                    WHERE
                        ID_BangCap = @ID_BangCap";
                var parameters = new { ID_BangCap };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Lấy thông tin tài khoản
            builder.MapPost("admin/taikhoan", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin tài khoản
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        TaiKhoan
                    WHERE TenDangNhap != ''";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            builder.MapPost("admin/taikhoan/id", async (string ID_TaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin tài khoản
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        TaiKhoan
                    WHERE ID_TaiKhoan = @ID_TaiKhoan";

                var parameters = new { ID_TaiKhoan };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Thông tin thêm về tài khoản admin 
            builder.MapPost("admin/taikhoan/admin/id", async (string ID_TaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin tài khoản
                const string sql = @"
                    SELECT 
                        am.ID_Admin,
                        tk.ID_TaiKhoan,
                        am.ChiTiet
                    FROM 
                        Admin am
                    JOIN
                        TaiKhoan tk ON am.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE am.ID_TaiKhoan = @ID_TaiKhoan";

                var parameters = new { ID_TaiKhoan };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Thông tin thêm về tài khoản bác sĩ
            builder.MapPost("admin/taikhoan/bacsi/id", async (string ID_TaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin tài khoản
                const string sql = @"
                    SELECT 
                        bs.ID_BacSi,
                        bs.MoTa,
                        ck.TenChuyenKhoa,
                        bc.TenBangCap
                    FROM 
                        BacSi bs
                    JOIN
                        TaiKhoan tk ON bs.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN
                        ChuyenKhoa ck ON bs.ID_ChuyenKhoa = ck.ID_ChuyenKhoa
                    JOIN
                        BangCap bc ON bs.ID_BangCap = bc.ID_BangCap
                    WHERE bs.ID_TaiKhoan = @ID_TaiKhoan";
                var parameters = new { ID_TaiKhoan };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Thông tin thêm về tài khoản hỗ trợ
            builder.MapPost("admin/taikhoan/hotro/id", async (string ID_TaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin tài khoản
                const string sql = @"
                    SELECT 
                        ht.ID_HoTro,
                        ht.MoTa
                    FROM 
                        HoTro ht
                    JOIN
                        TaiKhoan tk ON ht.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE ht.ID_TaiKhoan = @ID_TaiKhoan";
                var parameters = new { ID_TaiKhoan };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Sửa tài khoản admin
            builder.MapPost("admin/taikhoan/admin/sua", async (int ID_TaiKhoan, string TenDangNhap, string MatKhau, string Email, string HoTen, string ChiTiet, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    UPDATE TaiKhoan 
                        SET
                            TenDangNhap = @TenDangNhap,
                            MatKhau = @MatKhau,
                            Email = @Email,
                            HoTen = @HoTen,
                            NgaySinh = @NgaySinh,
                            GioiTinh = @GioiTinh,
                            SDT = @SDT,
                            DiaChi = @DiaChi,
                            Avatar = @Avatar
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var parameters = new { ID_TaiKhoan, TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                const string checkbs = @"
                    SELECT 
                        *
                    FROM 
                        Bacsi
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkParameters = new { ID_TaiKhoan };
                var checkResult = await connection.QueryAsync<dynamic>(checkbs, checkParameters);

                if (checkResult.Count() > 0)
                {
                    const string deletebs = @"
                        DELETE FROM BacSi
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deleteParameters = new { ID_TaiKhoan };
                    var deleteResult = await connection.QueryAsync<dynamic>(deletebs, deleteParameters);

                    const string addadmin = @"
                        INSERT INTO
                            Admin (ID_TaiKhoan, ChiTiet)
                        VALUES (@ID_TaiKhoan, @ChiTiet)";
                    var addParameters = new { ID_TaiKhoan, ChiTiet };
                    var addResult = await connection.QueryAsync<dynamic>(addadmin, addParameters);
                    return Results.Ok();
                }

                const string checkht = @"
                    SELECT 
                        *
                    FROM 
                        HoTro
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkhtParameters = new { ID_TaiKhoan };
                var checkhtResult = await connection.QueryAsync<dynamic>(checkht, checkhtParameters);

                if (checkhtResult.Count() > 0)
                {
                    const string deleteht = @"
                        DELETE FROM HoTro
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deletehtParameters = new { ID_TaiKhoan };
                    var deletehtResult = await connection.QueryAsync<dynamic>(deleteht, deletehtParameters);
                    const string addadmin = @"
                        INSERT INTO
                            Admin (ID_TaiKhoan, ChiTiet)
                        VALUES (@ID_TaiKhoan, @ChiTiet)";
                    var addhtParameters = new { ID_TaiKhoan, ChiTiet };
                    var addhtResult = await connection.QueryAsync<dynamic>(addadmin, addhtParameters);
                    return Results.Ok();
                }

                const string updateadmin = @"
                    UPDATE Admin
                    SET
                        ChiTiet = @ChiTiet
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var updateParameters = new { ID_TaiKhoan, ChiTiet };
                var updateResult = await connection.QueryAsync<dynamic>(updateadmin, updateParameters);
                return Results.Ok();
            });

            //sửa tài khoản bác sĩ
            builder.MapPost("admin/taikhoan/bacsi/sua", async (int ID_TaiKhoan, string TenDangNhap, string MatKhau, string Email, string HoTen, string MoTa, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, int ID_ChuyenKhoa, int ID_BangCap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    UPDATE TaiKhoan 
                        SET
                            TenDangNhap = @TenDangNhap,
                            MatKhau = @MatKhau,
                            Email = @Email,
                            HoTen = @HoTen,
                            NgaySinh = @NgaySinh,
                            GioiTinh = @GioiTinh,
                            SDT = @SDT,
                            DiaChi = @DiaChi,
                            Avatar = @Avatar
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var parameters = new { ID_TaiKhoan, TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                const string checkadmin = @"
                    SELECT 
                        *
                    FROM 
                        Admin
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkParameters = new { ID_TaiKhoan };
                var checkResult = await connection.QueryAsync<dynamic>(checkadmin, checkParameters);
                if (checkResult.Count() > 0)
                {
                    const string deleteadmin = @"
                        DELETE FROM Admin
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deleteParameters = new { ID_TaiKhoan };
                    var deleteResult = await connection.QueryAsync<dynamic>(deleteadmin, deleteParameters);
                    
                    const string addbs = @"
                        INSERT INTO
                            BacSi (ID_TaiKhoan, MoTa, ID_ChuyenKhoa, ID_BangCap)
                        VALUES (@ID_TaiKhoan, @MoTa, @ID_ChuyenKhoa, @ID_BangCap)";
                    var addParameters = new
                    {
                        ID_TaiKhoan,
                        MoTa,
                        ID_ChuyenKhoa,
                        ID_BangCap,
                    };
                    var addResult = await connection.QueryAsync<dynamic>(addbs, addParameters);
                    return Results.Ok();
                }

                const string checkht = @"
                    SELECT 
                        *
                    FROM 
                        HoTro
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkhtParameters = new { ID_TaiKhoan };
                var checkhtResult = await connection.QueryAsync<dynamic>(checkht, checkhtParameters);

                if (checkhtResult.Count() > 0)
                {
                    const string deleteht = @"
                        DELETE FROM HoTro
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deletehtParameters = new { ID_TaiKhoan };
                    var deletehtResult = await connection.QueryAsync<dynamic>(deleteht, deletehtParameters);
                    const string addbs = @"
                        INSERT INTO
                            BacSi (ID_TaiKhoan, MoTa, ID_ChuyenKhoa, ID_BangCap)
                        VALUES (@ID_TaiKhoan, @MoTa, @ID_ChuyenKhoa, @ID_BangCap)";
                    var addhtParameters = new
                    {
                        ID_TaiKhoan,
                        MoTa,
                        ID_ChuyenKhoa,
                        ID_BangCap,
                    };
                    var addhtResult = await connection.QueryAsync<dynamic>(addbs, addhtParameters);
                    return Results.Ok();
                }

                const string updatebs = @"
                    UPDATE BacSi
                    SET
                        MoTa = @MoTa,
                        ID_ChuyenKhoa = @ID_ChuyenKhoa,
                        ID_BangCap = @ID_BangCap
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var updateParameters = new
                {
                    ID_TaiKhoan,
                    MoTa,
                    ID_ChuyenKhoa,
                    ID_BangCap,
                };
                var updateResult = await connection.QueryAsync<dynamic>(updatebs, updateParameters);
                return Results.Ok();
            });

            //Sửa tài khoản hỗ trợ
            builder.MapPost("admin/taikhoan/hotro/sua", async (int ID_TaiKhoan, string TenDangNhap, string MatKhau, string Email, string HoTen, string MoTa, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    UPDATE TaiKhoan 
                        SET
                            TenDangNhap = @TenDangNhap,
                            MatKhau = @MatKhau,
                            Email = @Email,
                            HoTen = @HoTen,
                            NgaySinh = @NgaySinh,
                            GioiTinh = @GioiTinh,
                            SDT = @SDT,
                            DiaChi = @DiaChi,
                            Avatar = @Avatar
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var parameters = new { ID_TaiKhoan, TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                const string checkadmin = @"
                    SELECT 
                        *
                    FROM 
                        Admin
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkParameters = new { ID_TaiKhoan };
                var checkResult = await connection.QueryAsync<dynamic>(checkadmin, checkParameters);
                if (checkResult.Count() > 0)
                {
                    const string deleteadmin = @"
                        DELETE FROM Admin
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deleteParameters = new { ID_TaiKhoan };
                    var deleteResult = await connection.QueryAsync<dynamic>(deleteadmin, deleteParameters);

                    const string addht = @"
                        INSERT INTO
                            HoTro (ID_TaiKhoan, MoTa)
                        VALUES (@ID_TaiKhoan, @MoTa)";
                    var addParameters = new { ID_TaiKhoan, MoTa };
                    var addResult = await connection.QueryAsync<dynamic>(addht, addParameters);
                    return Results.Ok();
                }

                const string checkbs = @"
                    SELECT 
                        *
                    FROM
                        BacSi
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var checkbsParameters = new { ID_TaiKhoan };
                var checkbsResult = await connection.QueryAsync<dynamic>(checkbs, checkbsParameters);

                if (checkbsResult.Count() > 0)
                {
                    const string deletebs = @"
                        DELETE FROM BacSi
                        WHERE
                            ID_TaiKhoan = @ID_TaiKhoan";
                    var deleteParameters = new { ID_TaiKhoan };
                    var deleteResult = await connection.QueryAsync<dynamic>(deletebs, deleteParameters);

                    const string addht = @"
                        INSERT INTO
                            HoTro (ID_TaiKhoan, MoTa)
                        VALUES (@ID_TaiKhoan, @MoTa)";
                    var addParameters = new { ID_TaiKhoan, MoTa };
                    var addResult = await connection.QueryAsync<dynamic>(addht, addParameters);
                    return Results.Ok();
                }

                const string updateht = @"
                    UPDATE HoTro
                    SET
                        MoTa = @MoTa
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";
                var updateParameters = new { ID_TaiKhoan, MoTa };
                var updateResult = await connection.QueryAsync<dynamic>(updateht, updateParameters);
                return Results.Ok();
            });

            //Thêm Tài Khoản Admin
            builder.MapPost("admin/taikhoan/admin/them", async (string TenDangNhap, string MatKhau, string Email, string HoTen, string ChiTiet, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, SqlConnectionFactory sqlConnectionFactory) =>
            {

                if (!DateTime.TryParseExact(NgaySinh, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedNgaySinh))
                {
                    return Results.BadRequest("Invalid date format for NgaySinh. Please use dd/MM/yyyy.");
                }

                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO TaiKhoan(TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar, TrangThai)
                    VALUES (@TenDangNhap, @MatKhau, @Email, @HoTen, @NgaySinh, @GioiTinh, @SDT, @DiaChi, @Avatar, 1)";

                var parameters = new {
                    TenDangNhap,
                    MatKhau,
                    Email,
                    HoTen,
                    NgaySinh = parsedNgaySinh,
                    GioiTinh,
                    SDT,
                    DiaChi,
                    Avatar
                };

                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                const string sql2 = @"
                    SELECT ID_TaiKhoan
                    FROM TaiKhoan
                    WHERE TenDangNhap = @TenDangNhap";
                var parameters2 = new { TenDangNhap };
                var result2 = await connection.QueryAsync<dynamic>(sql2, parameters2);

                const string sql3 = @"
                    INSERT INTO Admin(ID_TaiKhoan, ChiTiet)
                    VALUES (@ID_TaiKhoan, @ChiTiet)";
                var parameters3 = new { ID_TaiKhoan = result2.First().ID_TaiKhoan, ChiTiet };
                var result3 = await connection.QueryAsync<dynamic>(sql3, parameters3);

                return Results.Ok();
            });

            //Thêm Tài Khoản Bác Sĩ
            builder.MapPost("admin/taikhoan/bacsi/them", async (string TenDangNhap, string MatKhau, string Email, string HoTen, string MoTa, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, int ID_ChuyenKhoa, int ID_BangCap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                if (!DateTime.TryParseExact(NgaySinh, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedNgaySinh))
                {
                    return Results.BadRequest("Invalid date format for NgaySinh. Please use dd/MM/yyyy.");
                }
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO TaiKhoan(TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar, TrangThai)
                    VALUES (@TenDangNhap, @MatKhau, @Email, @HoTen, @NgaySinh, @GioiTinh, @SDT, @DiaChi, @Avatar, 1)";
                var parameters = new
                {
                    TenDangNhap,
                    MatKhau,
                    Email,
                    HoTen,
                    NgaySinh = parsedNgaySinh,
                    GioiTinh,
                    SDT,
                    DiaChi,
                    Avatar
                };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                const string sql2 = @"
                    SELECT ID_TaiKhoan
                    FROM TaiKhoan
                    WHERE TenDangNhap = @TenDangNhap";
                var parameters2 = new { TenDangNhap };
                var result2 = await connection.QueryAsync<dynamic>(sql2, parameters2);
                const string sql3 = @"
                    INSERT INTO BacSi(ID_TaiKhoan, MoTa, ID_ChuyenKhoa, ID_BangCap)
                    VALUES (@ID_TaiKhoan, @MoTa, @ID_ChuyenKhoa, @ID_BangCap)";
                var parameters3 = new { ID_TaiKhoan = result2.First().ID_TaiKhoan, MoTa, ID_ChuyenKhoa, ID_BangCap };
                var result3 = await connection.QueryAsync<dynamic>(sql3, parameters3);
                return Results.Ok();
            });

            //Thêm Tài Khoản Hỗ Trợ
            builder.MapPost("admin/taikhoan/hotro/them", async (string TenDangNhap, string MatKhau, string Email, string HoTen, string MoTa, string NgaySinh, string GioiTinh, string SDT, string DiaChi, string Avatar, SqlConnectionFactory sqlConnectionFactory) =>
            {
                if (!DateTime.TryParseExact(NgaySinh, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedNgaySinh))
                {
                    return Results.BadRequest("Invalid date format for NgaySinh. Please use dd/MM/yyyy.");
                }
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    INSERT INTO TaiKhoan(TenDangNhap, MatKhau, Email, HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Avatar, TrangThai)
                    VALUES (@TenDangNhap, @MatKhau, @Email, @HoTen, @NgaySinh, @GioiTinh, @SDT, @DiaChi, @Avatar, 1)";
                var parameters = new
                {
                    TenDangNhap,
                    MatKhau,
                    Email,
                    HoTen,
                    NgaySinh = parsedNgaySinh,
                    GioiTinh,
                    SDT,
                    DiaChi,
                    Avatar
                };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                const string sql2 = @"
                    SELECT ID_TaiKhoan
                    FROM TaiKhoan
                    WHERE TenDangNhap = @TenDangNhap";
                var parameters2 = new { TenDangNhap };
                var result2 = await connection.QueryAsync<dynamic>(sql2, parameters2);
                const string sql3 = @"
                    INSERT INTO HoTro(ID_TaiKhoan, MoTa)
                    VALUES (@ID_TaiKhoan, @MoTa)";
                var parameters3 = new { ID_TaiKhoan = result2.First().ID_TaiKhoan, MoTa };
                var result3 = await connection.QueryAsync<dynamic>(sql3, parameters3);
                return Results.Ok();
            });





























            //Xóa tài khoản
            builder.MapPost("admin/taikhoan/xoa", async (int ID_TaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE TaiKhoan 
                    SET
                        TrangThai = 0
                    WHERE
                        ID_TaiKhoan = @ID_TaiKhoan";

                var parameters = new { ID_TaiKhoan };

                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Danh sách bênh nhan
            builder.MapPost("admin/dsbenhnhan", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn
                const string sql = @"
                    SELECT
                        bn.ID_BenhNhan,
                        MAX(tk2.HoTen) AS TenBenhNhan,
                        MAX(bn.TienSuBenh) AS TienSuBenh,
                        MAX(bn.DiUng) AS DiUng
                    FROM
                        BenhNhan bn
                    JOIN
                        LichHen lh ON bn.ID_BenhNhan = lh.ID_BenhNhan
                    JOIN
                        BacSi bs ON bs.ID_BacSi = lh.ID_BacSi
                    JOIN
                        TaiKhoan tk ON bs.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN
                        TaiKhoan tk2 ON bn.ID_TaiKhoan = tk2.ID_TaiKhoan
                    GROUP BY
                        bn.ID_BenhNhan;";

                var parameters = new {};

                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                return Results.Ok(result);
            });

            //Lấy thông tin lịch làm việc
            builder.MapPost("admin/lichlamviec", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn thông tin lịch làm việc
                const string sql = @"
                    SELECT 
                        llv.ID_LichLamViec,
                        llv.NgayLamViec,
                        llv.ThoiGianBatDau,
                        llv.ThoiGianKetThuc,
                        tk.HoTen AS TenBacSi,
                        llv.TrangThai
                    FROM 
                        LichLamViec llv
                    JOIN
                        BacSi bs On bs.ID_BacSi = llv.ID_BacSi
                    JOIN
                        TaiKhoan tk On tk.ID_TaiKhoan = bs.ID_TaiKhoan";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Sửa trạng thái lịch làm việc
            builder.MapPost("admin/lichlamviec/suaduyet", async (int ID_LichLamViec, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE LichLamViec 
                    SET
                        TrangThai = N'Đã duyệt'
                    WHERE
                        ID_LichLamViec = @ID_LichLamViec";
                var parameters = new { ID_LichLamViec };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });

            //Sửa trạng thái lịch làm việc
            builder.MapPost("admin/lichlamviec/suahuy", async (int ID_LichLamViec, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    UPDATE LichLamViec 
                    SET
                        TrangThai = N'Huỷ'
                    WHERE
                        ID_LichLamViec = @ID_LichLamViec";
                var parameters = new { ID_LichLamViec };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok();
            });
        }
    }
}
