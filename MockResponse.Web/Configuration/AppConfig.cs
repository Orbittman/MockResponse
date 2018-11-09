namespace MockResponse.Web.Configuration
{
    public class AppConfig
    {
        public string ApiAddress { get; set; }
        public string Styles { get; set; }
        public string ClientJs { get; set; }
        public object MongoUserName { get; set; }
        public object MongoPassword { get; set; }
        public double SessionDuration { get; set; }
        public string MongoConnectionString { get; set; }
    }
}
