using System.Windows.Input;
using System.Diagnostics;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Mapsui.UI.Maui;
using Mapsui.Rendering.Skia;
using Mapsui.Projections;
using Mapsui.Tiling;

namespace InitialMapper
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        private Location user_location;
        private MapControl mapControl;

        public MainPage()
        {
            InitializeComponent();
            routeEntry.Completed += RouteEntry_Completed;
            this.Loaded += MainPage_Loaded;

            mapControl = new Mapsui.UI.Maui.MapControl();
            mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            mapControl.ReSnapRotationDegrees = 20;
            mapControl.Map.GetWidgetsOfMapAndLayers().First().Enabled = true;
            
            

            container.SetRow(mapControl, 0);
            container.SetRowSpan(mapControl, 2);
            container.Add(mapControl);

            // put routeEntry back on top
            
        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
            container.Remove(entryContainer);
            container.Remove(routeEntry);
            container.Add(entryContainer, 0, 0);
            Helper();
        }

        private async void Helper()
        {
            await GetCurrentLocation();
            if (user_location != null)
            {
                Mapsui.MPoint center = SphericalMercator.FromLonLat(new Mapsui.MPoint(user_location.Longitude, user_location.Latitude));
                mapControl.Map.Navigator.CenterOnAndZoomTo(center, 1);
            }
        }

        private void RouteEntry_Completed(object sender, EventArgs e)
        {
            
        }

        private async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();
                user_location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            }
            catch (Exception ex) { }
            finally { _isCheckingLocation = false; }
        }

        private void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }


}