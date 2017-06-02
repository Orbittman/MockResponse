namespace MockResponse.Site.Infrastructure
{
	public class EmailClient : IEmailClient
	{
		public bool Send(string to, string @from, string body)
		{
			return true;
		}
	}
}
