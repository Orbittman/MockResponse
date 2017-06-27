using MockResponse.Api.Commands.Parameters;
using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Commands
{
    public interface IPostResponseCommand : ICommand<Response, ResponsePostParameters>
    {
    }
}
