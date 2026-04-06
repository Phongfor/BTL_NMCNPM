using FurnitureShop.BLL;

var builder = WebApplication.CreateBuilder(args);

// L?y connection string t? appsettings.json
var connectionString = builder.Configuration.GetConnectionString("FurnitureShopDB")!;

// ??ng ký các BLL service — inject connection string
builder.Services.AddScoped(s => new ProductBLL(connectionString));
builder.Services.AddScoped(s => new CategoryBLL(connectionString));
builder.Services.AddScoped(s => new UserBLL(connectionString));
builder.Services.AddScoped(s => new OrderBLL(connectionString));
builder.Services.AddScoped<CartBLL>();

// C?u hình Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Ph?i ??t TR??C UseAuthorization
app.UseAuthorization();

// Route m?c ??nh
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();