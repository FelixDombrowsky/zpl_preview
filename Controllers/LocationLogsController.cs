using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace zpl_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationLogsController(AppDbContext db) : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetAll() =>
    Ok(await db.LocationLogs.OrderByDescending(x => x.Timestamp).ToListAsync());

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] LocationLogRequest req)
  {
    var log = new LocationLog
    {
      Latitude = req.Latitude,
      Longitude = req.Longitude,
      Accuracy = req.Accuracy,
      Action = req.Action,
      Method = req.Method,
      Timestamp = DateTime.UtcNow
    };
    db.LocationLogs.Add(log);
    await db.SaveChangesAsync();
    return Ok(log);
  }
}
