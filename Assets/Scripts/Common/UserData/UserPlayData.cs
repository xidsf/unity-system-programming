using UnityEngine;

public class UserPlayData : IUserData
{
    public int MaxClearChapter { get; set; }
    public int CurrentSelectedChapter { get; set; } = 1;
    //not saved in PlayerPrefs
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        MaxClearChapter = 2;
        CurrentSelectedChapter = 1;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            MaxClearChapter = PlayerPrefs.GetInt("MaxClearChapter");
            CurrentSelectedChapter = MaxClearChapter + 1;

            result = true;

            Logger.Log($"Max Clear Chapter Loaded: {MaxClearChapter}");
        }
        catch(System.Exception e)
        {
            Logger.Log($"loadFailed : ({e.Message})");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            PlayerPrefs.SetInt("MaxClearChapter", MaxClearChapter);

            result = true;

            Logger.Log($"Max Clear Chapter Saved: {MaxClearChapter}");
        }
        catch (System.Exception e)
        {
            Logger.Log($"loadFailed : ({e.Message})");
        }

        return result;
    }

    
}
