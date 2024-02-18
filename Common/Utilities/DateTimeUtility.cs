using Common.Enums;

namespace Common.Utilities
{
    public static class DateTimeUtility
    {
        public static DateTime Now() => DateTime.UtcNow;
        public static DateTime AddTime(int time, TimeUnit unit = TimeUnit.Minutes)
        {
            DateTime dateTime = Now();
            switch (unit)
            {
                case TimeUnit.Seconds:
                    dateTime = dateTime.AddSeconds(time);
                    break;
                case TimeUnit.Hours:
                    dateTime = dateTime.AddHours(time);
                    break;
                case TimeUnit.Days:
                    dateTime = dateTime.AddDays(time);
                    break;
                default:
                    dateTime = dateTime.AddMinutes(time);
                    break;
            }
            return dateTime;
        }
    }
}
