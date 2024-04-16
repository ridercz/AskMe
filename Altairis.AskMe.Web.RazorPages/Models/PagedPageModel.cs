namespace Altairis.AskMe.Web.RazorPages.Models;

public abstract class PagedPageModel<TItem> : PageModel {

    public IEnumerable<TItem> Data { get; set; } = null!;

    public PagingInfo Paging { get; set; } = new PagingInfo();

    protected async Task GetData(IQueryable<TItem> dataSource, int pageNumber, int pageSize) {
        // Validate arguments
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

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