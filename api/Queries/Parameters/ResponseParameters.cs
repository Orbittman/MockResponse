using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Queries.Parameters
{
    public class ResponseParameters : IPageable
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}