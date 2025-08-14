using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using SuperMaxim.Messaging;

[Serializable]
public class UserAchievementProgressData
{
    public AchievementType achiementType;
    public int achivementAmount;
    public bool isAchieved;
    public bool isRewardClaimed;
}

[Serializable]
public class UserAchievementProgressDataListWrapper
{
    public List<UserAchievementProgressData> userAchivementProgressDataList;
}

public class AchievementProgressMsg
{

}

public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> UserAchievementProgressDataList { get; set; } = new();

    public void SetDefaultData()
    {
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::Load Data");

        bool result = false;

        try
        {
            string jsonData = PlayerPrefs.GetString("UserAchievementProgressDataList");
            if(!string.IsNullOrEmpty(jsonData))
            {
                var wrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(jsonData);
                UserAchievementProgressDataList = wrapper.userAchivementProgressDataList;

                Logger.Log("UserAchievementProgressDataList");

                foreach (var item in UserAchievementProgressDataList)
                {
                    Logger.Log($"AchivementType:{item.achiementType} AchievementAmount:{item.achivementAmount} isAchieved:{item.isAchieved} isRewardClaimed:{item.isRewardClaimed}");
                }
            }
            result = true;
        }
        catch (Exception e)
        {
            Logger.LogError($"userAchievementProgressData Load Failed. {e.Message}");
        }
        return result;
    }

    public bool SaveData()
    {
        bool result = false;

        try
        {
            UserAchievementProgressDataListWrapper wrapper = new();
            wrapper.userAchivementProgressDataList = UserAchievementProgressDataList;
            string jsonData = JsonUtility.ToJson(wrapper);

            PlayerPrefs.SetString("UserAchievementProgressDataList", jsonData);
            PlayerPrefs.Save();

            foreach (var item in UserAchievementProgressDataList)
            {
                Logger.Log($"AchivementType:{item.achiementType} AchievementAmount:{item.achivementAmount} isAchieved:{item.isAchieved} isRewardClaimed:{item.isRewardClaimed}");
            }

            result = true;
        }
        catch (Exception e)
        {
            Logger.LogError($"userAchievementProgressData Save Failed. {e.Message}");
        }

        return result;
    }

    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achivemenetType)
    {
        return UserAchievementProgressDataList.Where(item => item.achiementType == achivemenetType).FirstOrDefault();
    }

    public void ProgressAchievement(AchievementType achievementType, int achievementAmount)
    {
        var achivemenetData = DataTableManager.Instance.GetAchievementData(achievementType);

        if(achivemenetData == null)
        {
            Logger.LogError("AchievementData does not exist");
            return;
        }

        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        if(userAchievementProgressData == null)
        {
            userAchievementProgressData = new();
            userAchievementProgressData.achiementType = achievementType;
            UserAchievementProgressDataList.Add(userAchievementProgressData);
        }

        if(!userAchievementProgressData.isAchieved)
        {
            userAchievementProgressData.achivementAmount += achievementAmount;
            if(userAchievementProgressData.achivementAmount > achivemenetData.achievementGoal)
            {
                userAchievementProgressData.achivementAmount = achivemenetData.achievementGoal;
            }
            if(userAchievementProgressData.achivementAmount == achivemenetData.achievementGoal)
            {
                userAchievementProgressData.isAchieved = true;
            }

            SaveData();

            AchievementProgressMsg achievementProgressMsg = new();
            Messenger.Default.Publish(achievementProgressMsg);
        }
        
    }
}
