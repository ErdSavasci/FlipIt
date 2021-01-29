
namespace ScreenSaver
{
    internal static class APIs
    {
        public const string defaultLatLonAPI = "https://us1.locationiq.com/v1/search.php?key=<API_KEY>&format=json&q=<city>";
        public const string defaultTimezoneAPI = "http://api.geonames.org/timezoneJSON?lat=<lat>&lng=<lon>&username=<USERNAME>";

        public static string LatLonAPIUrl { get; set; }

        public static string TimezoneAPIUrl { get; set; }
    }
}
