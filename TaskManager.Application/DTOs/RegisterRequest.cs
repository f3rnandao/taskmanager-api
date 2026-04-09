namespace TaskManager.Application.DTOs;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);