using System.Collections.Generic;

namespace MockResponse.Web.Models
{
    public class ListResponseViewModel : BaseViewModel
    {
        public IEnumerable<ResponseViewModel> Responses { get; set; }
    }
}
