using Microsoft.AspNetCore.Mvc;

namespace MockResponse.Web.Models
{
    public abstract class BaseViewModel
    {
        public bool Authenticated { get; set; }

        public ISiteRequestContext RequestContext { get; set; }

        public IUrlHelper Url { get; set; }

        public string ClientStyles { get; internal set; }

        public string ClientJs { get; internal set; }
    }
}