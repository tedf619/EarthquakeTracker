namespace EarthquakeTracker;

internal class EarthquakeMarker
{
  EarthquakeFeature earthquake;
  public RectangleF Bounds { get; private set; }

  public bool IsSelected { get; set; } = false;

  public EarthquakeMarker(EarthquakeFeature earthquakeFeature)
  {
    earthquake = earthquakeFeature;
  }

  public void Paint(Graphics g, double topLeftX, double topLeftY, double zoom)
  {
    (double worldX, double worldY) = LatLonToWorldPixels(earthquake.Geometry.Latitude, earthquake.Geometry.Longitude, (int)zoom);

    float screenX = (float)(worldX - topLeftX);
    float screenY = (float)(worldY - topLeftY);

    double mag = earthquake.Properties.Mag ?? 1.0;
    float radius = Math.Max(5f, (float)(mag * 4.0));
    float diameter = radius * 2f;

    Bounds = new RectangleF(screenX - radius, screenY - radius, diameter, diameter);

    Color fillColor = mag switch
    {
      >= 7.0 => Color.FromArgb(190, 178, 34, 34),   // Dark Red
      >= 5.0 => Color.FromArgb(190, 255, 69, 0),    // Red-Orange
      >= 3.0 => Color.FromArgb(190, 255, 165, 0),   // Orange
      _ => Color.FromArgb(190, 255, 215, 0)         // Gold
    };

    using var brush = new SolidBrush(fillColor);
    using var pen = new Pen(Color.FromArgb(230, 40, 40, 40), 1.2f);

    g.FillEllipse(brush, Bounds);
    g.DrawEllipse(pen, Bounds);

    if (!IsSelected) return;

    var properties = earthquake.Properties;
    string depth = earthquake.Geometry.DepthKm == 0 ? "Unknown" : $"{earthquake.Geometry.DepthKm:F2} km";
    string info = $"Magnitude: {properties.Mag}\n" +
                  $"Location: {properties.Place}\n" +
                  $"Date/Time: {properties.Time.ToLocalTime().ToString("yyyy-MM-dd HH:mm")}\n" +
                  $"Coordinates: ({earthquake.Geometry.Latitude:F4}, {earthquake.Geometry.Longitude:F4})\n" +
                  $"Depth: {depth}";

    using var font = new Font("Arial", 10);
    SizeF textSize = g.MeasureString(info, font);
    RectangleF textRect = new RectangleF(screenX + radius + 5, screenY - textSize.Height / 2, textSize.Width + 6, textSize.Height + 4);
    using var textBrush = new SolidBrush(Color.FromArgb(220, 255, 255, 255));
    using var borderPen = new Pen(Color.FromArgb(200, 0, 0, 0), 1f);

    g.FillRectangle(textBrush, textRect);
    g.DrawRectangle(borderPen, textRect.X, textRect.Y, textRect.Width, textRect.Height);
    g.DrawString(info, font, Brushes.Black, new PointF(textRect.X + 3, textRect.Y + 2));
  }

  static (double x, double y) LatLonToWorldPixels(double lat, double lon, int zoom)
  {
    const int TileSize = 256;
    double mapSize = TileSize * (1 << zoom);
    double x = (lon + 180.0) / 360.0 * mapSize;

    double sinLat = Math.Sin(lat * Math.PI / 180.0);
    sinLat = Math.Clamp(sinLat, -0.9999, 0.9999);
    double y = (0.5 - Math.Log((1.0 + sinLat) / (1.0 - sinLat)) / (4.0 * Math.PI)) * mapSize;

    return (x, y);
  }
}
