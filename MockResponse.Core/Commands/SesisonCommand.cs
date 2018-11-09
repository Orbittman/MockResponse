using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Requests;
using MockResponse.Core.Utilities;

namespace MockResponse.Core.Commands
{
    public class SessionCommand : ISessionCommand
    {
        readonly INoSqlClient _dbClient;
        readonly IDateTimeProvider _dateTimeProvider;

        public SessionCommand(
            INoSqlClient dbClient,
            IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _dbClient = dbClient;
        }

        public void Execute(SaveSessionRecordRequest request)
        {
            _dbClient.InsertOne(new SessionRecord
            {
                Account = request.AccountId,
                SessionKey = request.SessionKey,
                Expiry = _dateTimeProvider.Now.Add(request.Expiry)
            }, nameof(SessionRecord));
        }
    }
}
