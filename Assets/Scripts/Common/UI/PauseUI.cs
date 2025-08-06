using UnityEngine;

public class PauseUI : BaseUI
{
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();
        CloseUI();
    }

    public void OnClickHomeButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
}
