namespace Sunday.Core.Models.Base
{
    public interface ISearchResult<T>
    {
        public int Total { get; }
        public T[] Result { get; set; }
    }
}
