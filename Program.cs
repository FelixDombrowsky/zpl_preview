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

  // สร้าง DB ถ้าไม่มี
  db.Database.EnsureCreated();

  // เพิ่ม table / column ที่อาจขาดใน DB เก่า (ถ้ามีอยู่แล้วจะ catch และข้ามไป)
  db.Database.ExecuteSqlRaw(@"
    CREATE TABLE IF NOT EXISTS ""LocationLogs"" (
      ""Id""        INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
      ""Latitude""  REAL    NOT NULL DEFAULT 0,
      ""Longitude"" REAL    NOT NULL DEFAULT 0,
      ""Accuracy""  REAL    NOT NULL DEFAULT 0,
      ""Action""    TEXT    NOT NULL DEFAULT '',
      ""Method""    TEXT    NOT NULL DEFAULT '',
      ""Timestamp"" TEXT    NOT NULL DEFAULT ''
    )");
  try { db.Database.ExecuteSqlRaw(@"ALTER TABLE ""LocationLogs"" ADD COLUMN ""Method"" TEXT NOT NULL DEFAULT ''"); } catch { }

  db.Database.ExecuteSqlRaw(@"
    CREATE TABLE IF NOT EXISTS ""Locations"" (
      ""Id""          INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
      ""Code""        TEXT NOT NULL DEFAULT '',
      ""Description"" TEXT NOT NULL DEFAULT '',
      ""Type""        TEXT NOT NULL DEFAULT '',
      ""Status""      TEXT NOT NULL DEFAULT 'Active',
      ""Latitude""    REAL NULL,
      ""Longitude""   REAL NULL
    )");

  foreach (var sql in new[]
  {
    @"ALTER TABLE ""LayoutTemplates"" ADD COLUMN ""ValOffset""       INTEGER NOT NULL DEFAULT 120",
    @"ALTER TABLE ""LayoutTemplates"" ADD COLUMN ""ZplTemplate""     TEXT    NOT NULL DEFAULT ''",
    @"ALTER TABLE ""LayoutTemplates"" ADD COLUMN ""LabelFieldsJson"" TEXT    NOT NULL DEFAULT '[]'",
  })
  {
    try { db.Database.ExecuteSqlRaw(sql); } catch { /* column already exists */ }
  }
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
