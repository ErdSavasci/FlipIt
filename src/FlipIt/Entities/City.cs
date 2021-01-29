using NodaTime;
using System;

namespace ScreenSaver
{
    internal class City
    {
        public City(string timeZoneID, string displayName)
        {
            DisplayName = displayName;
            TimeZoneID = timeZoneID;

            if (timeZoneID.Contains("/"))
            {
                AutoConfiguredCity = true;
            }
        }

        public City(string displayName)
        {
            DisplayName = displayName;

            AutoConfiguredCity = true;
            DateTimeZone timezoneInfo = DateTimeZoneProviders.Tzdb[Utilities.ConvertCityToTimeZoneName(DisplayName)];
            TimeZoneID = timezoneInfo.Id;
        }

        internal int CityID { get; set; }
        internal string DisplayName { get; }
        internal string TimeZoneID { get; private set; }
        internal DateTime CurrentTime { get; private set; }
        internal bool IsDaylightSavingTime { get; private set; }
        internal int DaysDifference { get; private set; }
        internal bool AutoConfiguredCity { get; private set; }

        internal void RefreshTime(DateTime now)
        {
            if (!AutoConfiguredCity)
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                CurrentTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Local, timeZoneInfo);
                IsDaylightSavingTime = timeZoneInfo.IsDaylightSavingTime(CurrentTime);
                DaysDifference = (CurrentTime.Date - now.Date).Days;
            }
            else
            {
                DateTimeZone timezoneInfo = DateTimeZoneProviders.Tzdb[TimeZoneID];
                CurrentTime = Instant.FromDateTimeUtc(now.ToUniversalTime()).InZone(timezoneInfo).ToDateTimeUnspecified();
                IsDaylightSavingTime = CurrentTime.IsDaylightSavingTime();
                DaysDifference = (CurrentTime.Date - now.Date).Days;
            }
        }
    }
}