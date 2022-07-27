using BethanyPieShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BethanysPieShopDbCtxConnection")
                       ?? throw new InvalidOperationException("Connection string 'BethanysPieShopDbCtxConnection' not found.");

//Registering repositories for Dependency Injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BethanysPieShopDbContext>(opts =>
{
    opts.UseSqlServer(connectionString);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<BethanysPieShopDbContext>();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseStaticFiles();

//Bringing support for session
app.UseSession();

//Bringing support for authentication and authorization
app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/app/{*catchall}", "/App/Index");

DbInitializer.SeedData(app);

app.Run();
