using System.Collections;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController inGameUIController { get; private set; }
    public bool isPause { get; private set; }
    public bool isStageClear { get; private set; }

    private int m_SelectedChapter;
    private ChapterData m_currChapterData;
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages";
    private Transform m_StageTransform;
    private SpriteRenderer m_Background;

    private GameObject m_currentStageObj;
    public int coinCount = 0;
    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        InitVariable();
        LoadBackground();
        LoadStage();

        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);

    }

    private void InitVariable()
    {
        Logger.Log($"{GetType()}::InitVariable");

        m_StageTransform = GameObject.Find("Stage").transform;
        m_Background = GameObject.Find("Background").GetComponent<SpriteRenderer>();

        m_CurrStage = 1;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("userPlayData is null");
            return;
        }
        m_SelectedChapter = userPlayData.CurrentSelectedChapter;
        m_currChapterData = DataTableManager.Instance.GetChapterData(m_SelectedChapter);
        if(m_currChapterData == null)
        {
            Logger.LogError($"currChapterData not found. chapter:{m_SelectedChapter}");
        }
    }

    private void LoadBackground()
    {
        var bgTexture = Resources.Load<Texture2D>($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}");
        if (bgTexture != null)
        {
            m_Background.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        Logger.Log($"Chapter:{m_SelectedChapter}, Stage:{m_CurrStage}");

        if(m_currentStageObj)
        {
            Destroy(m_currentStageObj);
        }

        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}/{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        stageObj.transform.SetParent(m_StageTransform);
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;
        m_currentStageObj = stageObj;
    }

    private void Start()
    {
        inGameUIController = FindAnyObjectByType<InGameUIController>();
        if(inGameUIController == null)
        {
            Logger.LogError($"InGameUIController does not exist");
            return;
        }

        inGameUIController.Init();
    }

    private void Update()
    {
        CheckStageClear();
    }

    private void CheckStageClear()
    {
        if(isStageClear)
        {
            return;
        }

        if(coinCount > 5)
        {
            StageClear();
        }
    }

    private void StageClear()
    {
        Logger.Log("Stage Clear!!");

        isStageClear = true;
        coinCount = 0;
        StartCoroutine(ShowStageClearCoroutine());
    }

    IEnumerator ShowStageClearCoroutine()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData);

        yield return new WaitForSeconds(1f);

        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if(stageClearUI)
        {
            stageClearUI.CloseUI();
        }

        if(IsAllClear())
        {
            ChapterClearUI();
        }
        else
        {
            isStageClear = false;

            m_CurrStage++;
            LoadStage();
        }

            
    }

    private bool IsAllClear()
    {
        return m_currChapterData.totalStage == m_CurrStage;
    }

    private void ChapterClearUI()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear);
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.Log($"userPlayData cannot found");
            return;
        }
        var chapterClearData = new ChapterClearUIData();
        chapterClearData.clearChapter = m_SelectedChapter;
        chapterClearData.earnReward = m_SelectedChapter > userPlayData.MaxClearChapter;
        UIManager.Instance.OpenUI<ChapterClearUI>(chapterClearData);
        if(m_SelectedChapter > userPlayData.MaxClearChapter)
        {
            userPlayData.MaxClearChapter++;
            userPlayData.CurrentSelectedChapter = userPlayData.MaxClearChapter + 1;
            userPlayData.SaveData();
        }
    }


    public void PauseGame()
    {
        isPause = true;
        //필요한 pause코드 작성
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1f;
    }
}
