using TaskManager.Domain.Enums;
using TimeZoneConverter;

namespace TaskManager.Application.DTOs;

public record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    WorkTaskStatus Status,
    TaskPriority Priority,
    Guid ProjectId,
    DateTime CreatedAt,
    DateTime? CompletedAt
)
{
    private static readonly TimeZoneInfo BrasiliaZone =
        TZConvert.GetTimeZoneInfo("America/Sao_Paulo");

    public DateTime CreatedAtBrasilia =>
        TimeZoneInfo.ConvertTimeFromUtc(CreatedAt, BrasiliaZone);

    public DateTime? CompletedAtBrasilia => CompletedAt.HasValue
        ? TimeZoneInfo.ConvertTimeFromUtc(CompletedAt.Value, BrasiliaZone)
        : null;
}