using MediatR;
using Microsoft.Extensions.Logging;
using ProfileService.Application.Commands;
using ProfileService.Application.Models;
using ProfileService.Domain.Entities;
using ProfileService.Domain.Interfaces.Repositories;

namespace ProfileService.Application.Handlers;

public sealed class CreateProfileCommandHandler :
    BaseHandler<CreateProfileCommandHandler>,
    IRequestHandler<CreateProfileCommand, BaseResponse<Guid>>
{
    private readonly IProfileRepository _profileRepository;

    public CreateProfileCommandHandler(
        IProfileRepository profileRepository,
        ILogger<CreateProfileCommandHandler> logger) : base(logger)
    {
        _profileRepository = profileRepository;
    }

    public async Task<BaseResponse<Guid>> Handle(
        CreateProfileCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Проверка на существование пользователя
            if (await _profileRepository.ExistsByIdAsync(request.UserId, cancellationToken))
            {
                throw new InvalidOperationException($"Профиль пользователя с Id '{request.UserId}' уже существует");
            }

            // Создание доменного объекта
            var userProfile = new Profile(
                id: request.UserId,
                email: request.Email,
                firstName: request.FirstName,
                middleName: request.MiddleName,
                lastName: request.LastName,
                emailVerified: request.EmailVerified);

            // Сохранение
            await _profileRepository.AddAsync(userProfile, cancellationToken);
            await _profileRepository.SaveAsync(cancellationToken);

            return GetSuccessResponse(userProfile.Id, $"Профиль пользователя с Id '{userProfile.Id}' успешно создан");
        }
        catch (OperationCanceledException)
        {
            return GetFailureResponse<Guid>($"Не удалось создать профиль для пользователя с Id '{request.UserId}'. Операция отменена");
        }
        catch (Exception ex)
        {
            return GetFailureResponse<Guid>($"Не удалось создать профиль для пользователя с Id '{request.UserId}'. {ex.Message}");
        }
    }
}