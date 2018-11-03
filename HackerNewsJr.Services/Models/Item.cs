using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HackerNewsJr.Services.Models
{


    [DataContract]
    public class Item
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "deleted")]
        public bool? Deleted { get; set; }

        [DataMember(Name = "type")]
        public ItemType Type { get; set; }

        [DataMember(Name = "by")]
        public string By { get; set; }

        [DataMember(Name = "time")]
        public long? Time { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "dead")]
        public bool? Dead { get; set; }

        [DataMember(Name = "parent")]
        public int? Parent { get; set; }

        [DataMember(Name = "poll")]
        public int? Poll { get; set; }

        [DataMember(Name = "kids")]
        public IEnumerable<int> Kids { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "score")]
        public int? Score { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "parts")]
        public IEnumerable<int> Parts { get; set; }

        [DataMember(Name = "descendants")]
        public int? Descendants { get; set; }
    }

    [DataContract(Name = "type")]
    public enum ItemType
    {
        [EnumMember(Value = "job")]
        Job,
        [EnumMember(Value = "story")]
        Story,
        [EnumMember(Value = "comment")]
        Comment,
        [EnumMember(Value = "poll")]
        Poll,
        [EnumMember(Value = "pollopt")]
        Pollopt,
    }
}
