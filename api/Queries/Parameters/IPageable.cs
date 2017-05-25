namespace MockResponse.Api.Queries
{
    public interface IPageable
    {
        int Page { get; set; }

        int PageSize { get; set; }
    }
}