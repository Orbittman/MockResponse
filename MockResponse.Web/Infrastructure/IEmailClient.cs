namespace MockResponse.Web.Infrastructure
{
	public interface IEmailClient
	{
		bool Send(string to, string subject, string body);
	}
}
