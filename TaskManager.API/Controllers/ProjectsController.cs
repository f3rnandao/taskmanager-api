using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;

namespace TaskManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
        => _projectService = projectService;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var projects = await _projectService.GetAllProjectsAsync(ct);
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var project = await _projectService.GetProjectByIdAsync(id, ct);

        if (project is null)
            return NotFound(new { message = $"Project {id} not found." });

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProjectRequest request, CancellationToken ct)
    {
        var project = await _projectService.CreateProjectAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }
}