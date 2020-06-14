namespace Sunday.Core.Models.Base
{
    public class PagingCriteria : IPagingCriteria
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
