using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, ct);

        if (project is null)
            throw new KeyNotFoundException($"Project {request.ProjectId} not found.");

        var task = TaskItem.Create(
            request.Title,
            request.Description,
            request.Priority,
            request.ProjectId
        );

        await _taskRepository.AddAsync(task, ct);
        await _taskRepository.SaveChangesAsync(ct);

        return ToResponse(task);
    }

    public async Task<TaskResponse> CompleteTaskAsync(Guid taskId, CancellationToken ct)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, ct);

        if (task is null)
            throw new KeyNotFoundException($"Task {taskId} not found.");

        task.Complete();
        await _taskRepository.SaveChangesAsync(ct);

        return ToResponse(task);
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksByProjectAsync(Guid projectId, CancellationToken ct)
    {
        var tasks = await _taskRepository.GetAllByProjectAsync(projectId, ct);
        return tasks.Select(ToResponse);
    }

    private static TaskResponse ToResponse(TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.Priority,
        task.ProjectId,
        task.CreatedAt,
        task.CompletedAt
    );
}