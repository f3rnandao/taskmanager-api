namespace TaskManager.Application.DTOs;

public record AuthResponse(
    Guid UserId,
    string Name,
    string Email,
    string Token,
    DateTime ExpiresAt
);