namespace Altairis.AskMe.Web.Mvc.Models;
public class PagedModel<TItem> {

    public IEnumerable<TItem> Data { get; set; }

    public PagingInfo Paging { get; set; } = new PagingInfo();

    public async Task GetData(IQueryable<TItem> dataSource, int pageNumber, int pageSize) {
        // Validate arguments
        if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

        // Get number of records
        this.Paging.TotalRecords = await dataSource.CountAsync();
        this.Paging.PageNumber = pageNumber;
        this.Paging.TotalPages = (int)Math.Ceiling(this.Paging.TotalRecords / (float)pageSize);
        this.Paging.PrevPageNumber = pageNumber - 1;
        this.Paging.NextPageNumber = this.Paging.PageNumber == this.Paging.TotalPages ? 0 : pageNumber + 1;

        // Get data
        this.Data = dataSource.Skip(this.Paging.PrevPageNumber * pageSize).Take(pageSize);
    }

}
