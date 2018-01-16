using Microsoft.Extensions.Configuration;
using MockResponse.Web.Configuration;

public class DomainContext : IDomainContext 
{
    public string ClientStyles => Configuration.Styles;

    public string ClientJs => Configuration.ClientJs;

    public AppConfig Configuration { get; set; }
}