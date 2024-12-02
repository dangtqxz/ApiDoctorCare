using Dapper;
using ApiDoctorCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ApiDoctorCare.Endpoints
{
    public static class SoSanhEndPoint
    {
        public static void MapSoSanhEndPoints(this IEndpointRouteBuilder builder)
        {
            // Endpoint sử dụng Dapper để chèn dữ liệu
            builder.MapPost("DapperInsert", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                var stopwatch = Stopwatch.StartNew();
                using var connection = sqlConnectionFactory.Create();
                connection.Open();
                var sql = "INSERT INTO TableForDapper (HoTen, SDT, DiaChi, GioiTinh, QueQuan) VALUES (@HoTen, @SDT, @DiaChi, @GioiTinh, @QueQuan)";
                
                for (int i = 1; i <= 1000; i++)
                {
                    var testData = new
                    {
                        HoTen = $"HoTen {i}",
                        SDT = $"SDT {i}",
                        DiaChi = $"DiaChi {i}",
                        GioiTinh = $"GioiTinh {i}",
                        QueQuan = $"QueQuan {i}"
                    };
                    await connection.ExecuteAsync(sql, testData);
                }
                stopwatch.Stop();
                return Results.Ok($"Inserted 10000 records using Dapper in {stopwatch.ElapsedMilliseconds} ms.");
            });

            // Endpoint sử dụng Entity Framework để chèn dữ liệu
            builder.MapPost("EFInsert", async (MyDbContext dbContext) =>
            {
                var stopwatch = Stopwatch.StartNew();

                for (int i = 1; i <= 1000; i++)
                {
                    var testData = new TestEntity
                    {
                        HoTen = $"HoTen {i}",
                        SDT = $"SDT {i}",
                        DiaChi = $"DiaChi {i}",
                        GioiTinh = $"GioiTinh {i}",
                        QueQuan = $"QueQuan {i}"
                    };
                    await dbContext.TableForEntityFramework.AddAsync(testData);
                    await dbContext.SaveChangesAsync();
                }
                stopwatch.Stop();
                return Results.Ok($"Inserted 10000 records using Entity Framework in {stopwatch.ElapsedMilliseconds} ms.");
            });

            // // Endpoint sử dụng Entity Framework để chèn dữ liệu sử dụng Bulk Insert
            // builder.MapPost("EFInsertBulkInsert", async (MyDbContext dbContext) =>
            // {
            //     var stopwatch = Stopwatch.StartNew();

            //     for (int i = 1; i <= 10000; i++)
            //     {
            //         var testData = new TestEntity
            //         {
            //             HoTen = $"HoTen {i}",
            //             SDT = $"SDT {i}",
            //             DiaChi = $"DiaChi {i}",
            //             GioiTinh = $"GioiTinh {i}",
            //             QueQuan = $"QueQuan {i}"
            //         };
            //         await dbContext.TableForEntityFramework.AddAsync(testData);
            //     }
            //     await dbContext.SaveChangesAsync();
            //     stopwatch.Stop();
            //     return Results.Ok($"Inserted 10000 records using Entity Framework with Bulk Insert in {stopwatch.ElapsedMilliseconds} ms.");
            // });

            // Endpoint sử dụng Dapper để lấy dữ liệu
            builder.MapGet("DapperGet", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                var stopwatch = Stopwatch.StartNew();
                using var connection = sqlConnectionFactory.Create();
                var sql = "SELECT * FROM TableForDapper";
                var data = connection.Query<dynamic>(sql).ToList();
                stopwatch.Stop();
                return Results.Ok(new
                {
                    Message = $"Retrieved {data.Count} records using Dapper in {stopwatch.ElapsedMilliseconds} ms."
                });
            });

            // Endpoint sử dụng Entity Framework để lấy dữ liệu
            builder.MapGet("EFGet", async (MyDbContext dbContext) =>
            {
                var stopwatch = Stopwatch.StartNew();
                var data = await dbContext.TableForEntityFramework.ToListAsync();
                stopwatch.Stop();
                return Results.Ok(new
                {
                    Message = $"Retrieved {data.Count} records using Entity Framework in {stopwatch.ElapsedMilliseconds} ms."
                });
            });

            // // Endpoint sử dụng Entity Framework lấy dữ liệu sử dụng AsNoTracking
            // builder.MapGet("EFGetNoTracking", async (MyDbContext dbContext) =>
            // {
            //     var stopwatch = Stopwatch.StartNew();
            //     var data = await dbContext.TableForEntityFramework.AsNoTracking().ToListAsync();
            //     stopwatch.Stop();
            //     return Results.Ok(new
            //     {
            //         Message = $"Retrieved {data.Count} records using Entity Framework with AsNoTracking in {stopwatch.ElapsedMilliseconds} ms."
            //     });
            // });

            // Endpoint sử dụng Dapper để xóa tất cả dữ liệu
            builder.MapDelete("DapperDeleteAll", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                var stopwatch = Stopwatch.StartNew();
                using var connection = sqlConnectionFactory.Create();
                var sql = "DELETE FROM TableForDapper";
                var affectedRows = await connection.ExecuteAsync(sql);
                stopwatch.Stop();

                return Results.Ok(new
                {
                    Message = $"Deleted {affectedRows} records from TableForDapper using Dapper.",
                    TotalTimeMs = stopwatch.ElapsedMilliseconds
                });
            });

            // Endpoint sử dụng Entity Framework để xóa tất cả dữ liệu
            builder.MapDelete("EFDeleteAll", async (MyDbContext dbContext) =>
            {
                var stopwatch = Stopwatch.StartNew();
                var allEntities = await dbContext.TableForEntityFramework.ToListAsync();
                dbContext.TableForEntityFramework.RemoveRange(allEntities);
                var affectedRows = await dbContext.SaveChangesAsync();
                stopwatch.Stop();

                return Results.Ok(new
                {
                    Message = $"Deleted {affectedRows} records from TableForEntityFramework using Entity Framework.",
                    TotalTimeMs = stopwatch.ElapsedMilliseconds
                });
            });
        }
    }

    // Định nghĩa lớp DbContext
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities { get; set; }

        public DbSet<TestEntity> TableForEntityFramework { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>().ToTable("TableForEntityFramework");
        }
    }

    // Định nghĩa lớp thực thể
    public class TestEntity
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public string QueQuan { get; set; }
    }
}
