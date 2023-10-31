using System.Runtime.Serialization;

namespace IBSAPI.Models
{
    public class TokenEntity
    {
        [IgnoreDataMember]
        public Int64 TokenId { get; set; }
        [IgnoreDataMember]
        public string UserId { get; set; }
        [DataMember]
        public string AuthToken { get; set; }
        [IgnoreDataMember]
        public System.DateTime IssuedOn { get; set; }
        [IgnoreDataMember]
        public System.DateTime ExpiresOn { get; set; }
    }
    public class TokenEntry
    {
        public string token { get; set; }
    }
}
