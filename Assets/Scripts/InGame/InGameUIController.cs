using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {

    }

    private void OnApplicationFocus(bool focus) //OS마다 동작이 살짝 다르기 때문에 유의가 필요함
    {
        if(!focus)
        {
            if(!InGameManager.Instance.isPause && !InGameManager.Instance.isStageClear)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
            }
        }
    }

    private void Update()
    {
        if(!InGameManager.Instance.isPause && !InGameManager.Instance.isStageClear)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            InGameManager.Instance.PauseGame();
        }
    }

    public void OnClickPauseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
}
