# EarthquakeTracker

A C# .NET 10 WinForms app that shows current and historic earthquakes around the world.

## Features

- Great project for getting started with maps and online earthquake tracking services.
- Completely self-contained. No NuGet packages, third-party libraries or API keys needed.
- Shows earthquake activity worldwide.
- Uses the online USGS Earthquake API..
- Uses OpenStreetMap for maps.

The following figure shows EarthquakeTracker in action.

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/9ca52529-fa52-476e-9f2d-775c2dc2fc08" />

*Figure 1* - The app showing earthquake locations and magnitudes in North America. 

The app starts out with a blank map. The starting and end dates default to covering the previous week. By clicking the **Get Earthquakes** button the app queries the USGS (United States Geological Survey) online service at ```earthquake.usgs.gov``` to get a list of events in the given period and matching the **Lowest Magnitude** filter. Earthquakes are denoted on the map by circles, colored according to the quake magnitude. 

## The UI Layout

The user interface uses two layers of docked panels for its layout, as shown in the following figure.

<img width="778" height="520" alt="image" src="https://github.com/user-attachments/assets/2e05c2ea-ceab-4716-a319-4655f04b50b6" />

*Figure 2* - The docked panels for the user interface.

* Layer 1 - Shown in red, this layer divides the screen vertically, with the middle area filling available space.
* Layer 2 - Shown in blue, this layer is just for the map, which fills the middle area of layer 1.

To achieve the required layout, the panels in layer 1 must be added in the proper back-to-front order, like this:

  1. PanelTop.
  2. PanelMagnitudeColors.
  3. PanelBottom.
  4. PanelMiddle.

As a general rule, components with Dock=Fill must always be added last to their parent container.

## How it Works
The main form of the app is just used to setup the layout and handle user interactions with the UI controls. The interesting stuff is mostly in EarthquakeMap, derived from MapControl. MapControl handles the download and display of the map, built using OpenStreetMap tiles. EarthquakeMap handles the download and display of earthquake activity. The following figure shows the class hierarchy of EarthquakeMap.

<img width="198" height="502" alt="image" src="https://github.com/user-attachments/assets/c579941d-73e4-48a1-9d33-a1af1bc20a9f" />

*Figure 3* - The class hierarchy of EarthquakeMap.

The map is made up by tiles that fit tightly together to create a complete and seamless image. The geographic area covered by each tile depends on the Zoom level, which you can change using the mouse wheel. You can also pan the map by clicking and dragging the mouse.

## Retrieving Earthquake Data

When you click the **Get Earthquakes** button, the main form calls the async method EarthquakeMap.GetEarthquakesAsync, which calls the USGS online service, gets a list of earthquakes and displays them on the map. The following figure shows the sequence diagram.

<img width="905" height="507" alt="image" src="https://github.com/user-attachments/assets/ad53a52a-247b-4c6a-85c9-edbe2714ffbc" />

*Figure 4* - The sequence diagram showing how earthquakes are found and displayed.

After SetEarthquakes is called, the map is invalidated to force the the system to refresh the map with the newly added earthquakes. The following listing shows the code related to the actions covered in the sequence diagram above.

```csharp
public partial class EarthquakeMap : MapControl
{
  HttpClient http = new();
  List<EarthquakeMarker> earthquakeMarkers = new List<EarthquakeMarker>();

  //...

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
}
```
*Listing 1* - The code related to the retrieval of USGS earthquake data.

Earthquake data returned from USGS is in JSON format with the following structure:

```
{
  "features":
  [
    {
      "type":"Feature",
      "properties":
      {
        "mag": 4.6,
        "place": "31 km S of Tezpur, India",
        "time": 1791240822181,
        "title": "M 4.6 - 31 km S of Tezpur, India"
      },
      "geometry":
      {
        "type": "Point",
        "coordinates": [92.8359, 26.3539,10]
      },
    },
    // other earthquakes...
  ]
}
```
*Listing 2* - A fragment of the JSON data returned by USGS.

The time is given in Unix format: milliseconds since UTC Midnight, Jan 1, 1970. The coordinates are latitude and longitude. The JSON is deserialized into classes that you can find in the EarthquakeModels.cs file. It much easier to handle C# objects than raw JSON text.

## Earthquake Markers
You can't just take a piece of earthquake data and slap it on the map. You need to wrap it in something that understands map coordinates and screen painting. This is where markers come into play. An EarthquakeMarker encapsulates the earthquake data and paints an earthquake circle on the screen. The following listing shows the highlights.

```csharp
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
    //...
  }
}

```
*Listing 3* - The salient EarthquakeMarker code.

EarthquakeMarkers paint themselves as a circle colored according to the earthquake magnitude. When you select a marker by clicking it with the mouse, a tooltip is also painted with the earthquake details. The marker's Paint method is called by EarthquakeMap, in the OnPaint method like this:

```csharp
public partial class EarthquakeMap : MapControl
{
  List<EarthquakeMarker> earthquakeMarkers = new List<EarthquakeMarker>();

  //...

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
}
```
*Listing 4* - How EarthquakeMap paints the markers.

## The Main Form

The main form is very simple. Essentially it hosts the docked panels on which the various controls are placed. It also handles the events from the controls, delegating them to EarthquakeMap. The following listing shows the code.

```csharp
public partial class FormMain : Form
{
  public FormMain()
  {
    InitializeComponent();

    earthquakeMap.CenterView(latitude: 30, longitude: -100, zoom: 4); // continental US

    dateTimePickerStartingDate.Value = DateTime.Now.AddDays(-7);
    dateTimePickerEndingDate.Value = DateTime.Now;
    labelZoomLevel.Text = $"Zoom level: {earthquakeMap.Zoom}";
    labelRefreshedAt.Text = $"Refreshed at {DateTime.Now.ToString("T")}";
  }

  async void ButtonGetEarthquakes_Click(object sender, EventArgs e)
  {
    buttonGetEarthquakes.Enabled = false;
    labelStatusMessage.Text = "Retrieving earthquake data...";
    DateTime startingDate = dateTimePickerStartingDate.Value;
    DateTime endingDate = dateTimePickerEndingDate.Value;

    try
    {
      var earthquakesFound = await earthquakeMap.GetEarthquakesAsync(startingDate, endingDate, (double)numericUpDownLowestMagnitude.Value);

      labelRefreshedAt.Text = $"Refreshed at {DateTime.Now.ToString("T")}";
      labelStatusMessage.Text = $"Worldwide earthquakes found: {earthquakesFound}";
      panelMagnitudeColors.Visible = earthquakesFound > 0;
    }
    catch (Exception ex)
    {
      labelStatusMessage.ForeColor = Color.Red;
      labelStatusMessage.Text = ex.Message;
      panelMagnitudeColors.Visible = false;
    }
    finally
    {
      buttonGetEarthquakes.Enabled = true;
    }
  }

  void EarthquakeMap_StatusChanged(string status)
  {
    labelStatusMessage.Text = status;
  }

  void EarthquakeMap_ZoomLevelChanged(int zoomLevel)
  {
    labelZoomLevel.Text = $"Zoom level: {zoomLevel}";
  }
}
```
*Listing 5* - The code for the main form of the app.

## The Map

At the root of EarthquakeTracker is a world map. Displaying a map is more complicated than it might seem at first, because the Earth is round and we usually display maps on a flat surface. A rather naive but simple way to handle the problem is to use a Mercator projection of the Earth. The idea is to conceptually wrap a cylinder around the Earth so that it touches the globe at the equator. Mercator maps are the projection of the Earth's surface onto that cylinder, which is then unwrapped into a flat rectangle. The following figure shows the idea as described by ChatGPT.

<img width="622" height="491" alt="image" src="https://github.com/user-attachments/assets/2deca2de-3e6b-4464-ac6c-1a6f24e10529" />

*Figure 5* - Generating a Mercator projection of the Earth.

Mercator projections provide reasonably good approximations of countries at lower latitudes. The farther you get from the equator, the greater the distortion. Near the poles, the distortion is huge. Look at you big Greenland and Antarctica appear on the map. 
<p></p>
  
What makes Mercator projections popular is their ease of use. You can simply divide the square map into smaller square tiles, avoiding a lot of tricky math to account for the round Earth.
<p></p>
The Web Mercator is a special format for Mercator maps. It's based on a standard called EPSG:3857, which uses 256x256 pixel tiles. The coordinate system uses longitude for the X axis and latitude for the Y axis. The origin corresponds to the point at longitude=0 and latitude=0, which is the center of the map. This point turns out to be somewhere in the Gulf of Guinea, off the coast of West Africa. The X axis increases to the right (East). The Y axis increases upward (North). In Web Mercator, the Earth is assumed to be a perfect sphere, so the height and width of the map are the same: about 40,000 km, which is the circumference of the Earth at the equator. Since the origin is in the middle of the map, no location can be farther than about + or - 20,000 km.

## Downloading Tiles
As mentioned earlier, the Web Mercator format is based on 256x256 pixel tiles. MapControl handles the downloading of all tiles needed to display the visible portion of map at the given zoom level. The following listing shows the main code.

```csharp
  async void RequestTile(int zoom, int x, int y, string cacheKey)
  {
    FireStatusChanged("Downloading tiles...");

    // download the bytes for a tile from the server
    string uri = $"https://tile.openstreetmap.org/{zoom}/{x}/{y}.png";
    byte[] data = await http.GetByteArrayAsync(uri);

    FireStatusChanged("");

    // convert bytes to a bitmap representation of the tile
    using var ms = new MemoryStream(data);
    var bitmap = new Bitmap(ms);

    // save tile in the cache
    tileCache[cacheKey] = bitmap;
    tileOrderInCache.Enqueue(cacheKey);
}
```
*Listing 6* - The salient code for downloading tiles.

Downloaded tiles are stored in a tile cache, which is just a Dictionary defined like this: ```Dictionary<string, Bitmap>```. The value is the 256x256 pixel bitmap for the tile, the key is a combination of tile-specific properties put together with the following code:

```string GetCacheKey(int zoom, int x, int y) => $"{zoom}|{x}|{y}";```

*Listing 7* - Generating a key for the Tile cache.

## Painting Tiles

Painting a map on the screen entails painting tiles. These are painted in the method MapControl.OnPaint, whose highlights are shown in the following listing.

```csharp
  protected override void OnPaint(PaintEventArgs e)
  {
    var g = e.Graphics;
    g.Clear(BackColor);
    g.InterpolationMode = InterpolationMode.HighQualityBilinear;

    double size = WorldSize(Zoom);  // size of each side in pixels of the square Mercator world at this zoom level
    int tilesPerSide = 1 << Zoom;   // number of tiles per side of the square Mercator map at this zoom level
    double left = CenterX - Width / 2.0;
    double top = CenterY - Height / 2.0;

    int x0 = (int)Math.Floor(left / TileSizeInPixels);                                       // leftmost tile index (can be negative)
    int x1 = (int)Math.Floor((left + Width) / TileSizeInPixels);                             // rightmost tile index (can be greater than tilesPerSide - 1)
    int y0 = Math.Max(0, (int)Math.Floor(top / TileSizeInPixels));                           // topmost tile index (clamped to 0)
    int y1 = Math.Min(tilesPerSide - 1, (int)Math.Floor((top + Height) / TileSizeInPixels)); // bottommost tile index (clamped to tilesPerSide - 1)

    using var imageAttributes = new ImageAttributes();

    for (int ty = y0; ty <= y1; ty++)     // iterate over the visible tile rows
    {
      for (int tx = x0; tx <= x1; tx++)   // iterate over the visible tile columns
      {
        int wrappedX = ((tx % tilesPerSide) + tilesPerSide) % tilesPerSide;  // wrap X coordinate around the world map
        string key = GetCacheKey(Zoom, wrappedX, ty);       // unique key for this tile in the cache

        var destination = new Rectangle(                    // client coordinates where the tile should be drawn
            (int)Math.Round(tx * TileSizeInPixels - left),  // top-left X of the tile in client coordinates (0,0 is top-left of control)
            (int)Math.Round(ty * TileSizeInPixels - top),   // top-left Y of the tile in client coordinates
            TileSizeInPixels, TileSizeInPixels);            // width and height of the tile in client coordinates

        // if the tile is already downloaded and cached, draw it; otherwise download it

        if (tileCache.TryGetValue(key, out var bitmap))
          g.DrawImage(bitmap, destination, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);

        else if (!failedTiles.Contains(key))
          RequestTile(Zoom, wrappedX, ty, key);
      }
    }
  }

```
*Listing 8* - Painting map tiles on the screen.

Tile coordinates are in meters, indicating the X and Y distance from the center of the Web Mercator map. To paint tiles, these coordinates must be converted to MapControl client coordinates. The line g.DrawImage(...) does the actual painting.

## Some Famous Historical Earthquakes

Before closing I thought it might be interesting to show some major earthquakes from the past.
<br>

It was the year 1960 when Chile was struck by a magnitude 9.5 earthquake, the biggest in recorded history anywhere in the world.

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/9b8a1480-cd2c-4da3-88a4-70ec7be56a21" />

*Figure 6* - The 1960 Valdivia earthquake in Chile.

Despite the tremendous power of the quake and the ensuing tsunami, the number of fatalities wasn't massive, possibly due to the area not having a large population at the time.
<p></p>
A few years before the Chile earthquake there was a magnitude 7.8 quake in Alaska. Although the 1958 quake wasn't truly massive, it produced an avalanche into nearby Lituya Bay, generating the largest mega tsunami in recorded history. Due to the peculiar shape and depth of the bay, the wave reached an astonishing 524 meters in height.

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/78d76aab-549a-4c22-a505-c6604f020e1e" />

*Figure 7* - The 1958 Alaska earthquake the generated the Lituya Bay mega tsunami.

The deadliest earthquake in the 20th century, and possibly of all time, was the 1976 Tangshan quake in China. 

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/5369bffa-6a29-46d2-a36e-d0c4ba2c913f" />

*Figure 8* - The 1976 Tangshan earthquake in China.

Although the Tangshan magnitude was *only* 7.4,  the official death toll is 242,000. Unofficial foreign estimates have been as high as 800,000.

Another earthquake that caused massive fatalities was the 2004 Sumatra-Andaman earthquake, which occurred the day after Christmas.

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/2c0ca419-7aa2-41e6-990b-df3102437473" />

*Figure 9* - The 2004 Sumatra-Andaman earthquake in Indonesia.

 The quake caused many fatalities, but its main legacy was the tsunami that followed. It was a truly world-level event, killing over 227,000 people across 14 countries. It was the deadliest (but not biggest) mega tsunami in history. An estimated 100,000+ fatalities occurred in Banda Aceh alone, a town on the Northwest tip of Sumatra (Indonesia). A 30-meter tsunami swept through the densely populated area, reaching over 5 km inland.

Although North America doesn't have a lot of catastrophic earthquakes (outside Alaska), 1906 was a pretty rough year, as shown in the next figure.

<img width="981" height="630" alt="image" src="https://github.com/user-attachments/assets/246561ac-fc93-4f93-87a8-f38beb6aa4f5" />

*Figure 10* - The year 1906 in North America.

As you can see, the San Francisco earthquake wasn't the only significant event in 1906.

## Closing Notes
Much of the logic in a mapping app is devoted to coordinate management and conversions. If you don't understand the coordinate systems, you'll have a hard time modifying the code to add your own new features.
<p></p><p></p>
The MapControl in EarthquakeTracker can easily be used in your own projects. It doesn't have any external dependencies so you can literally copy and paste it into your code.
<p></p><p></p>
Many thanks to Google Gemini and ChatGPT.
