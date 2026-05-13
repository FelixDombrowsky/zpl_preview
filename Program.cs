using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
  opt.UseSqlite("Data Source=layouts.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
