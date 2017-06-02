namespace MockResponse.Site.Infrastructure
{
	public interface IEmailClient
	{
		bool Send(string to, string from, string body);
	}
}
