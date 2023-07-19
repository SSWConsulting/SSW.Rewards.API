namespace SSW.Rewards.Application.Achievements.Command.ClaimFormCompletedAchievement;

public class ClaimFormCompletedAchievementCommand : IRequest
{
    public string Email { get; set; }

    public string IntegrationId { get; set; }
}

public class ClaimFormCompletedAchievementCommandHandler : IRequestHandler<ClaimFormCompletedAchievementCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public ClaimFormCompletedAchievementCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(ClaimFormCompletedAchievementCommand request, CancellationToken cancellationToken)
    {
        var achievement = await _dbContext.Achievements.FirstOrDefaultAsync(a => a.IntegrationId == request.IntegrationId);

        if (achievement is not null)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (user is not null)
            {
                user.UserAchievements.Add(new UserAchievement
                {
                    Achievement = achievement
                });
            }
            else
            {
                var unclaimed = new UnclaimedAchievement
                {
                    Achievement = achievement,
                    EmailAddress = request.Email
                };

                _dbContext.UnclaimedAchievements.Add(unclaimed);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
