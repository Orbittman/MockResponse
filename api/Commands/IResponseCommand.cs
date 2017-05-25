using MockResponse.Api.Commands.Parameters;
using MockResponse.Api.Queries;
using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Commands
{
    public interface IResponseCommand : ICommand<ResponsePostParameters>
    {
    }
}
