using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.DTO
{

    public class MatchingRule
    {
        public string tag { get; set; }
        public long id { get; set; }
        public string id_str { get; set; }
    }
}