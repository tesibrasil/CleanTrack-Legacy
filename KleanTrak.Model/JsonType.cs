using System.Runtime.Serialization;

namespace KleanTrak.Model
{
    [DataContract]
    public class JsonType
    {
        [DataMember]
        public string MethodName { get; set; }
    }

}
