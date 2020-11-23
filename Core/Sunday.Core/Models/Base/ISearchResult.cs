namespace Sunday.Core.Models.Base
{
    public interface ISearchResult<T>
    {
        public int Total { get; set; }
        public T[] Result { get; set; }
    }
}
