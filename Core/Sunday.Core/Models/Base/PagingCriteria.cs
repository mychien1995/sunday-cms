namespace Sunday.Core.Models.Base
{
    public class PagingCriteria : IPagingCriteria
    {
        public int? PageIndex { get; set; } = 0;
        public int? PageSize { get; set; } = 10;
    }
}
