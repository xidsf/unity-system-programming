using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterClearUIData : BaseUIData
{
    public int clearChapter;
    public bool earnReward;
}

public class ChapterClearUI : BaseUI
{
    public GameObject rewards;
    public TextMeshProUGUI gemRewardAmountText;
    public TextMeshProUGUI goldRewardAmountText;
    public Button homeButton;
    public ParticleSystem[] clearFX;

    ChapterClearUIData m_ClearUIData;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        m_ClearUIData = data as ChapterClearUIData;
        if(m_ClearUIData == null)
        {
            Logger.LogError($"{GetType()}::data cannot convert to clearUIData");
            return;
        }

        var chapterData = DataTableManager.Instance.GetChapterData(m_ClearUIData.clearChapter);
        if(m_ClearUIData == null)
        {
            Logger.LogError($"{GetType()}::cannot found chapterData");
            return;
        }

        rewards.SetActive(m_ClearUIData.earnReward);
        if(m_ClearUIData.earnReward)
        {
            gemRewardAmountText.text = chapterData.chapterRewardGem.ToString();
            goldRewardAmountText.text = chapterData.ChapterRewardGold.ToString();

            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData == null)
            {
                Logger.LogError("UserGoodsData is null");
                return;
            }
            userGoodsData.Gold += chapterData.ChapterRewardGold;
            userGoodsData.Gem += chapterData.chapterRewardGem;
            userGoodsData.SaveData();

            var goldUpdateMsg = new GoldUpdateMsg();
            goldUpdateMsg.isAdd = true;
            Messenger.Default.Publish(goldUpdateMsg);

            var gemUpdateMsg = new GemUpdateMsg();
            gemUpdateMsg.isAdd = true;
            Messenger.Default.Publish(gemUpdateMsg);
        }

        homeButton.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ClearUIData.earnReward ? -250f : 50f, 0f);

        for (int i = 0; i < clearFX.Length; i++)
        {
            clearFX[i].Play();
        }
    }

    public void OnClickHomeButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);
        CloseUI();
    }

}
