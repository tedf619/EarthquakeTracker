using System.Text;
using System.Text.Json;

namespace EarthquakeTracker;

public partial class EarthquakeMap : MapControl
{
  readonly HttpClient http = new();
  EarthquakeMarker? selectedMarker = null;

  List<EarthquakeMarker> earthquakeMarkers = new List<EarthquakeMarker>();

  public EarthquakeMap()
  {
    InitializeComponent();
  }

  public void SetEarthquakes(List<EarthquakeFeature> features)
  {
    earthquakeMarkers.Clear();

    // Sort features by magnitude in descending order so that smaller earthquakes are drawn on top
    foreach (var feature in features.OrderByDescending((f) => f.Properties.Mag))
    {
      var marker = new EarthquakeMarker(feature);
      earthquakeMarkers.Add(marker);
    }
    Invalidate();
  }

  public async Task<int> GetEarthquakesAsync(DateTime startingDate, DateTime endingDate, double minMagnitude)
  {
    string startDateIso = startingDate.ToString("yyyy-MM-dd");
    string endDateIso = endingDate.ToString("yyyy-MM-dd");
    double minMag = minMagnitude;

    string requestUrl = $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={startDateIso}&endtime={endDateIso}&minmagnitude={minMag}&orderby=time";

    try
    {
      using var response = await http.GetAsync(requestUrl);
      response.EnsureSuccessStatusCode();

      await using var stream = await response.Content.ReadAsStreamAsync();
      var data = await JsonSerializer.DeserializeAsync<FeatureCollection>(stream);

      if (data?.Features == null || data.Features.Count == 0)
      {
        SetEarthquakes(new List<EarthquakeFeature>());
        return 0;
      }

      SetEarthquakes(data.Features);
      return data.Features.Count;
    }
    catch (Exception ex)
    {
      throw new Exception($"Failed to fetch earthquake data: {ex.Message})");
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    Graphics g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

    int width = Width;
    int height = Height;

    double topLeftX = CenterX - width / 2.0;
    double topLeftY = CenterY - height / 2.0;

    foreach (var marker in earthquakeMarkers)
      marker.Paint(g, topLeftX, topLeftY, Zoom);
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    base.OnMouseDown(e);

    if (e.Button != MouseButtons.Left) return;

    // deselect any previously selected marker
    selectedMarker = null;
    foreach (var marker in earthquakeMarkers)
      marker.IsSelected = false;

    // select the clickedmarker, if any
    foreach (var marker in earthquakeMarkers)
    {
      marker.IsSelected = marker.Bounds.Contains(e.X, e.Y);

      if (!marker.IsSelected) continue;
      selectedMarker = marker;
      break;
    }

    if (selectedMarker != null)
    {
      // move selected marker to the end of the list, so it appears on top of other markers
      earthquakeMarkers.Remove(selectedMarker);
      earthquakeMarkers.Add(selectedMarker);
    }

    Invalidate();
  }

  void EarthquakeMap_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Escape) return;
    if (selectedMarker == null) return;

    selectedMarker.IsSelected = false;
    selectedMarker = null;

    e.Handled = true;
    e.SuppressKeyPress = true;

    Invalidate();  // to remove any tooltip shown
  }
}
