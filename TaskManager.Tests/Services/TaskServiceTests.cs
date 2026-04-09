using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _service = new TaskService(_taskRepositoryMock.Object, _projectRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_ValidRequest_ShouldReturnTaskResponse()
    {
        var project = Project.Create("My Project", null);
        var request = new CreateTaskRequest(
            "Implement login",
            "Add JWT auth",
            TaskPriority.High,
            project.Id
        );

        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(project.Id, default))
            .ReturnsAsync(project);

        var result = await _service.CreateTaskAsync(request, default);

        result.Should().NotBeNull();
        result.Title.Should().Be("Implement login");
        result.Description.Should().Be("Add JWT auth");
        result.Priority.Should().Be(TaskPriority.High);
        result.Status.Should().Be(WorkTaskStatus.Todo);
        result.CompletedAt.Should().BeNull();
    }

    [Fact]
    public async Task CreateTaskAsync_ValidRequest_ShouldCallAddAndSave()
    {
        var project = Project.Create("My Project", null);
        var request = new CreateTaskRequest("Implement login", null, TaskPriority.Medium, project.Id);

        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(project.Id, default))
            .ReturnsAsync(project);

        await _service.CreateTaskAsync(request, default);

        _taskRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), default), Times.Once);
        _taskRepositoryMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_ProjectNotFound_ShouldThrowKeyNotFoundException()
    {
        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Project?)null);

        var request = new CreateTaskRequest("Task", null, TaskPriority.Low, Guid.NewGuid());

        var act = () => _service.CreateTaskAsync(request, default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*not found*");
    }

    [Fact]
    public async Task CompleteTaskAsync_ValidTask_ShouldReturnDoneStatus()
    {
        var project = Project.Create("My Project", null);
        var task = TaskItem.Create("Implement login", null, TaskPriority.High, project.Id);

        _taskRepositoryMock
            .Setup(r => r.GetByIdAsync(task.Id, default))
            .ReturnsAsync(task);

        var result = await _service.CompleteTaskAsync(task.Id, default);

        result.Status.Should().Be(WorkTaskStatus.Done);
        result.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task CompleteTaskAsync_TaskNotFound_ShouldThrowKeyNotFoundException()
    {
        _taskRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((TaskItem?)null);

        var act = () => _service.CompleteTaskAsync(Guid.NewGuid(), default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*not found*");
    }

    [Fact]
    public async Task GetTasksByProjectAsync_ShouldReturnMappedResponses()
    {
        var projectId = Guid.NewGuid();
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task A", null, TaskPriority.Low, projectId),
            TaskItem.Create("Task B", "Description", TaskPriority.High, projectId)
        };

        _taskRepositoryMock
            .Setup(r => r.GetAllByProjectAsync(projectId, default))
            .ReturnsAsync(tasks);

        var result = await _service.GetTasksByProjectAsync(projectId, default);

        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Title == "Task A");
        result.Should().Contain(t => t.Title == "Task B");
    }
}