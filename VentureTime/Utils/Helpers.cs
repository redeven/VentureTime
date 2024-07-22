using System;

namespace VentureTime.Utils
{
    internal class Helpers
    {
        public static bool DateTimeClose(DateTime lhs, DateTime rhs)
            => (lhs - rhs).Duration() < TimeSpan.FromMinutes(1);


        // cf https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings
        public static int GetStableHashCode(string str)
        {
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;

                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + hash2 * 1566083941;
            }
        }

        // cf https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations
        public static int CombineHashCodes(params int[] hashCodes)
        {
            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            var i = 0;
            foreach (var hashCode in hashCodes)
            {
                if (i % 2 == 0)
                    hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ hashCode;
                else
                    hash2 = ((hash2 << 5) + hash2 + (hash2 >> 27)) ^ hashCode;

                ++i;
            }

            return hash1 + hash2 * 1566083941;
        }

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
