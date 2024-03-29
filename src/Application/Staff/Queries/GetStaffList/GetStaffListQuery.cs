﻿using AutoMapper.QueryableExtensions;

namespace SSW.Rewards.Application.Staff.Queries.GetStaffList;

public class GetStaffListQuery : IRequest<StaffListViewModel>
{
    public sealed class Handler : IRequestHandler<GetStaffListQuery, StaffListViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public Handler(
            IMapper mapper,
            IApplicationDbContext dbContext,
            IUserService userService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<StaffListViewModel> Handle(GetStaffListQuery request, CancellationToken cancellationToken)
        {
            var staffDtos = await _dbContext.StaffMembers
                .ProjectTo<StaffDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var user = await _userService.GetCurrentUser(cancellationToken);

            var achievements = await _userService.GetUserAchievements(user.Id, cancellationToken);

            var completedAchievements = achievements.UserAchievements
                .Where(a => a.Complete)
                .Select(a => a.AchievementId)
                .ToList();

            foreach (var dto in staffDtos)
            {
                if (dto?.StaffAchievement?.Id != null)
                {
                    if ((bool)(completedAchievements?.Contains(dto.StaffAchievement.Id)))
                    {
                        dto.Scanned = true;
                    }
                }

            }

            return new StaffListViewModel
            {
                Staff = staffDtos
            };
        }
    }
}
