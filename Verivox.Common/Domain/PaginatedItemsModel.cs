using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verivox.Common.Domain
{
    public class Pagination<TEntity>
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int Count { get; private set; }

        public TEntity Data { get; private set; }

        public bool Next { get { return PageIndex < (PageCount); } }

        public bool Previous { get { return PageIndex > 0; } }

        public int PageCount { get { return PageSize > 0 ? (Count / PageSize) : 0; } }

        public Pagination(int pageIndex, int pageSize, int count, TEntity data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }
    }
}
