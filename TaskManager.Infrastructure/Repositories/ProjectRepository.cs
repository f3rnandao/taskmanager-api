using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) => _context = context;

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken ct)
        => await _context.Projects
            .Include(p => p.Tasks)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task AddAsync(Project project, CancellationToken ct)
        => await _context.Projects.AddAsync(project, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}