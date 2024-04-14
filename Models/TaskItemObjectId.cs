using System.Text.Json.Serialization;

namespace noeTaskManager_app.Models
{
    public class TaskItemObjectId
    {
        [JsonPropertyName("timestamp")]
        public int TimeStamp {  get; set; }
        [JsonPropertyName("machine")]
        public int Machine { get; set; }
        [JsonPropertyName("pid")]
        public int Pid { get; set; }
        [JsonPropertyName("increment")]
        public int Increment { get; set; }
        [JsonPropertyName("creationTime")]
        public string CreationTime { get; set; }

    }
}
