﻿using Microsoft.Extensions.Logging;

namespace SSW.Rewards.Application.Users.Commands.DeleteMyProfile;

public class DeleteMyProfileCommand : IRequest { }

public class DeleteMyProfileCommandHandler : IRequestHandler<DeleteMyProfileCommand, Unit>
{
    private readonly IEmailService _emailService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<DeleteMyProfileCommandHandler> _logger;

    public DeleteMyProfileCommandHandler(IEmailService emailService, ICurrentUserService currentUserService, ILogger<DeleteMyProfileCommandHandler> logger)
    {
        _emailService = emailService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteMyProfileCommand request, CancellationToken cancellationToken)
    {
        var userName = _currentUserService.GetUserFullName();
        var userEmail = _currentUserService.GetUserEmail();

        var model = new DeleteProfileEmail
        {
            UserName = userName,
            UserEmail = userEmail,
            // TODO: Technical debt - needs to be switched to an appropriate
            //       distribution group. See Zendesk ticket #12202.
            RewardsTeamEmail = "mattgoldman@ssw.com.au",
        };

        var sent = await _emailService.SendProfileDeletionRequest(model, cancellationToken);

        if (sent)
        {
            return Unit.Value;
        }
        else
        {
            _logger.LogError("Could not send profile delete request message for {userName}, {email}", userName, userEmail);
            throw new Exception($"Failed to send profile delete request message for {userName}, {userEmail}");
        }
    }
}