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
            //Hiển thị các chuyên khoa
            builder.MapPost("chuyenkhoa", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT 
                        ID_ChuyenKhoa,
                        TenChuyenKhoa,
                        MoTa,
                        AnhUrl
                    FROM 
                        ChuyenKhoa";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Hiển thị thông tin bác sĩ
            builder.MapPost("bacsi", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn thông tin bác sĩ cùng với tài khoản, bằng cấp
                const string sql = @"
                    SELECT
                        b.ID_BacSi,
                        tk.HoTen,
                        ck.TenChuyenKhoa,
                        tk.Avatar,
                        bc.TenBangCap,
                        tk.DiaChi,
                        bc.HeSoLuong,
                        b.MoTa
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN 
                        BangCap bc ON b.ID_BangCap = bc.ID_BangCap
                    JOIN
                        ChuyenKhoa ck ON b.ID_ChuyenKhoa = ck.ID_ChuyenKhoa";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Lấy thông tin bác sĩ theo ID_BacSi
            builder.MapPost("bacsi/{id}", async (int idBacSi, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn thông tin bác sĩ theo ID_BacSi
                const string sql = @"
                    SELECT
                        b.ID_BacSi,
                        tk.HoTen,
                        ck.TenChuyenKhoa,
                        tk.Avatar,
                        bc.TenBangCap,
                        tk.DiaChi,
                        bc.HeSoLuong,
                        b.MoTa
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN 
                        BangCap bc ON b.ID_BangCap = bc.ID_BangCap
                    JOIN
                        ChuyenKhoa ck ON b.ID_ChuyenKhoa = ck.ID_ChuyenKhoa
                    WHERE
                        b.ID_BacSi = @ID_BacSi";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { ID_BacSi = idBacSi });

                if (result == null)
                {
                    return Results.NotFound("Không tìm thấy thông tin bác sĩ với ID này.");
                }

                return Results.Ok(result);
            });

            //Chuyên khoa theo id_chuyenkhoa
            builder.MapPost("chuyenkhoa/id", async (int idChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn thông tin bác sĩ theo ID_BacSi
                const string sql = @"
                    SELECT 
                        ID_ChuyenKhoa,
                        TenChuyenKhoa,
                        MoTa,
                        AnhUrl
                    FROM 
                        ChuyenKhoa
                    WHERE
                        ID_ChuyenKhoa = @ID_ChuyenKhoa";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { ID_ChuyenKhoa = idChuyenKhoa });

                if (result == null)
                {
                    return Results.NotFound("Không tìm thấy thông tin chuyên khoa với ID này.");
                }

                return Results.Ok(result);
            });

            //Laays thông tin bác sĩ theo id_chuyenkhoa
            builder.MapPost("bacsi/chuyenkhoa/{id}", async (int idChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn thông tin bác sĩ theo ID_BacSi
                const string sql = @"
                    SELECT
                        b.ID_BacSi,
                        tk.HoTen,
                        ck.TenChuyenKhoa,
                        tk.Avatar,
                        bc.TenBangCap,
                        tk.DiaChi,
                        bc.HeSoLuong,
                        b.MoTa
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN 
                        BangCap bc ON b.ID_BangCap = bc.ID_BangCap
                    JOIN
                        ChuyenKhoa ck ON b.ID_ChuyenKhoa = ck.ID_ChuyenKhoa
                    WHERE
                        ck.ID_ChuyenKhoa = @ID_ChuyenKhoa";

                var result = await connection.QueryAsync<dynamic>(sql, new { ID_ChuyenKhoa = idChuyenKhoa });

                if (result == null)
                {
                    return Results.NotFound("Không tìm thấy thông tin bác sĩ với ID này.");
                }

                return Results.Ok(result);
            });

            builder.MapPost("benhnhan/hosobenhan", async (string email, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT
                        hsba.ID_BenhNhan,
                        hsba.ID_HoSoBenhAn,
	                    hsba.NgayKham,
	                    hsba.ChuanDoan,
	                    hsba.HuongDieuTri,
	                    hsba.GhiChu AS TenBacSi,
	                    hsba.TrieuChung,
	                    hsba.KetQuaXetNghiem,
	                    bn.DiUng,
	                    bn.TienSuBenh,
	                    tk.HoTen AS TenBenhNhan,
	                    tk.NgaySinh,
	                    tk.GioiTinh,
	                    tk.SDT,
	                    tk.DiaChi
                    FROM
                        HoSoBenhAn hsba
                    JOIN
	                    BenhNhan bn ON bn.ID_BenhNhan = hsba.ID_BenhNhan
                    JOIN
	                    TaiKhoan tk ON bn.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE
                        tk.Email = @Email;";
                var parameters = new { email };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy thông tin kết quả xét nghiệm theo ID_HoSoBenhAn
            builder.MapPost("benhnhan/hosobenhan/ketquaxetnghiem", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    Select
	                    cs.TenChiSo,
	                    kq.GiaTri,
	                    cs.MucBinhThuong,
	                    cs.DonViDo,
	                    kq.GhiChu
                    From
	                    ChiSoXetNghiem cs
                    JOIN
	                    KetQuaXetNghiem kq ON cs.ID_ChiSoXetNghiem = kq.ID_ChiSoXetNghiem
                    WHERE
                        kq.ID_HoSoBenhAn = @ID_HoSoBenhAn;";
                var parameters = new { ID_HoSoBenhAn };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy đơn thuốc theo ID_HoSoBenhAn
            builder.MapPost("benhnhan/hosobenhan/donthuoc", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    Select 
	                    t.ID_Thuoc,
	                    t.TenThuoc,
	                    ct.SoLuong,
	                    ct.DonVi,
	                    ct.CachDung
                    From
	                    DonThuoc dt
                    JOIN
	                    ChiTietDonThuoc ct ON dt.ID_DonThuoc = ct.ID_DonThuoc
                    JOIN
	                    Thuoc t ON ct.ID_Thuoc = t.ID_Thuoc
                    WHERE
                        dt.ID_HoSoBenhAn = @ID_HoSoBenhAn;";
                var parameters = new { ID_HoSoBenhAn };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy thông tin bệnh nhân theo ID_HoSoBenhAn
            builder.MapPost("benhnhan/hosobenhan/thongtin", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT
                        hsba.ID_BenhNhan,
                        hsba.ID_HoSoBenhAn,
	                    hsba.NgayKham,
	                    hsba.ChuanDoan,
	                    hsba.HuongDieuTri,
	                    hsba.GhiChu AS TenBacSi,
	                    hsba.TrieuChung,
	                    hsba.KetQuaXetNghiem,
	                    bn.DiUng,
	                    bn.TienSuBenh,
	                    tk.HoTen AS TenBenhNhan,
	                    tk.NgaySinh,
	                    tk.GioiTinh,
	                    tk.SDT,
	                    tk.DiaChi
                    FROM
                        HoSoBenhAn hsba
                    JOIN
	                    BenhNhan bn ON bn.ID_BenhNhan = hsba.ID_BenhNhan
                    JOIN
	                    TaiKhoan tk ON bn.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE
                        hsba.ID_HoSoBenhAn = @ID_HoSoBenhAn;";
                var parameters = new { ID_HoSoBenhAn };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            // Check Đăng ký với tài khoản user
            builder.MapPost("register/partient", async (RegisterPartientDto registerPartientDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Kiểm tra tài khoản đã tồn tại chưa
                const string sqlCheckAccount = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Email";
                var accountExists = await connection.ExecuteScalarAsync<int>(sqlCheckAccount, new { registerPartientDto.Email });

                if (accountExists > 0)
                {
                    return Results.BadRequest(new { Message = "Gmail này đã đăng ký khám trước đó." });
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
                const string sqlGetID = "SELECT ID_TaiKhoan FROM TaiKhoan WHERE Email = @Email";
                var idTaiKhoan = await connection.ExecuteScalarAsync<int>(sqlGetID, new { registerPartientDto.Email });

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


                // Lấy ID_BenhNhan vừa thêm
                const string sqlGetPatientID = "SELECT ID_BenhNhan FROM BenhNhan WHERE ID_TaiKhoan = @ID_TaiKhoan";
                var idBenhNhan = await connection.ExecuteScalarAsync<int>(sqlGetPatientID, new { ID_TaiKhoan = idTaiKhoan });

                return Results.Created("login", new { Message = "Đăng ký thành công.", ID_TaiKhoan = idBenhNhan });
            });

            // Thêm mới lịch hẹn
            builder.MapPost("lichhen", async (LichHenDto LichHenDto, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Thêm lịch hẹn
                const string sql = @"
                    INSERT INTO LichHen(ID_BacSi, ID_BenhNhan, NgayHen, GioHen, TrieuChung, GhiChu, TrangThai) 
                    VALUES(@ID_BacSi, @ID_BenhNhan, @NgayHen, @GioHen, @TrieuChung, @GhiChu, @TrangThai)";
                await connection.ExecuteAsync(sql, LichHenDto);

                var NgayHen = LichHenDto.NgayHen;
                var GioHen = LichHenDto.GioHen;
                var ID_BacSi = LichHenDto.ID_BacSi;
                var ID_BenhNhan = LichHenDto.ID_BenhNhan;

                // Lấy thông tin bác sĩ
                const string sqlGetDoctor = @"
                    SELECT 
                        tk.HoTen
                    FROM 
                        BacSi b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE
                        b.ID_BacSi = @ID_BacSi";
                var tenBacSi = await connection.QueryFirstOrDefaultAsync<string>(sqlGetDoctor, new { ID_BacSi = ID_BacSi });

                // Lấy thông tin bệnh nhân
                const string sqlGetPartient = @"
                    SELECT 
                        tk.Email
                    FROM 
                        BenhNhan b
                    JOIN 
                        TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE
                        b.ID_BenhNhan = @ID_BenhNhan";

                var email = await connection.QueryFirstOrDefaultAsync<string>(sqlGetPartient, new { ID_BenhNhan = ID_BenhNhan });

                EmailServices.Send(NgayHen, email, GioHen, tenBacSi);

                return Results.Created("login", new { Message = "Tạo lịch hẹn thành công." });
            });

            //Kiểm tra email đã đăng ký chưa nếu đăng ký rồi trả về thông tin tài khoản JOIN với bảng bệnh nhân
            builder.MapPost("check/email", async (string email, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    SELECT 
                        tk.ID_TaiKhoan,
                        tk.TenDangNhap,
                        tk.Email,
                        tk.HoTen,
                        tk.NgaySinh,
                        tk.GioiTinh,
                        tk.SDT,
                        tk.DiaChi,
                        tk.Avatar,
                        bn.TienSuBenh,
                        bn.DiUng
                    FROM 
                        TaiKhoan tk
                    JOIN 
                        BenhNhan bn ON tk.ID_TaiKhoan = bn.ID_TaiKhoan
                    WHERE
                        tk.Email = @Email";
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Email = email });
                if (result == null)
                {
                    return Results.NotFound("Email này chưa đăng ký.");
                }
                return Results.Ok(result);
            });









            //// Lấy thông tin bệnh nhân theo ID_TaiKhoan
            //builder.MapGet("partient/{id}", async (int idTaiKhoan, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    const string sql = @"
            //        SELECT t.TenDangNhap, t.Email, t.HoTen, t.NgaySinh, t.GioiTinh, t.SDT, t.DiaChi, t.Avatar, b.TienSuBenh, b.DiUng
            //        FROM BenhNhan b
            //        JOIN TaiKhoan t ON b.ID_TaiKhoan = t.ID_TaiKhoan
            //        WHERE b.ID_TaiKhoan = @ID_TaiKhoan";

            //    var result = await connection.QueryAsync<dynamic>(sql, new { ID_TaiKhoan = idTaiKhoan });

            //    if (result == null || !result.Any())
            //    {
            //        return Results.NotFound("Không tìm thấy bệnh nhân có id này.");
            //    }

            //    return Results.Ok(result);
            //});

            //// Cập nhật thông tin tài khoản bệnh nhân theo ID_TaiKhoan
            //builder.MapPut("partient/{id}", async (int idTaiKhoan, UpdatePartientDto updatePartientDto, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    const string sql = @"
            //        UPDATE TaiKhoan
            //        SET
            //            Email = @Email,
            //            HoTen = @HoTen,
            //            NgaySinh = @NgaySinh,
            //            GioiTinh = @GioiTinh,
            //            SDT = @SDT,
            //            DiaChi = @DiaChi,
            //            Avatar = @Avatar
            //        WHERE ID_TaiKhoan = @ID_TaiKhoan";

            //    await connection.ExecuteAsync(sql, new
            //    {
            //        ID_TaiKhoan = idTaiKhoan,
            //        updatePartientDto.Email,
            //        updatePartientDto.HoTen,
            //        updatePartientDto.NgaySinh,
            //        updatePartientDto.GioiTinh,
            //        updatePartientDto.SDT,
            //        updatePartientDto.DiaChi,
            //        updatePartientDto.Avatar
            //    });

            //    return Results.Ok("Cập nhật thành công");
            //});


            ////Hiển thị thông tin bác sĩ theo id chuyên khoa
            //builder.MapGet("bacsi/chuyen-khoa/{idChuyenKhoa}", async (int idChuyenKhoa, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    // Truy vấn bác sĩ theo chuyên khoa
            //    const string sql = @"
            //        SELECT
            //            tk.HoTen,
            //            tk.GioiTinh,
            //            tk.Avatar,
            //            b.MoTa,
            //            bc.TenBangCap
            //        FROM 
            //            BacSi b
            //        JOIN 
            //            TaiKhoan tk ON b.ID_TaiKhoan = tk.ID_TaiKhoan
            //        JOIN 
            //            BangCap bc ON b.ID_BangCap = bc.ID_BangCap
            //        WHERE 
            //            b.ID_ChuyenKhoa = @ID_ChuyenKhoa";

            //    var result = await connection.QueryAsync<dynamic>(sql, new { ID_ChuyenKhoa = idChuyenKhoa });

            //    if (result == null || !result.Any())
            //    {
            //        return Results.NotFound("Không tìm thấy bác sĩ cho chuyên khoa này.");
            //    }

            //    return Results.Ok(result);
            //});

            //// Lấy thông tin lịch làm việc của bác sĩ theo ID và ngày
            //builder.MapGet("bacsi/{id}/lichlamviec", async (int id, DateTime ngayLamViec, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    const string sql = @"
            //        SELECT
            //            lv.NgayLamViec,
            //            lv.ThoiGianBatDau,
            //            lv.ThoiGianKetThuc,
            //            lv.TrangThai
            //        FROM 
            //            LichLamViec lv
            //        JOIN 
            //            BacSi b ON lv.ID_BacSi = b.ID_BacSi
            //        WHERE 
            //            b.ID_BacSi = @ID_BacSi AND 
            //            lv.NgayLamViec = @NgayLamViec";

            //    var result = await connection.QueryAsync<dynamic>(sql, new { ID_BacSi = id, NgayLamViec = ngayLamViec });

            //    if (result == null || !result.Any())
            //    {
            //        return Results.NotFound("Không tìm thấy lịch làm việc của bác sĩ vào ngày này.");
            //    }

            //    return Results.Ok(result);
            //});

            ////Xem thông tin lịch hẹn theo ID_BenhNhan
            //builder.MapGet("lichhen/{id}", async (int idBenhNhan, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    const string sql = @"
            //        SELECT 
            //            l.NgayHen, l.GioHen, t.HoTen as TenBacSi, l.TrieuChung, l.GhiChu, l.TrangThai FROM LichHen l 
            //            Join BacSi b On l.ID_BacSi = b.ID_BacSi
            //            Join TaiKhoan t On t.ID_TaiKhoan = b.ID_TaiKhoan
            //            Where ID_BenhNhan = @ID_BenhNhan";

            //    var result = await connection.QueryAsync<dynamic>(sql, new { ID_BenhNhan = idBenhNhan });

            //    if (result == null || !result.Any())
            //    {
            //        return Results.NotFound("Không tìm thấy lịch hẹn của bệnh nhân này.");
            //    }

            //    return Results.Ok(result);
            //});

            ////Cập nhật trạng thái lịch hẹn
            //builder.MapPut("lichhen/{id}", async (int id, UpdateLichHenDto updateLichHenDto, SqlConnectionFactory sqlConnectionFactory) =>
            //{
            //    using var connection = sqlConnectionFactory.Create();

            //    const string sql = @"
            //        UPDATE LichHen
            //        SET
            //            TrangThai = @TrangThai
            //        WHERE ID_LichHen = @ID_LichHen";

            //    await connection.ExecuteAsync(sql, new
            //    {
            //        ID_LichHen = id,
            //        updateLichHenDto.TrangThai
            //    });

            //    return Results.Ok("Cập nhật trạng thái lịch hẹn thành công.");
            //});
        }
    }
}
