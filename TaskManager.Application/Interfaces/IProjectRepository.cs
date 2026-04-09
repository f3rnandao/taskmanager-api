using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Project project, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}