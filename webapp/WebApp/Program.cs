using SCISalesTest.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductApiService, ProductApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApi:BaseAddress"]
        ?? "https://localhost:5000");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
