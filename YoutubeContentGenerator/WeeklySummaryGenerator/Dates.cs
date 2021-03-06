﻿using System;
using System.Globalization;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public static class Dates
    {
        public static int GetNextWeekNumber(this DateTime date)
        {
            var myCI = new CultureInfo("pl-PL");
            Calendar myCal = myCI.Calendar;
            var nextWeek = date.AddDays(7);
            return myCal.GetWeekOfYear(nextWeek, myCI.DateTimeFormat.CalendarWeekRule, myCI.DateTimeFormat.FirstDayOfWeek);
        }

        public static DateTime GetNextWeekSaturday(this DateTime date) => date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(7),
            DayOfWeek.Sunday =>  date.AddDays(6),
            _ => date.Next(DayOfWeek.Saturday).AddDays(7),
        };
        
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
