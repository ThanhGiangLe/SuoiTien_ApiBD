//using Microsoft.EntityFrameworkCore;
using WApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Thêm vào Service để gọi API.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<ApiService>(); // Đăng ký ApiService như một dịch vụ

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Sử dụng Https call Api
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
