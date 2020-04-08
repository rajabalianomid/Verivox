namespace Verivox.Common.Domain
{
    public class Pagination<TEntity>
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int Count { get; private set; }

        public TEntity Data { get; private set; }

        public bool Next => PageIndex < (PageCount);

        public bool Previous => PageIndex > 0;

        public int PageCount => PageSize > 0 ? (Count / PageSize) : 0;

        public Pagination(int pageIndex, int pageSize, int count, TEntity data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
