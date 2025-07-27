using TMPro;
using UnityEngine;

public class SettingsUI : BaseUI
{
    public TextMeshProUGUI gameVersionText;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;

    private const string PRIVACY_POLICY_URL = "https://www.inflearn.com/";

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        SetGameVersion();

        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingsData != null)
        {
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    private void SetGameVersion()
    {
        gameVersionText.text = $"Version: {Application.version}";
    }

    private void SetSoundSetting(bool isSoundOn)
    {
        soundOnToggle.SetActive(isSoundOn);
        soundOffToggle.SetActive(!isSoundOn);
    }

    public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()} :: OnClickSoundOnToggle() : Click Sound On Button");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingData != null)
        {
            userSettingData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            SetSoundSetting(userSettingData.Sound);
        }
    }

    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()} :: OnClickSoundOnToggle() : Click Sound Off Button");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingData != null)
        {
            userSettingData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Unmute();
            SetSoundSetting(userSettingData.Sound);
        }
    }

    public void OnClickPrivacyPolicyURL()
    {
        Logger.Log($"{GetType()} :: OnClickPrivacyPolicyURL() : Click Privacy Policy URL");
        Application.OpenURL(PRIVACY_POLICY_URL);


    }
}
