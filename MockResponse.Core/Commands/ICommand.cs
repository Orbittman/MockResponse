namespace MockResponse.Core.Commands
{
    public interface ICommand<TRequestType>
    {
        void Execute(TRequestType request);
    }
}
