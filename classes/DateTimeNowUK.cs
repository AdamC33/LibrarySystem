using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.classes
{
    public static class DateTimeUK
    {
        public static DateTimeOffset DateTimeOffsetNow
        {
            get
            { 
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                TimeZoneInfo ukTime = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                if (ukTime.IsDaylightSavingTime(currentTime))
                {
                    return currentTime.AddHours(1);
                }
                else
                {
                    return currentTime;
                }
            }
        }
    }
}
