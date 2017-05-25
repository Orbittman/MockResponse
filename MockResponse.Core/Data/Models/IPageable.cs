namespace MockResponse.Core.Data.Models
{
    public interface IPageable
    {
        int Page { get; set; }

        int PageSize { get; set; }
    }
}