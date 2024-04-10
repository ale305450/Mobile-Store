using BusinessLogicLayer.Repository;
using DataAccessLayer.Data;
using DataAccessLayer.Entites;
using DataAccessLayer.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PresentionLayer;
using PresentionLayer.Areas.Identity.Code;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(option =>
                        option.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.SignIn.RequireConfirmedAccount = true;
}
   ).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
var googleConfiguration = builder.Configuration.GetSection("Authentication:Google");
builder.Services.AddAuthentication()
    .AddGoogle(option =>
    {
        option.ClientId = googleConfiguration["ClientId"];
        option.ClientSecret = googleConfiguration["ClientSecret"];
    }
    );

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.IOTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.Name = ".UserSession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
CreateAdmin();
await app.RunAsync();

async void CreateAdmin()
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider;

    var userManger = service.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManger = service.GetRequiredService<RoleManager<IdentityRole>>();
    var role = "Admin";

    //check if admin role in DB
    var check = await roleManger.RoleExistsAsync(role);
    if (!check)
    {
        //create admin role
        await roleManger.CreateAsync(new IdentityRole(role));
        //Create Admin account
        var user = new ApplicationUser
        {
            Email = "admin@test.com",
            UserName = "admin@test.com",
            EmailConfirmed = true,
            fullName = "Admin",
            address = "Yemen",
        };
        await userManger.CreateAsync(user, "awd1@3");
        await userManger.AddToRoleAsync(user, role);
    }
}