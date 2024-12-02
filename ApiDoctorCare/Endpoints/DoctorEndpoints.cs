using ApiDoctorCare.Models;
using ApiDoctorCare.Services;
using Dapper;

namespace ApiDoctorCare.Endpoints
{
    public static class DoctorEndpoints
    {
        public static void MapDoctorEndpoints(this IEndpointRouteBuilder builder)
        {
            //Lấy danh sách bệnh nhân theo Username bacsi
            builder.MapPost("bacsi/dsbenhnhan", async (string TenDangNhap, SqlConnectionFactory sqlConnectionFactory) =>
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
                    WHERE
                        tk.TenDangNhap = @TenDangNhap
                    GROUP BY
                        bn.ID_BenhNhan;";

                var parameters = new { TenDangNhap };

                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                return Results.Ok(result);
            });

            //Lấy tất cả lịch hẹn
            builder.MapPost("bacsi/lichhen", async (string TenDangNhap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT
                        lh.ID_LichHen,
                        bn.ID_BenhNhan,
                        lh.NgayHen,
                        lh.GioHen,
                        lh.TrieuChung,
                        lh.GhiChu,
                        lh.TrangThai,
	                    tk2.HoTen as TenBenhNhan,
                        tk2.GioiTinh as GioiTinh,
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
	                    TaiKhoan tk2 ON bn.ID_TaiKhoan = tk2.ID_TaiKhoan
                    WHERE 
                        tk.TenDangNhap = @TenDangNhap";

                var parameters = new { TenDangNhap };

                var result = await connection.QueryAsync<dynamic>(sql, parameters);

                return Results.Ok(result);
            });

            //Lấy thông tin bệnh nhân theo id_lichhen
            builder.MapPost("bacsi/thongtinbenhnhan", async (int ID_LichHen, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = @"
                    SELECT
                        bn.ID_BenhNhan,
                        tk.HoTen,
                        tk.NgaySinh,
                        tk.GioiTinh,
                        tk.SDT,
                        tk.DiaChi,
                        bn.TienSuBenh,
                        bn.DiUng,
                        lh.TrieuChung
                    FROM
                        BenhNhan bn
                    JOIN
                        TaiKhoan tk ON bn.ID_TaiKhoan = tk.ID_TaiKhoan
                    JOIN
                        LichHen lh ON bn.ID_BenhNhan = lh.ID_BenhNhan
                    WHERE
                        lh.ID_LichHen = @ID_LichHen;";
                var parameters = new { ID_LichHen };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy hồ sơ bệnh án theo ID_BenhNhan
            builder.MapPost("bacsi/hosobenhan", async (int ID_BenhNhan, SqlConnectionFactory sqlConnectionFactory) =>
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
                        hsba.ID_BenhNhan = @ID_BenhNhan;";
                var parameters = new { ID_BenhNhan };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            builder.MapPost("bacsi/hosobenhan/id", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn
                const string sql = @"
                    SELECT
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

            builder.MapPost("bacsi/hosobenhan/ketquaxetnghiem", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
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
            builder.MapPost("bacsi/hosobenhan/donthuoc", async (int ID_HoSoBenhAn, SqlConnectionFactory sqlConnectionFactory) =>
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

            //Lấy chỉ số xét nghiệm
            builder.MapPost("bacsi/chisoxetnghiem", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        ChiSoXetNghiem";

                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });

            //Lấy thuốc
            builder.MapPost("bacsi/thuoc", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                // Truy vấn các chuyên khoa
                const string sql = @"
                    SELECT 
                        *
                    FROM 
                        Thuoc
                    WHERE
                        TrangThai = 1";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Hiển thị lịch làm của bác sĩ theo username
            builder.MapPost("bacsi/lichlam", async (string TenDangNhap, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    SELECT
                        ll.ID_LichLamViec,
                        ll.NgayLamViec,
                        ll.ThoiGianBatDau,
                        ll.ThoiGianKetThuc,
                        ll.TrangThai
                    FROM
                        LichLamViec ll
                    JOIN
                        BacSi bs ON ll.ID_BacSi = bs.ID_BacSi
                    JOIN
                        TaiKhoan tk ON bs.ID_TaiKhoan = tk.ID_TaiKhoan
                    WHERE
                        tk.TenDangNhap = @TenDangNhap;";
                var parameters = new { TenDangNhap };
                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Results.Ok(result);
            });

            //Them lich lam viec theo username
            builder.MapPost("bacsi/themlichlam", async (string TenDangNhap, string NgayLamViec, string ThoiGianBatDau, string ThoiGianKetThuc, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    INSERT INTO LichLamViec (ID_BacSi, NgayLamViec, ThoiGianBatDau, ThoiGianKetThuc, TrangThai)
                    VALUES ((SELECT ID_BacSi FROM BacSi WHERE ID_TaiKhoan = 
                        (SELECT ID_TaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)), @NgayLamViec, @ThoiGianBatDau, @ThoiGianKetThuc, N'Chưa duyệt');";
                var parameters = new { TenDangNhap, NgayLamViec, ThoiGianBatDau, ThoiGianKetThuc };
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Thêm hồ sơ bệnh án
            builder.MapPost("bacsi/themhosobenhan", async (int ID_BenhNhan, string NgayKham, string ChuanDoan, string HuongDieuTri, string GhiChu, string TrieuChung, SqlConnectionFactory sqlConnectionFactory) =>
            {
                if (!DateTime.TryParseExact(NgayKham, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedNgaySinh))
                {
                    return Results.BadRequest("Invalid date format for NgaySinh. Please use dd/MM/yyyy.");
                }

                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    INSERT INTO HoSoBenhAn (ID_BenhNhan, NgayKham, ChuanDoan, HuongDieuTri, GhiChu, TrieuChung, KetQuaXetNghiem)
                    VALUES (@ID_BenhNhan, @NgayKham, @ChuanDoan, @HuongDieuTri, @GhiChu, @TrieuChung, 0);";
                var parameters = new { ID_BenhNhan, NgayKham = parsedNgaySinh, ChuanDoan, HuongDieuTri, GhiChu, TrieuChung, };
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Thêm kết quả xét nghiệm
            builder.MapPost("bacsi/themketquaxetnghiem", async (int ID_HoSoBenhAn, int ID_ChiSoXetNghiem, string GiaTri, string GhiChu, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    INSERT INTO KetQuaXetNghiem (ID_HoSoBenhAn, ID_ChiSoXetNghiem, GiaTri, GhiChu)
                    VALUES (@ID_HoSoBenhAn, @ID_ChiSoXetNghiem, @GiaTri, @GhiChu);";
                var parameters = new { ID_HoSoBenhAn, ID_ChiSoXetNghiem, GiaTri, GhiChu };
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Thêm đơn thuốc
            builder.MapPost("bacsi/themdonthuoc", async (int ID_HoSoBenhAn, string NgayKe, SqlConnectionFactory sqlConnectionFactory) =>
            {
                if (!DateTime.TryParseExact(NgayKe, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedNgayKe))
                {
                    return Results.BadRequest("Invalid date format for NgayKe. Please use dd/MM/yyyy.");
                }
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    INSERT INTO DonThuoc (ID_HoSoBenhAn, NgayKe)
                    VALUES (@ID_HoSoBenhAn, @NgayKe);";
                var parameters = new { ID_HoSoBenhAn, NgayKe = parsedNgayKe };
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Lấy ID_DonThuoc vừa thêm
            builder.MapPost("bacsi/donthuocidthem", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    SELECT MAX(ID_DonThuoc) as
                        ID_DonThuoc
                    FROM
                        DonThuoc;";
                var result = await connection.QueryAsync<dynamic>(sql);
                return Results.Ok(result);
            });

            //Thêm chi tiết thuốc
            builder.MapPost("bacsi/themchitietdonthuoc", async (int ID_DonThuoc, int ID_Thuoc, int SoLuong, string DonVi, string CachDung, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    INSERT INTO ChiTietDonThuoc (ID_DonThuoc, ID_Thuoc, SoLuong, DonVi, CachDung)
                    VALUES (@ID_DonThuoc, @ID_Thuoc, @SoLuong, @DonVi, @CachDung);";
                var parameters = new { ID_DonThuoc, ID_Thuoc, SoLuong, DonVi, CachDung };
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Sửa thông tin trạng thái lịch hẹn
            builder.MapPost("bacsi/sualichhen", async (int ID_LichHen, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    UPDATE LichHen
                    SET TrangThai = N'Đã hoàn thành'
                    WHERE ID_LichHen = @ID_LichHen;";
                var parameters = new { ID_LichHen};
                var result = await connection.ExecuteAsync(sql, parameters);
                return Results.Ok(result);
            });

            //Hiển thị lịch làm của bác sĩ theo username
            builder.MapPost("bacsi/hosobenhanidthem", async ( SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
                const string sql = @"
                    SELECT MAX(ID_HoSoBenhAn) as
                        ID_HoSoBenhAn
                    FROM
                        HoSoBenhAn;";
                var result = await connection.QueryAsync<dynamic>(sql);

                return Results.Ok(result);
            });
        }
    }
}
