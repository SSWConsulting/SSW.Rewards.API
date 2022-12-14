using SSW.Rewards.Application.Common.Mappings;

namespace SSW.Rewards.Application.Achievements.Queries.Common;

public class AchievementDto : IMapFrom<Achievement>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public AchievementType Type { get; set; }
}