namespace SSW.Rewards.Application.Notifications.Commands.DeleteInstallation;

public class DeleteInstallationCommand : IRequest<Unit>
{
    public string InstallationId { get; set; }

    public DeleteInstallationCommand(string installationId)
    {
        InstallationId = installationId;
    }
}

public class DeleteInstallationCommandHandler : IRequestHandler<DeleteInstallationCommand, Unit>
{
    private readonly INotificationService _notificationService;

    public DeleteInstallationCommandHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public async Task<Unit> Handle(DeleteInstallationCommand request, CancellationToken cancellationToken)
    {
        var success = await _notificationService
            .DeleteInstallationByIdAsync(request.InstallationId, CancellationToken.None);

        if (!success)
            throw new Exception("Bad Request");

        return Unit.Value;
    }
}