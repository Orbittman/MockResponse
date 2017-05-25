namespace MockResponse.Api.Queries
{
    public interface IQuery<TRequest, TResponse>
    {
        TResponse Execute(TRequest request);
    }
}