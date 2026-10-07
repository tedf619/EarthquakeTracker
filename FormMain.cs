namespace EarthquakeTracker;

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
