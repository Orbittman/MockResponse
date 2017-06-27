namespace MockResponse.Web.Models
{
    public abstract class BaseViewModel
    {
        public bool Authenticated { get; set; }

        public ISiteRequestContext RequestContext { get; set; }
    }
}