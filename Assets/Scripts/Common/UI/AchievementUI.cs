using Gpm.Ui;
using SuperMaxim.Messaging;
using UnityEngine;

public class AchievementUI : BaseUI
{
    public InfiniteScroll achievementScrollList;

    private void OnEnable()
    {
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        SetAchievementList();
        SortAchievementList();
    }

    private void SetAchievementList()
    {
        achievementScrollList.Clear();

        var achievementDataList = DataTableManager.Instance.GetAchievementDataTable();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();

        if(achievementDataList == null)
        {
            Logger.LogError("achievementDataList does not exist");
        }

        if(userAchievementData == null)
        {
            Logger.LogError("userAchievementData does not exist");
        }

        if(achievementDataList != null && userAchievementData != null)
        {
            foreach (var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                achievementItemData.achievementType = achievement.achievementType;
                var userAchieveData = userAchievementData.GetUserAchievementProgressData(achievement.achievementType);
                if(userAchieveData != null)
                {
                    achievementItemData.achievementAmount = userAchieveData.achivementAmount;
                    achievementItemData.isAchieved = userAchieveData.isAchieved;
                    achievementItemData.isRewardClaimed = userAchieveData.isRewardClaimed;
                }
                achievementScrollList.InsertData(achievementItemData);
            }
        }
    }

    private void SortAchievementList()
    {
        achievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;

            var AComp = achievementA.isAchieved && !achievementA.isRewardClaimed;
            var BComp = achievementB.isAchieved && !achievementB.isRewardClaimed;

            int compareResult = BComp.CompareTo(AComp);
            if(compareResult == 0)
            {
                compareResult = achievementA.isAchieved.CompareTo(achievementB.isAchieved);
                if(compareResult == 0)
                {
                    compareResult = (achievementA.achievementType).CompareTo(achievementB.achievementType);
                }
            }
            return compareResult;
        });
    }

    private void OnAchievementProgressed(AchievementProgressMsg msg)
    {
        SetAchievementList();
        SortAchievementList();
    }
}
