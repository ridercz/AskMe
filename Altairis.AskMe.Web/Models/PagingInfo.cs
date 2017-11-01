using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.AskMe.Web.Models {
    public class PagingInfo {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PrevPageNumber { get; set; }

        public int NextPageNumber { get; set; }

        public int TotalRecords { get; set; }

    }
}
