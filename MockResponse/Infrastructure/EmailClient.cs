using NETCore.MailKit.Core;

namespace MockResponse.Web.Infrastructure
{
	public class EmailClient : IEmailClient
	{
	    private readonly IEmailService _emailService;

	    public EmailClient(IEmailService emailService)
	    {
	        _emailService = emailService;
	    }

		public bool Send(string to, string subject, string body)
		{
		    try
		    {
		        _emailService.Send(to, subject, body, true);

                return true;
		    }
		    catch
		    {
		        return false;
		    }
		}
	}
}
