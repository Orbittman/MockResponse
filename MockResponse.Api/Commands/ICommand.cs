namespace MockResponse.Api.Commands
{
    public interface ICommand<in TRequest>
    {
        void Execute(TRequest request);
    }

    public interface ICommand<TResponse, TRequest>
    {
        TResponse Execute(TRequest request);    
    }
}