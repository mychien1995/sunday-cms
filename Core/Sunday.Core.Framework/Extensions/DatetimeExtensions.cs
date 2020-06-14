namespace System
{
    public static class DatetimeExtensions
    {
        public static long ToEpoch(this DateTime datetime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (datetime.ToUniversalTime() - epoch).TotalMilliseconds;
            return (long)unixDateTime;
        }

        public static DateTime ToDatetime(this long epochTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(epochTime);
        }
    }
}
