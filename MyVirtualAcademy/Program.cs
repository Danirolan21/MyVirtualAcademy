using Microsoft.EntityFrameworkCore;
using MyVirtualAcademy.Data;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<RepositoryMyVirtualAcademy>();
builder.Services.AddDbContext<MyVirtualAcademyContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlMyVirtualAcademy")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
