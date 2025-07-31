using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController lobbyUIController { get; private set; }

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
}
