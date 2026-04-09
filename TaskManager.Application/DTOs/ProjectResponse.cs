using TimeZoneConverter;

namespace TaskManager.Application.DTOs;

public record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    int TaskCount
)
{
    private static readonly TimeZoneInfo BrasiliaZone =
        TZConvert.GetTimeZoneInfo("America/Sao_Paulo");

    public DateTime CreatedAtBrasilia =>
        TimeZoneInfo.ConvertTimeFromUtc(CreatedAt, BrasiliaZone);
}