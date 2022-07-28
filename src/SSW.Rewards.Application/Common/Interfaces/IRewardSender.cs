﻿using SSW.Rewards.Domain.Entities;

namespace SSW.Rewards.Application.Common.Interfaces;

public interface IRewardSender
{
    Task SendReward(User user, Reward reward);
    Task SendRewardAsync(User user, Reward reward, CancellationToken cancellationToken);
}
