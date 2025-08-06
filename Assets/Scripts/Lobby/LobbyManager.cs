using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController lobbyUIController { get; private set; }

    //여러 이유로 인해 스타트버튼을 이미 눌렀는데 여러번 눌러도 인게임 변경 요청은 1번만 이루어지기 위한 변수
    private bool m_IsLoadingGame = false;

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;
        base.Init();

    }

    private void Start()
    {
        lobbyUIController = FindAnyObjectByType<LobbyUIController>();
        if(lobbyUIController == null)
        {
            Logger.LogError($"{GetType()} :: Start() : lobbyUIController is null");
            return;
        }

        lobbyUIController.Init();
        //AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    public void StartInGame()
    {
        if(m_IsLoadingGame)
        {
            return;
        }
        else
        {
            m_IsLoadingGame = true;
        }

        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
        {
            UIManager.Instance.CloseAllUI();
            SceneLoader.Instance.LoadScene(SceneType.InGame);
        });
    }
}
