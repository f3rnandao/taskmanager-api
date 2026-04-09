using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class ProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
        => _repository = repository;

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken ct)
    {
        var project = Project.Create(request.Name, request.Description);

        await _repository.AddAsync(project, ct);
        await _repository.SaveChangesAsync(ct);

        return ToResponse(project);
    }

    public async Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync(CancellationToken ct)
    {
        var projects = await _repository.GetAllAsync(ct);
        return projects.Select(ToResponse);
    }

    public async Task<ProjectResponse?> GetProjectByIdAsync(Guid id, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(id, ct);
        return project is null ? null : ToResponse(project);
    }

    private static ProjectResponse ToResponse(Project project) => new(
        project.Id,
        project.Name,
        project.Description,
        project.CreatedAt,
        project.Tasks.Count
    );
}