namespace Core.SlotSystem
{
    public interface ITaskService
    {
        bool IsCompleted(string taskId);
    }
}