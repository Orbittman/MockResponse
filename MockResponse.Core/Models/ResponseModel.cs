using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MockResponse.Core.Models
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember(Name="responseId")]
        public string Id { get; set; }

        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name= "content")]
        public string Content { get; set; }

        [DataMember(Name="path")]
        public string Path { get; set; }

        [DataMember(Name="headers")]
        public List<HeaderModel> Headers { get; set; }
    }
}
