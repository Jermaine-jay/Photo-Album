using ImageApp.BLL.Extensions;
using ImageApp.DAL.DataBase;
using ImageApp.DAL.Repository;
using ImageApp.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImageAppDbContext>(opts =>
{
    var defaultConn = builder.Configuration.GetSection("ConnectionString")["DefaultConn"];
    opts.UseSqlServer(defaultConn);
});

builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailSenderOptions"));

builder.Services.AddControllersWithViews();
builder.Services.RegisterServices();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
// Add services to the container.

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<ImageAppDbContext>>();
builder.Services.AddAutoMapper(Assembly.Load("ImageApp.BLL"));


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    ServiceExtensions.Configure(services);
}

await Seeds.EnsurePopulatedAsync(app);

app.Run();
