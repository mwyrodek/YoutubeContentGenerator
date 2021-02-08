using System;
using NUnit.Framework;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class DatesTest
    {
        [TestCase(1,1,2021, ExpectedResult = 1)]
        [TestCase(7,1,2021, ExpectedResult = 2)]
        [TestCase(31,12,2017, ExpectedResult = 1)]
        [TestCase(23,12,2017, ExpectedResult = 52)]
        [TestCase(24,12,2026, ExpectedResult = 53)]
        public int GetNextWeekTest(int day, int month, int year)
        {
            var date = new DateTime(year,month,day);
            return date.GetNextWeekNumber();
        }
        
        
        [TestCase("2021-1-1", "2021-1-9")]
        [TestCase("2021-1-2", "2021-1-9")]
        [TestCase("2021-1-3", "2021-1-9")]
        [TestCase("2020-12-30", "2021-1-9")]
        public void GetNextWeekSaturdayTest(DateTime startDate, DateTime expectedDate)
        {
   
            var newDate = startDate.GetNextWeekSaturday();
            Assert.That(newDate, Is.EqualTo(expectedDate));
        }
        
        [TestCase("2021-1-1", DayOfWeek.Monday ,"2021-1-4")]
        [TestCase("2021-1-1", DayOfWeek.Friday ,"2021-1-8")]
        [TestCase("2021-1-1", DayOfWeek.Saturday ,"2021-1-2")]
        public void GetNextTest(DateTime startDate, DayOfWeek day, DateTime expectedDate)
        {

            var newDate = startDate.Next(day);
            Assert.That(newDate, Is.EqualTo(expectedDate));
        }
    }
}