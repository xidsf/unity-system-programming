using TMPro;
using UnityEngine;

public class GoodsUI : MonoBehaviour
{
    public TextMeshProUGUI goldAmountText;
    public TextMeshProUGUI gemAmountText;

    public void SetValues()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError($"{GetType()} :: SetValue() - userGoodsData is null");
        }
        else
        {
            goldAmountText.text = userGoodsData.Gold.ToString("N0");
            gemAmountText.text = userGoodsData.Gem.ToString("N0");
        }

    }
}
