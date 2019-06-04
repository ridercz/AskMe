using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class CollectionDataResult<TObj>
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public TObj Data { get; set; }

        public CollectionDataResult()
        {
            // NOOP
        }

        public CollectionDataResult(TObj data, QueryFilterBase filter, int totalItems)
        {
            this.Data = data;
            this.PageSize = filter.PageSize;
            this.PageIndex = filter.PageIndex;
            this.TotalItems = totalItems;
        }
    }
}
