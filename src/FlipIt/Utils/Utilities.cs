using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

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

        internal static string GetConfigFilePath()
        {
            string userApplicationDataFolderPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                userApplicationDataFolderPath = Directory.GetParent(userApplicationDataFolderPath).ToString();
            }

            return userApplicationDataFolderPath + Path.DirectorySeparatorChar + "flipit_config.xml";
        }

        internal static float GetTimeBoxScale()
        {
            string configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
            {
                return 2 + ((3 - 2) * (float)0.75);
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);

                        try
                        {
                            int returnValue = int.Parse(xmlDoc.GetElementsByTagName("TimeBoxScale")[0].InnerText, CultureInfo.InvariantCulture);
                            return returnValue + ((3 - returnValue) * (float)0.75);
                        }
                        catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                        {
                            return 2 + ((3 - 2) * (float)0.75);
                        }
                    }
                }
            }
        }

        internal static bool Will24HoursBeShowed()
        {
            string configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
            {
                return false;
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);

                        return xmlDoc.GetElementsByTagName("Show24Hours")[0].InnerText.ToUpperInvariant().Equals("TRUE");
                    }
                }
            }
        }

        internal static bool WillSecondsBeShowed()
        {
            string configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
            {
                return false;
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);

                        return xmlDoc.GetElementsByTagName("ShowSeconds")[0].InnerText.ToUpperInvariant().Equals("TRUE");
                    }
                }
            }
        }

        internal static int GetCitiesSortOrder()
        {
            string configFilePath = GetConfigFilePath();

            // 1 -> Name
            // 2 -> Time

            if (!File.Exists(configFilePath))
            {
                return 1;
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);

                        return xmlDoc.GetElementsByTagName("CitiesSortOrder")[0].InnerText.ToUpperInvariant().Equals("NAME") ? 1 : 2;
                    }
                }
            }
        }

        internal static bool ShowWorldCitiesAtMainScreen()
        {
            string configFilePath = GetConfigFilePath();

            if (!File.Exists(configFilePath))
            {
                return false;
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);

                        return xmlDoc.GetElementsByTagName("MainScreen")[0].InnerText.ToUpperInvariant().Equals("WORLDCITIES");
                    }
                }
            }
        }

        internal static List<City> GetCities()
        {
            string configFilePath = GetConfigFilePath();
            var result = new List<City>();

            if (!File.Exists(configFilePath))
            {
                result = Utilities.GetDefaultCities();
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);
                        XmlNodeList cityNodeList = xmlDoc.GetElementsByTagName("cities")[0].ChildNodes;

                        foreach (XmlNode cityNode in cityNodeList)
                        {
                            result.Add(new City(cityNode.FirstChild.NextSibling.InnerText, cityNode.FirstChild.InnerText));
                        }
                    }
                }
            }

            return result;
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
                new City("Central European Standard Time", "Paris"),
                new City("Tokyo Standard Time", "Tokyo"),
                new City("Eastern Standard Time", "Toronto"),
                new City("America/New_York", "Virginia")
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
                { "7", new City("Central European Standard Time", "Paris") },
                { "8", new City("Tokyo Standard Time", "Tokyo") },
                { "9", new City("Eastern Standard Time", "Toronto") },
                { "10", new City("America/New_York", "Virginia") }
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

        internal static string ConvertCityToTimeZoneName(string cityName)
        {
            string timezoneDisplayName = null;
            
            try
            {
                string cityNameEncoded = cityName.Replace(" ", "+");

                using (WebClient webClient = new WebClient())
                {
                    string latLongResult = webClient.DownloadString(APIs.LatLonAPIUrl.Replace("<city>", cityNameEncoded));
                    latLongResult = latLongResult.Replace("\"class\":", "\"class_type\":");
                    Results.LocationIQ[] deserializedLatLongResult = JsonConvert.DeserializeObject<Results.LocationIQ[]>(latLongResult);

                    string lat = deserializedLatLongResult[0].lat;
                    string lon = deserializedLatLongResult[0].lon;

                    string timezoneResult = webClient.DownloadString(APIs.TimezoneAPIUrl.Replace("<lat>", lat).Replace("<lon>", lon));
                    Results.GeoNames deserializedTimezoneResult = JsonConvert.DeserializeObject<Results.GeoNames>(timezoneResult);
                    timezoneDisplayName = deserializedTimezoneResult.timezoneId;
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is WebException || ex is NotSupportedException)
            {
                Console.WriteLine(ex.Message);
            }

            return timezoneDisplayName;
        }
    }
}
