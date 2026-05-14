public class LocationLog
{
  public int Id { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double Accuracy { get; set; }
  public string Action { get; set; } = "";
  public string Method { get; set; } = "";
  public DateTime Timestamp { get; set; }
}

public class LocationLogRequest
{
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double Accuracy { get; set; }
  public string Action { get; set; } = "";
  public string Method { get; set; } = "";
}
