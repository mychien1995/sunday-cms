namespace Sunday.Core.Models
{
    public interface IPagingCriteria
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
