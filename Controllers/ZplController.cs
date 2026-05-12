using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

namespace zpl_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZplController(IHttpClientFactory httpClientFactory, IConfiguration config) : ControllerBase
{
  private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

  [HttpPost("preview")]
  public async Task<IActionResult> Preview([FromBody] ZplPreviewRequest request)
  {
    var url = $"https://api.labelary.com/v1/printers/{request.Dpi}/labels/{request.Width}x{request.Height}/0/";
    var content = new StringContent(request.Zpl, Encoding.UTF8, new MediaTypeHeaderValue("application/x-www-form-urlencoded"));
    var response = await _httpClient.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
      return BadRequest("Labelary API error");

    return File(await response.Content.ReadAsByteArrayAsync(), "image/png");
  }

  [HttpGet("printers")]
  public async Task<IActionResult> GetPrinters()
  {
    var configs = config.GetSection("Printers").Get<List<PrinterConfig>>() ?? [];
    var tasks = configs.Select(async p => new { p.Ip, p.Name, Online = await CheckPrinterOnline(p.Ip) });
    return Ok(await Task.WhenAll(tasks));
  }

  [HttpPost("print")]
  public async Task<IActionResult> Print([FromBody] PrintRequest request)
  {
    try
    {
      using var client = new TcpClient();
      await client.ConnectAsync(request.PrinterIp, 9100);
      await using var stream = client.GetStream();
      await stream.WriteAsync(Encoding.UTF8.GetBytes(request.Zpl));
      return Ok(new { message = "ส่งงานพิมพ์สำเร็จ" });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = $"พิมพ์ไม่สำเร็จ: {ex.Message}" });
    }
  }

  private static async Task<bool> CheckPrinterOnline(string ip)
  {
    try
    {
      using var client = new TcpClient();
      using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
      await client.ConnectAsync(ip, 9100, cts.Token);
      return true;
    }
    catch { return false; }
  }
}

public class ZplPreviewRequest
{
  public string Zpl { get; set; } = "";
  public string Dpi { get; set; } = "8dpmm";
  public string Width { get; set; } = "4";
  public string Height { get; set; } = "3";
}

public class PrinterConfig
{
  public string Ip { get; set; } = "";
  public string Name { get; set; } = "";
}

public class PrintRequest
{
  public string PrinterIp { get; set; } = "";
  public string Zpl { get; set; } = "";
}
