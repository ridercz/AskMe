using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Web.Mvc.Models {

    public static class PagedModel {

        // We are using factory method, because constructors cannot be async
        public static async Task<PagedModel<TItem>> CreateAsync<TItem>(IQueryable<TItem> dataSource, int pageNumber, int pageSize) {
            // Validate arguments
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            // Get number of records
            var m = new PagedModel<TItem>();
            m.Paging.TotalRecords = await dataSource.CountAsync();
            m.Paging.PageNumber = pageNumber;
            m.Paging.TotalPages = (int)Math.Ceiling(m.Paging.TotalRecords / (float)pageSize);
            m.Paging.PrevPageNumber = pageNumber - 1;
            m.Paging.NextPageNumber = m.Paging.PageNumber == m.Paging.TotalPages ? 0 : pageNumber + 1;

            // Get data
            m.Data = dataSource.Skip(m.Paging.PrevPageNumber * pageSize).Take(pageSize);

            return m;
        }
    }

    public class PagedModel<TItem> {

        public IEnumerable<TItem> Data { get; set; }

        public PagingInfo Paging { get; set; } = new PagingInfo();

    }
}
