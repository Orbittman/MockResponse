namespace MockResponse.Api.Queries
{
    public interface IQuery<in TRequest, out TResponse>
    {
        TResponse Execute(TRequest request);
    }
}