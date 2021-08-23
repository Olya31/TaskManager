using BL.Models;
using System;

namespace BL.Extantions
{
    public static class CronExtantion
    {
        public static CronModel ConvertToCronModel(this string cronString)
        {
            if (string.IsNullOrWhiteSpace(cronString))
            {
                return null;
            }

            var newCronString = cronString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (newCronString.Length != 5) return new CronModel();

            var cronModel = new CronModel()
            {
                Minutes = ConvertToMinutes(newCronString[0]),
                Hours = ConvertToMinutes(newCronString[1]),
                DayOfMonth = ConvertToMinutes(newCronString[2]),
                Month = ConvertToMinutes(newCronString[3]),
                DayOfWeek = ConvertToMinutes(newCronString[4]),
            };

            return cronModel;
        }


        private static TimeSpan ConvertToMinutes(string str)
        {
            if (int.TryParse(str, out var minutes))
            {
                return new TimeSpan(0, minutes, 0);
            }

            return TimeSpan.Zero;
        }
    }
}
