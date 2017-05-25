using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Queries.Parameters
{
    public class ResponseParameters : IKeyedEntity, IPageable
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string ApiKey { get; set; }
    }
}