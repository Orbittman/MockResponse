namespace MockResponse.Web.Configuration
{
    public class AppConfig
    {
        public string ApiAddress { get; set; }
        public string Styles { get; internal set; }
        public string ClientJs { get; internal set; }
        public object MongoUsername { get; internal set; }
        public object MongoPassword { get; internal set; }
        public double SessionDuration { get; internal set; }
    }
}
