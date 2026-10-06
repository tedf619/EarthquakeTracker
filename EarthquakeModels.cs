using System.Text.Json.Serialization;

namespace EarthquakeTracker;

public class FeatureCollection
{
    [JsonPropertyName("features")]
    public List<EarthquakeFeature> Features { get; set; } = new();
}

public class EarthquakeFeature
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("properties")]
    public EarthquakeProperties Properties { get; set; } = new();

    [JsonPropertyName("geometry")]
    public PointGeometry Geometry { get; set; } = new();
}

public class EarthquakeProperties
{
    [JsonPropertyName("mag")]
    public double? Mag { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public long TimeUnixMs { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    public DateTimeOffset Time => DateTimeOffset.FromUnixTimeMilliseconds(TimeUnixMs);
}

public class PointGeometry
{
    [JsonPropertyName("coordinates")]
    public List<double?> Coordinates { get; set; } = new();

    public double Longitude => Coordinates.Count > 0 ? Coordinates[0] ?? 0.0 : 0.0;
    public double Latitude => Coordinates.Count > 1 ? Coordinates[1] ?? 0.0 : 0.0;
  public double? DepthKm => Coordinates.Count > 2 ? Coordinates[2] ?? 0.0 : 0.0;
}