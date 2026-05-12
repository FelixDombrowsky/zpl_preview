using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class ZplController : ControllerBase
{
  private readonly HttpClient _httpClient;

  public ZplController(IHttpClientFactory httpClientFactory)
  {
    _httpClient = httpClientFactory.CreateClient();
  }

  [HttpPost("preview")]
  public async Task<IActionResult> Preview([FromBody] ZplPreviewRequest request)
  {
    var url = $"https://api.labelary.com/v1/printers/{request.Dpi}/labels/{request.Width}x{request.Height}/0/";

    var content = new StringContent(request.Zpl, Encoding.UTF8, "application/x-www-form-urlencoded");

    var response = await _httpClient.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
      return BadRequest("Labelary API error");

    var imageBytes = await response.Content.ReadAsByteArrayAsync();
    return File(imageBytes, "image/png");
  }
}

public class ZplPreviewRequest
{
  public string Zpl { get; set; } = "";
  public string Dpi { get; set; } = "8dpmm";
  public string Width { get; set; } = "4";
  public string Height { get; set; } = "3";
}