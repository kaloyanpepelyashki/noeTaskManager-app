using System.Text.Json.Serialization;

namespace noeTaskManager_app.Models
{
    public class TaskItem
    {
        [JsonPropertyName("id")]
        public TaskItemObjectId Id { get; set; }
        [JsonPropertyName("taskKey")]
        public string TaskKey { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("description")]
        public string Description {  get; set; }
        [JsonPropertyName("priority")]
        public string Priority { get; set; }
        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }
    }
}
