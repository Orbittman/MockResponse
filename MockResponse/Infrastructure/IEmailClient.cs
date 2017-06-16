namespace MockResponse.Web.Infrastructure
{
	public interface IEmailClient
	{
		bool Send(string to, string from, string body);
	}
}
