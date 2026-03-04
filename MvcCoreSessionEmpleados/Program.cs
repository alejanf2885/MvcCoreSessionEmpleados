using Microsoft.EntityFrameworkCore;
using MvcCoreSessionEmpleados.Data;
using MvcCoreSessionEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleados>();

string connectionString =
    builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
