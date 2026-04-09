using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IEnumerable<TaskItem>> GetAllByProjectAsync(Guid projectId, CancellationToken ct)
        => await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(TaskItem task, CancellationToken ct)
        => await _context.Tasks.AddAsync(task, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}