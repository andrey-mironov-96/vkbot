using Newtonsoft.Json;
namespace FirstBot.Models
{
    public class VkConfirm
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("group_id")]
        public long group_id { get; set; }
    }
}