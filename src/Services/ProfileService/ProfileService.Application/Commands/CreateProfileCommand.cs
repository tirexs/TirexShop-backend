using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Commands;

public record CreateProfileCommand(
    Guid UserId,
    string Email,
    string FirstName,
    string MiddleName,
    string LastName,
    bool EmailVerified
) : IRequest<BaseResponse<Guid>>;