using System;

namespace VentureTime.Utils
{
    internal class Helpers
    {
        public static DateTime DateFromTimeStamp(uint timeStamp)
        {
            const long timeFromEpoch = 62135596800;

            return timeStamp == 0u
                ? DateTime.MinValue
                : new DateTime((timeStamp + timeFromEpoch) * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
        }

        public static TimeSpan TimeLeftFromNow(DateTime time)
        {
            if (time > DateTime.UtcNow)
            {
                return time - DateTime.UtcNow;
            }
            else {
                return TimeSpan.Zero;
            }
        }

        public static string FormatTimeSpan(TimeSpan time) {
            return string.Format(time.TotalHours >= 1 ? "{0:D2}:{1:D2}:{2:D2}" : "{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        }
    }
}
