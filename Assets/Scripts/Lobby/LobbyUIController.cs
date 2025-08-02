using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI currChapterNameText;
    public RawImage currChapterBg;
    //RawImage는 그냥 이미지만 사용하는 것. 배경 택스쳐같이 여러 기능이 필요 없는 이미지는 RawImage가 적절

    public void Init()
    {
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrentChapter();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();

            if(frontUI)
            {
                frontUI.CloseUI();
            }
            else
            {
                var uiData = new ConfirmUIData();
                uiData.confirmType = confirmType.OK_CANCEL;
                uiData.titleText = "게임 종료";
                uiData.DescriptionText = "게임을 종료하시겠습니까?";
                uiData.OKButtonText = "종료";
                uiData.OnClickOKButton = () =>
                {
                    Application.Quit();
                };
                uiData.CancelButtonText = "취소";

                UIManager.Instance.OpenUI<ConfirmUI>(uiData);

            }
        }
    }

    public void SetCurrentChapter()
    {
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("userPlayData is null");
            return;
        }

        var currentSelectedChapter = DataTableManager.Instance.GetChapterData(userPlayData.CurrentSelectedChapter);
        if(currentSelectedChapter == null)
        {
            Logger.LogError("current Chapter Data is null");
            return;
        }

        currChapterNameText.text = currentSelectedChapter.chapterName;
        var bgTexture = Resources.Load<Texture2D>($"ChapterBG/Background_{userPlayData.CurrentSelectedChapter.ToString("D3")}");
        if(bgTexture != null)
        {
            currChapterBg.texture = bgTexture;
        }
    }    

    public void OnClickProfileButton()
    {
        Logger.Log($"{GetType()}::OnClickProfileButton()");
        var ui = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(ui);
    }

    public void OnClickSettingsButton()
    {
        Logger.Log($"{GetType()}:: OnClickSettingsButton");
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickCurrChapterImage()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }
}
