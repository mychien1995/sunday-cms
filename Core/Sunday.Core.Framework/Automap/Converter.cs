using AutoMapper;
using System;

namespace Sunday.Core.Framework.Automap
{
    public class TicksToDateTimeConverter : ITypeConverter<long, DateTime>
    {
        public DateTime Convert(long source, DateTime destination, ResolutionContext context) => source.ToDatetime();
    }

    public class DatetimeToTicksConverter : ITypeConverter<DateTime, long>
    {
        public long Convert(DateTime source, long destination, ResolutionContext context) => source.ToEpoch();
    }
}
