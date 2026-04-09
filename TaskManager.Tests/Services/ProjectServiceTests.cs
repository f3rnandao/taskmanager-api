using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _service = new ProjectService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateProjectAsync_ValidRequest_ShouldReturnProjectResponse()
    {
        var request = new CreateProjectRequest("My Project", "My description");

        var result = await _service.CreateProjectAsync(request, default);

        result.Should().NotBeNull();
        result.Name.Should().Be("My Project");
        result.Description.Should().Be("My description");
        result.TaskCount.Should().Be(0);
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateProjectAsync_ValidRequest_ShouldCallAddAndSave()
    {
        var request = new CreateProjectRequest("My Project", null);

        await _service.CreateProjectAsync(request, default);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>(), default), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetProjectByIdAsync_NonExistentId_ShouldReturnNull()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Project?)null);

        var result = await _service.GetProjectByIdAsync(Guid.NewGuid(), default);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnMappedResponses()
    {
        var projects = new List<Project>
        {
            Project.Create("Project A", null),
            Project.Create("Project B", "Description B")
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(projects);

        var result = await _service.GetAllProjectsAsync(default);

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Project A");
        result.Should().Contain(p => p.Name == "Project B");
    }
}