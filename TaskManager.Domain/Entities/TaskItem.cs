using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public WorkTaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public Guid ProjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private TaskItem() { }

    public static TaskItem Create(string title, string? description,
        TaskPriority priority, Guid projectId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = WorkTaskStatus.Todo,
            Priority = priority,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Complete()
    {
        if (Status == WorkTaskStatus.Done)
            throw new InvalidOperationException("Task is already completed.");

        Status = WorkTaskStatus.Done;
        CompletedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        Title = title;
    }
}