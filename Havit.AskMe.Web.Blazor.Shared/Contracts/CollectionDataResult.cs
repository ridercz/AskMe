using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts
{
    public class CollectionDataResult<TObj> : PagingInfo
    {
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
