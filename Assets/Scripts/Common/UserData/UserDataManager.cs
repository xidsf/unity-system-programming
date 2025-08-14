using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    //저장된 유저 데이터 존재 여부
    public bool ExistsSavedData { get; private set; }
    //모든 유저 데이터 인스턴스를 저장하는 컨테이너
    public List<IUserData> userDataList { get; private set; } = new List<IUserData>();

    protected override void Init()
    {
        base.Init();

        //모든 유저 데이터를 UserDataList에 추가
        userDataList.Add(new UserSettingsData());
        userDataList.Add(new UserGoodsData());
        userDataList.Add(new UserInventoryData());
        userDataList.Add(new UserPlayData());
        userDataList.Add(new UserAchievementData());
    }

    public void SetDefaultUserData()
    {
        for (int i = 0; i < userDataList.Count; i++)
        {
            userDataList[i].SetDefaultData();
        }
    }

    public void LoadUserData()
    {
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        if(ExistsSavedData)
        {
            for (int i = 0; i < userDataList.Count; i++)
            {
                userDataList[i].LoadData();
            }
        }
    }

    public void SaveUserData()
    {
        bool hasSaveError = false;

        for (int i = 0; i < userDataList.Count; i++)
        {
            bool isSaveSuccess = userDataList[i].SaveData();
            if(!isSaveSuccess)
            {
                hasSaveError = true;
            }
        }

        if(!hasSaveError)
        {
            ExistsSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            PlayerPrefs.Save();
        }
    }

    public T GetUserData<T>() where T : class, IUserData
    {
        return userDataList.OfType<T>().FirstOrDefault();
    }
}
