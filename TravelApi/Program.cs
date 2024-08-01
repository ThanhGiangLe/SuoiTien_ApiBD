using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm appsettings.json vào cấu hình để lấy connectionString và các tùy chỉnh theo từng môi trường
string evn = builder.Environment.EnvironmentName; // Production(hiện tại)
builder.Configuration.AddJsonFile($"appsettings.{evn}.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();

// chỉ cho phép request http://localhost:5067 truy cập
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5067"));
});

var app = builder.Build();

// Sử dụng Cors sau khi AddCors
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
