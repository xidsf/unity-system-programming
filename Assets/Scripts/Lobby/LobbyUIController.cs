using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void Init()
    {
        UIManager.Instance.EnableGoodsUI(true);
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
}
