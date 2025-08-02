using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScrollItemData : InfiniteScrollData
{
    public int chapterNum;
}

public class ChapterScrollItem : InfiniteScrollItem
{
    public GameObject currChapter;
    public RawImage currChapterBackground;
    public Image Dim;
    public Image lockIcon;
    public Image round;
    public ParticleSystem CommingSoonFx;
    public TextMeshProUGUI CommingSoonText;

    private ChapterScrollItemData m_ChapterScrollItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;
        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("ChapterScrollItemData is null");
            return;
        }

        if(m_ChapterScrollItemData.chapterNum > GlobalDefine.MAX_CHAPTER)
        {
            currChapter.SetActive(false);
            CommingSoonFx.gameObject.SetActive(true);
            CommingSoonText.gameObject.SetActive(true);
        }
        else
        {
            currChapter.SetActive(true);
            CommingSoonFx.gameObject.SetActive(false);
            CommingSoonText.gameObject.SetActive(false);

            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();

            if(userPlayData != null)
            {
                var isLocked = m_ChapterScrollItemData.chapterNum > userPlayData.MaxClearChapter + 1;
                Dim.gameObject.SetActive(isLocked);
                lockIcon.gameObject.SetActive(isLocked);
                round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }

            var backgroundTexture = Resources.Load<Texture2D>($"ChapterBG/Background_{m_ChapterScrollItemData.chapterNum.ToString("D3")}");
            if(backgroundTexture != null)
            {
                currChapterBackground.texture = backgroundTexture;
            }

        }
    }
}
