using MockResponse.Web.Configuration;

public interface IDomainContext
{
    string ClientStyles { get; }

    string ClientJs { get; }

    AppConfig Configuration { get; set; }
}