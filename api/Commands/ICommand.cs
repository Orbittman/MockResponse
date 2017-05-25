namespace MockResponse.Api.Commands
{
    public interface ICommand<in TRequest>
    {
        void Execute(TRequest request);
    }
}