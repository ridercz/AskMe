using System;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageNumber => this.PageIndex + 1;
        public int TotalPages => (int)Math.Ceiling(this.TotalItems / (float)this.PageSize);
        public int PreviousPageNumber => this.PageNumber - 1;
        public int NextPageNumber => (this.PageNumber == this.TotalPages) ? 0 : this.PageNumber + 1;
    }
}