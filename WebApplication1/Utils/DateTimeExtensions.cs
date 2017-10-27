using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Utils
{
    public static class DateTimeExtensions
    {
        public static long EpochSeconds(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static long EpochMilliseconds(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        //public static DateTime UnixEpoch(this DateTime dateTime)
        //{
        //    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //}
    }

    public static class LongExtensions
    {
        public static DateTime DateTime(this long epoch)
        {
            if (epoch < 9999999999)
            {
                return DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToUniversalTime();
            }
            else
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(epoch).DateTime.ToUniversalTime();
            }
        }
    }
}
