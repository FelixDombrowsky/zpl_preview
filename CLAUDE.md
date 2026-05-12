# ZPL Test Project

## Project Overview
.NET Web API project สำหรับ preview ZPL label โดยเรียกผ่าน Labelary API
เพื่อแก้ปัญหา CORS ที่เกิดจากการเรียก Labelary API โดยตรงจาก browser

## Tech Stack
- .NET Web API (C#)
- Labelary API: https://api.labelary.com
- Frontend: HTML + JavaScript (อยู่ใน wwwroot/)

## Project Structure
```
zpl_test/
├── Controllers/
│   └── ZplController.cs       ← API endpoint สำหรับ preview ZPL
├── Properties/
│   └── launchSettings.json
├── wwwroot/
│   └── index.html             ← Frontend UI สำหรับ preview label
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── zpl_test.csproj
```

## Tasks

### 1. ลบไฟล์ที่ไม่ต้องการ
ลบ 2 ไฟล์นี้ออก:
- `Controllers/WeatherForecastController.cs`
- `WeatherForecast.cs`

### 2. สร้าง Controllers/ZplController.cs
สร้าง API Controller สำหรับรับ ZPL code แล้ว forward ไปที่ Labelary API และ return PNG กลับมา

```csharp
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
```

### 3. แก้ Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();
```

### 4. สร้าง wwwroot/index.html
สร้างหน้า UI ที่มี:
- Textarea สำหรับวาง ZPL code
- Dropdown เลือก DPI (8dpmm / 12dpmm / 24dpmm)
- Input width และ height (หน่วยเป็น inch)
- ปุ่ม Preview ที่เรียก POST /api/zpl/preview
- แสดงผล label เป็นรูปภาพ
- ปุ่ม Download PNG

API call ตัวอย่าง:
```javascript
const res = await fetch('/api/zpl/preview', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ zpl, dpi, width, height })
});
const blob = await res.blob();
const imgUrl = URL.createObjectURL(blob);
```

## ZPL ตัวอย่าง (ใช้ทดสอบ)
```
^XA
^CI28
^FO80,40^BQN,2,6^FDLA,SAMPLE-QR-DATA^FS
^FO490,30^A0N,30,30^FDPLANT:  TH-BKK^FS
^FO490,70^A0N,30,30^FDVENDOR:  ABC Corp^FS
^FO490,110^A0N,30,30^FDP/N:  12345678^FS
^FO490,150^A0N,30,30^FDP/O.No: PO-9999^FS
^FO490,190^A0N,30,30^FDINV No: INV-001^FS
^FO490,230^A0N,30,30^FDQTY: 100        PCE^FS
^FO490,270^A0N,30,30^FDD/C: 260311 REV.02^FS
^XZ
```

## Notes
- Labelary API endpoint: `https://api.labelary.com/v1/printers/{dpi}/labels/{width}x{height}/0/`
- Content-Type ที่ส่งไป Labelary ต้องเป็น `application/x-www-form-urlencoded`
- Response จาก Labelary เป็น PNG image
- ต้อง enable StaticFiles middleware ใน Program.cs เพื่อให้ wwwroot/index.html ทำงานได้
