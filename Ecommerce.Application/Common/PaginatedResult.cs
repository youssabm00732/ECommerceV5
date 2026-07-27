using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(IReadOnlyList<TEntity> data, int pageIndex, int pageSize, int count)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public IReadOnlyList<TEntity> Data { get; set; } = []; // de al Data nfsha aly hyb2a rag3a fel Josn 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; } // Total Count of all Data (Proudacts)
    }
}
