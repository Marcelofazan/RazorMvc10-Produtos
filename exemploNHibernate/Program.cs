using exemploNHibernate.Extensions;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNHibernate(builder.Configuration.GetConnectionString("SQLiteConnection"));
builder.Services.AddControllersWithViews();

builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();

string caminhoBanco = Path.Combine(AppContext.BaseDirectory, "meubanco.db");
string connectionString = $"Data Source={caminhoBanco};";

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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