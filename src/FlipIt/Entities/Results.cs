
namespace ScreenSaver
{
    #pragma warning disable IDE1006 // Naming Styles
    #pragma warning disable CA1707  // Identifiers should not contain underscores
    #pragma warning disable CA1819  // Properties should not return arrays
    #pragma warning disable CA1812  // Avoid uninstantiated internal classes
    internal static class Results
    {
        internal class LocationIQ
        {
            private LocationIQ() { }

            public string place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public string osm_id { get; set; }
            public string[] boundingbox { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
            public string class_type { get; set; }
            public string type { get; set; }
            public string importance { get; set; }
            public string icon { get; set; }
        }

        internal class GeoNames
        {
            private GeoNames() { }

            public string sunrise { get; set; }
            public string lng { get; set; }
            public string countryCode { get; set; }
            public string gmtOffset { get; set; }
            public string rawOffset { get; set; }
            public string sunset { get; set; }
            public string timezoneId { get; set; }
            public string dstOffset { get; set; }
            public string countryName { get; set; }
            public string time { get; set; }
            public string lat { get; set; }
        }
    }
    #pragma warning restore IDE1006 // Naming Styles
    #pragma warning restore CA1707  // Identifiers should not contain underscores
    #pragma warning restore CA1819  // Properties should not return arrays
    #pragma warning restore CA1812  // Avoid uninstantiated internal classes
}
