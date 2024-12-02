using ApiDoctorCare.Models;
using ApiDoctorCare.Services;
using ApiDoctorCare.Endpoints;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = "Server=NGUYENDANG;Database=BookingCare;Trusted_Connection=True;TrustServerCertificate=True;";
    options
        .UseSqlServer(connectionString,
            assembly =>
                assembly.MigrationsAssembly
                    (typeof(MyDbContext).Assembly.FullName))
        .UseLazyLoadingProxies();
});

builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new ApplicationException("The connection string is null");

    return new SqlConnectionFactory(connectionString);
});

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapAuthEndpoints();
app.MapPartientEndPoints();
app.MapDoctorEndpoints();
app.MapSupportEndPoints();
app.MapAdminEndpoints();
//app.MapSoSanhEndPoints();

app.Run();
