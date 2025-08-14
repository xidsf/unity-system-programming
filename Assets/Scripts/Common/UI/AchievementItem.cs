using Gpm.Ui;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemData : InfiniteScrollData
{
    public AchievementType achievementType;
    public int achievementAmount;
    public bool isAchieved;
    public bool isRewardClaimed;
}

public class AchievementItem : InfiniteScrollItem
{
    public GameObject achievedBG;
    public GameObject unAchievedBG;
    public TextMeshProUGUI achievementNameText;
    public Slider achievementProgressSlider;
    public TextMeshProUGUI achievementProgressText;
    public Image rewardIcon;
    public TextMeshProUGUI rewardAmountText;
    public Button rewardClaimButton;
    public Image claimButonImage;
    public TextMeshProUGUI claimButtonText;

    private AchievementItemData m_AchievementItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        m_AchievementItemData = scrollData as AchievementItemData;
        if(m_AchievementItemData == null)
        {
            Logger.LogError("m_AchievementItemData is invalied");
            return;
        }

        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.achievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist");
            return;
        }

        achievedBG.SetActive(m_AchievementItemData.isAchieved);
        unAchievedBG.SetActive(!m_AchievementItemData.isAchieved);
        achievementNameText.text = achievementData.achievementName;
        achievementProgressSlider.value = (float)m_AchievementItemData.achievementAmount / achievementData.achievementGoal;
        achievementProgressText.text = $"{m_AchievementItemData.achievementAmount.ToString("N0")}/{achievementData.achievementGoal.ToString("N0")}";
        rewardAmountText.text = achievementData.AchievementRewardAmount.ToString("N0");

        var rewardTextureName = string.Empty;
        switch(achievementData.AchievementRewardType)
        {
            case GlobalDefine.RewardType.Gem:
                rewardTextureName = "IconGems";
                break;
            case GlobalDefine.RewardType.Gold:
                rewardTextureName = "IconGolds";
                break;
        }

        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}");
        if(rewardTexture != null)
        {
            rewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }

        rewardClaimButton.enabled = m_AchievementItemData.isAchieved && !m_AchievementItemData.isRewardClaimed;
        claimButonImage.color = rewardClaimButton.enabled ? Color.white : Color.gray;
        claimButtonText.color = rewardClaimButton.enabled ? Color.white : Color.gray;
    }

    public void OnClickRewardClaimButton()
    {
        if(!m_AchievementItemData.isAchieved || m_AchievementItemData.isRewardClaimed)
        {
            return;
        }

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("UserAchievementData does not exist");
            return;
        }

        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.achievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist");
            return;
        }

        var userAchievementProgressData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.achievementType);
        if(userAchievementProgressData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                userAchievementProgressData.isRewardClaimed = true;
                userAchievementData.SaveData();
                m_AchievementItemData.isRewardClaimed = true;

                switch(achievementData.AchievementRewardType)
                {
                    case GlobalDefine.RewardType.Gold:
                        userGoodsData.Gold += achievementData.AchievementRewardAmount;
                        var goldUpdateMsg = new GoldUpdateMsg();
                        goldUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(goldUpdateMsg);
                        userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                        break;
                    case GlobalDefine.RewardType.Gem:
                        userGoodsData.Gem += achievementData.AchievementRewardAmount;
                        var gemUpdateMsg = new GemUpdateMsg();
                        gemUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(gemUpdateMsg);
                        break;
                }
                userGoodsData.SaveData();
            }
        }
    }
}
