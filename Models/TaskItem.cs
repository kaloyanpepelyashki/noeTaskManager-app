namespace noeTaskManager_app.Models
{
    public class TaskItem
    {
        string TaskKey { get; set; }
        string Summary { get; set; }  
        string Description {  get; set; }
        string Priority { get; set; }
        string DueDate { get; set; }
    }
}
