using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterListUI : BaseUI
{
    public InfiniteScroll chapterScrollList;
    public GameObject selectedChapterName;
    public TextMeshProUGUI selectedChapterText;
    public Button selectButton;

    private int selectedChapter;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            return;
        }

        selectedChapter = userPlayData.CurrentSelectedChapter;
        SetSelectedChapter();
        SetChapterScrollList();

        chapterScrollList.MoveTo(selectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        chapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if(chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };

    }

    private void SetSelectedChapter()
    {
        if(selectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            selectedChapterName.SetActive(true);
            selectButton.gameObject.SetActive(true);

            var itemData = DataTableManager.Instance.GetChapterData(selectedChapter);
            if(itemData != null)
            {
                selectedChapterText.text = itemData.chapterName;
            }
        }
        else
        {
            selectedChapterName.SetActive(false);
            selectButton.gameObject.SetActive(false);
        }
    }

    private void SetChapterScrollList()
    {
        chapterScrollList.Clear();

        for (int i = 1; i <= GlobalDefine.MAX_CHAPTER + 1; i++)
        {
            var chapterItemData = new ChapterScrollItemData();
            chapterItemData.chapterNum = i;
            chapterScrollList.InsertData(chapterItemData);
        }
    }

    private void OnSnap(int selectedChapter)
    {
        this.selectedChapter = selectedChapter;
        SetSelectedChapter();
    }

    public void OnClickSelect()
    {
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist");
            return;
        }

        if(selectedChapter <= userPlayData.MaxClearChapter + 1)
        {
            userPlayData.CurrentSelectedChapter = selectedChapter;
            LobbyManager.Instance.lobbyUIController.SetCurrentChapter();
            CloseUI();
        }
    }
}
