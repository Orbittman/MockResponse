using System.Runtime.Serialization;

namespace MockResponse.Core.Models
{
    [DataContract]
    public class HeaderModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
