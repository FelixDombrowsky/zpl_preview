using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace zpl_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LayoutsController(AppDbContext db) : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetAll() =>
    Ok(await db.LayoutTemplates.OrderByDescending(x => x.UpdatedAt).ToListAsync());

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] LayoutTemplateRequest req)
  {
    var t = new LayoutTemplate
    {
      TemplateName = req.TemplateName, Description = req.Description,
      QrX = req.QrX, QrY = req.QrY, QrSize = req.QrSize,
      DataX = req.DataX, DataY = req.DataY, FontSize = req.FontSize, ValOffset = req.ValOffset,
      ZplTemplate = req.ZplTemplate,
      LabelFieldsJson = req.LabelFieldsJson ?? System.Text.Json.JsonSerializer.Serialize(req.LabelFields),
      CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };
    db.LayoutTemplates.Add(t);
    await db.SaveChangesAsync();
    return Ok(t);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] LayoutTemplateRequest req)
  {
    var t = await db.LayoutTemplates.FindAsync(id);
    if (t is null) return NotFound();
    t.TemplateName = req.TemplateName; t.Description = req.Description;
    t.QrX = req.QrX; t.QrY = req.QrY; t.QrSize = req.QrSize;
    t.DataX = req.DataX; t.DataY = req.DataY; t.FontSize = req.FontSize; t.ValOffset = req.ValOffset;
    t.ZplTemplate = req.ZplTemplate;
    t.LabelFieldsJson = System.Text.Json.JsonSerializer.Serialize(req.LabelFields);
    t.UpdatedAt = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Ok(t);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var t = await db.LayoutTemplates.FindAsync(id);
    if (t is null) return NotFound();
    db.LayoutTemplates.Remove(t);
    await db.SaveChangesAsync();
    return Ok();
  }
}
