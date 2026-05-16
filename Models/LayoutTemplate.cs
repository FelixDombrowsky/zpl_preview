public class LayoutTemplate
{
  public int Id { get; set; }
  public string TemplateName { get; set; } = "";
  public string Description { get; set; } = "";
  public int QrX { get; set; } = 60;
  public int QrY { get; set; } = 40;
  public int QrSize { get; set; } = 6;
  public int DataX { get; set; } = 320;
  public int DataY { get; set; } = 30;
  public int FontSize { get; set; } = 28;
  public int ValOffset { get; set; } = 120;
  public string ZplTemplate { get; set; } = "";
  public string LabelFieldsJson { get; set; } = "[]";
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}

public class LayoutTemplateRequest
{
  public string TemplateName { get; set; } = "";
  public string Description { get; set; } = "";
  public int QrX { get; set; } = 60;
  public int QrY { get; set; } = 40;
  public int QrSize { get; set; } = 6;
  public int DataX { get; set; } = 320;
  public int DataY { get; set; } = 30;
  public int FontSize { get; set; } = 28;
  public int ValOffset { get; set; } = 120;
  public string ZplTemplate { get; set; } = "";
  public System.Text.Json.JsonElement? LabelFields { get; set; }
}
