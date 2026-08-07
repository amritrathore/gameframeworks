using System;

namespace Core.SlotSystem
{
    public sealed class TaskUnlockStrategy
        : ISlotUnlockStrategy
    {
        private readonly ITaskService taskService;
        private readonly string taskId;

        public TaskUnlockStrategy(
            ITaskService taskService,
            string taskId)
        {
            this.taskService =
                taskService ??
                throw new ArgumentNullException(
                    nameof(taskService));

            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException(
                    "Task ID cannot be empty.",
                    nameof(taskId));

            this.taskId = taskId;
        }

        public bool CanUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            return taskService.IsCompleted(taskId);
        }

        public bool TryUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            if (!taskService.IsCompleted(taskId))
                return false;

            return slot.MarkUnlocked();
        }
    }
}