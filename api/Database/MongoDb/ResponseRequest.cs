namespace MockResponse.Api.Database.MongoDb
{
    public class ResponseRequest
    {
        public ResponseRequest(string host, string path)
        {
            Host = host;
            Path = path;
        }

        public string Path { get; private set; }

        public string Host { get; private set; }
    }
}
