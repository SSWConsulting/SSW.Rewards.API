﻿using Microsoft.Extensions.Logging;

namespace SSW.Rewards.Application.Rewards.Commands;

public class ClaimRewardForUserCommand : IRequest<ClaimRewardResult>
{
    public int UserId { get; set; }
    public string Code { get; set; }
}

public class ClaimRewardForUserCommandHandler : IRequestHandler<ClaimRewardForUserCommand, ClaimRewardResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ClaimRewardForUserCommand> _logger;

    public ClaimRewardForUserCommandHandler(
            IApplicationDbContext context,
            ILogger<ClaimRewardForUserCommand> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ClaimRewardResult> Handle(ClaimRewardForUserCommand request, CancellationToken cancellationToken)
    {
        var reward = await _context
            .Rewards
            .Where(r => r.Code == request.Code)
            .FirstOrDefaultAsync(cancellationToken);

        if (reward == null)
        {
            _logger.LogError("Reward not found with code: {0}", request.Code);
            return new ClaimRewardResult
            {
                status = RewardStatus.NotFound
            };
        }

        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
            .Include(u => u.UserAchievements).ThenInclude(ua => ua.Achievement)
            .FirstOrDefaultAsync(cancellationToken);

        var userRewards = await _context
                .UserRewards
                .Include(ur => ur.Reward)
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync(cancellationToken);

        int pointsUsed = userRewards.Sum(ur => ur.Reward.Cost);

        int balance = user.UserAchievements.Sum(ua => ua.Achievement.Value) - pointsUsed;

        if (balance < reward.Cost)
        {
            _logger.LogInformation("User does not have enough points to claim reward");
            return new ClaimRewardResult
            {
                status = RewardStatus.NotEnoughPoints
            };
        }

        try
        {
            await _context
                .UserRewards
                .AddAsync(new UserReward
                {
                    UserId = user.Id,
                    RewardId = reward.Id
                }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return new ClaimRewardResult
            {
                status = RewardStatus.Error
            };
        }

        return new ClaimRewardResult
        {
            status = RewardStatus.Claimed
        };
    }
}