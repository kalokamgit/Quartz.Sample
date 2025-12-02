using Quartz.Impl.Calendar;

namespace Quartz.Sample.Jobs
{
    public static class CalendarExtensions
    {

        private static readonly DayOfWeek[] WeekOffs = [
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        ];

        private static IDictionary<string, DateOnly> Holidays = new Dictionary<string, DateOnly>
        {
            { "New Year's Day", new DateOnly(DateTime.Now.Year, 1, 1) },
            { "Pongal Holiday", new DateOnly(DateTime.Now.Year, 1, 14)  },
            { "Republic Day", new DateOnly(DateTime.Now.Year, 1, 26)  },
            { "Labor Day", new DateOnly(DateTime.Now.Year, 5, 1) },
            { "Independence Day", new DateOnly(DateTime.Now.Year, 8, 15) },
            { "Christmas Day", new DateOnly(DateTime.Now.Year, 12, 25) }
        };

        extension(IServiceCollectionQuartzConfigurator configurator)
        {
            public IServiceCollectionQuartzConfigurator AddHolidaysCalendar()
            {
                var holidayCalendar = new HolidayCalendar();

                foreach (var holiday in Holidays)
                {
                    holidayCalendar.AddExcludedDate(holiday.Value.ToDateTime(new TimeOnly(0, 0)));
                }

                var workingDaysCalendar = new WeeklyCalendar();

                foreach (var weekOff in WeekOffs)
                {
                    workingDaysCalendar.SetDayExcluded(weekOff, true);
                }

                holidayCalendar.CalendarBase = workingDaysCalendar;

                configurator.AddCalendar(nameof(HolidayCalendar), holidayCalendar, true, false);
                return configurator;

            }
        }
    }
}
