using System;

namespace BL.Models
{
    public class CronModel
    {
        public TimeSpan Minutes { get; set; }

        public TimeSpan Hours { get; set; }

        public TimeSpan DayOfMonth { get; set; }

        public TimeSpan Month { get; set; }

        public TimeSpan DayOfWeek { get; set; }
    }
}
