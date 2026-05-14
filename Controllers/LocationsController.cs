using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace zpl_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController(AppDbContext db) : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetAll() =>
    Ok(await db.Locations.OrderBy(x => x.Code).ToListAsync());

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] LocationRequest req)
  {
    var loc = new Location
    {
      Code = req.Code.Trim().ToUpper(),
      Description = req.Description,
      Type = req.Type,
      Status = req.Status,
      Latitude = req.Latitude,
      Longitude = req.Longitude
    };
    db.Locations.Add(loc);
    await db.SaveChangesAsync();
    return Ok(loc);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] LocationRequest req)
  {
    var loc = await db.Locations.FindAsync(id);
    if (loc is null) return NotFound();
    loc.Code = req.Code.Trim().ToUpper();
    loc.Description = req.Description;
    loc.Type = req.Type;
    loc.Status = req.Status;
    loc.Latitude = req.Latitude;
    loc.Longitude = req.Longitude;
    await db.SaveChangesAsync();
    return Ok(loc);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var loc = await db.Locations.FindAsync(id);
    if (loc is null) return NotFound();
    db.Locations.Remove(loc);
    await db.SaveChangesAsync();
    return Ok();
  }
}
