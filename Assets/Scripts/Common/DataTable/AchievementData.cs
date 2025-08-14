using Gpm.Ui;
using UnityEngine;
using static GlobalDefine;

public enum AchievementType
{
    CollectGold,
    ClearChapter1,
    ClearChapter2,
    ClearChapter3,
}

public class AchievementData
{
    public AchievementType achievementType;
    public string achievementName;
    public int achievementGoal;
    public RewardType AchievementRewardType;
    public int AchievementRewardAmount;
}
