using System;
using System.Collections.Generic;
using System.IO;

namespace ScreenSaver
{
    public static class Utilities
    { 
        internal static Stream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();

            #pragma warning disable CA2000 // Dispose objects before losing scope
            StreamWriter writer = new StreamWriter(stream);
            #pragma warning restore CA2000 // Dispose objects before losing scope
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        internal static List<City> GetDefaultCities()
        {
            List<City> defaultCities = new List<City>
            {
                new City("Central European Standard Time", "Amsterdam"),
                new City("Central European Standard Time", "Berlin"),
                new City("Hawaiian Standard Time", "Hawaii"),
                new City("GMT Standard Time", "London"),
                new City("Pacific Standard Time", "Los Angeles"),
                new City("Eastern Standard Time", "New York"),
                new City("Tokyo Standard Time", "Tokyo"),
                new City("Eastern Standard Time", "Toronto")
            };

            return defaultCities;
        }

        internal static Dictionary<string, City> GetDefaultCitiesWithIndexes()
        {
            Dictionary<string, City> defaultCities = new Dictionary<string, City>
            {
                { "1", new City("Central European Standard Time", "Amsterdam") },
                { "2", new City("Central European Standard Time", "Berlin") },
                { "3", new City("Hawaiian Standard Time", "Hawaii") },
                { "4", new City("GMT Standard Time", "London") },
                { "5", new City("Pacific Standard Time", "Los Angeles") },
                { "6", new City("Eastern Standard Time", "New York") },
                { "7", new City("Tokyo Standard Time", "Tokyo") },
                { "8", new City("Eastern Standard Time", "Toronto") }
            };

            return defaultCities;
        }

        internal static List<string> GetAllTimeZones()
        {
            List<string> defaultTimeZones = new List<string>();

            foreach (TimeZoneInfo timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                defaultTimeZones.Add(timeZoneInfo.Id);
            }

            defaultTimeZones.Sort();

            return defaultTimeZones;
        }
    }
}
