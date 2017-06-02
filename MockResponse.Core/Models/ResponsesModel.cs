using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MockResponse.Core.Models
{
    [DataContract]
    public class ResponsesModel
    {
        [DataMember(Name = "responses")]
        public List<ResponseModel> Responses { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
