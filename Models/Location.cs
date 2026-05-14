public class Location
{
  public int Id { get; set; }
  public string Code { get; set; } = "";
  public string Description { get; set; } = "";
  public string Type { get; set; } = "";
  public string Status { get; set; } = "Active";
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
}

public class LocationRequest
{
  public string Code { get; set; } = "";
  public string Description { get; set; } = "";
  public string Type { get; set; } = "";
  public string Status { get; set; } = "Active";
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
}
