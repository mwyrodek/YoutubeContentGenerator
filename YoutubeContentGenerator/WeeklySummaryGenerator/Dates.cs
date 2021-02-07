using System;
using System.Globalization;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public static class Dates
    {
        public static int GetNextWeekNumber()
        {
            CultureInfo myCI = new CultureInfo("pl-PL");
            DateTime date = DateTime.UtcNow;
            Calendar myCal = myCI.Calendar;
            var nextNextweek = date.AddDays(7);
            return myCal.GetWeekOfYear(nextNextweek, myCI.DateTimeFormat.CalendarWeekRule, myCI.DateTimeFormat.FirstDayOfWeek);
        }

        public static DateTime GetNextWeekSaturday()
        {
            var date = DateTime.UtcNow;
            return  date.Next(DayOfWeek.Saturday).AddDays(7);
        }
        
        //stolen from https://stackoverflow.com/a/7611480
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}
