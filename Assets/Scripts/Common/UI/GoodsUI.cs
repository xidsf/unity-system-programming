using SuperMaxim.Messaging;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUpdateMsg
{
    public bool isAdd;
}

public class GemUpdateMsg
{
    public bool isAdd;
}

public class GoodsUI : MonoBehaviour
{
    public Image goldIcon;
    public TextMeshProUGUI goldAmountText;

    public Image gemIcon;
    public TextMeshProUGUI gemAmountText;
    

    private Coroutine m_GoldIncreaseCoroutine;
    private Coroutine m_GemIncreaseCoroutine;
    private const float GOODS_INCREASE_DURATION = 0.5f;

    private void OnEnable()
    {
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);
    }

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

    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("userGoodsData is null");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);

        if(goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCoroutine != null)
            {
                StopCoroutine(m_GoldIncreaseCoroutine);
            }
            m_GoldIncreaseCoroutine = StartCoroutine(IncreaseGoldCoroutine());
        }
        else
        {
            goldAmountText.text = userGoodsData.Gold.ToString("N0");
        }
    }

    private IEnumerator IncreaseGoldCoroutine()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError($"{GetType()}::userGoodsData is null");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            var goldObj = Instantiate(Resources.Load<GameObject>("UI/GoldMove"));
            goldObj.transform.SetParent(UIManager.Instance.m_UICanvasTransform);
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            goldObj.GetComponent<GoodsMove>().SetMove(i, goldIcon.transform.position);
        }

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTextValue = Convert.ToInt64(goldAmountText.text.Replace(",", ""));
        var destValue = userGoodsData.Gold;

        while(elapsedTime <= GOODS_INCREASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCREASE_DURATION);
            goldAmountText.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        goldAmountText.text = destValue.ToString("N0");
    }

    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("userGoodsData is null");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);

        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCoroutine != null)
            {
                StopCoroutine(m_GemIncreaseCoroutine);
            }
            m_GemIncreaseCoroutine = StartCoroutine(IncreaseGemCoroutine());
        }
        else
        {
            gemAmountText.text = userGoodsData.Gold.ToString("N0");
        }
    }

    private IEnumerator IncreaseGemCoroutine()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError($"{GetType()}::userGoodsData is null");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            var gemObj = Instantiate(Resources.Load<GameObject>("UI/GemMove"));
            gemObj.transform.SetParent(UIManager.Instance.m_UICanvasTransform);
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            gemObj.GetComponent<GoodsMove>().SetMove(i, gemIcon.transform.position);
        }

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTextValue = Convert.ToInt64(gemAmountText.text.Replace(",", ""));
        var destValue = userGoodsData.Gem;

        while (elapsedTime <= GOODS_INCREASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCREASE_DURATION);
            gemAmountText.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gemAmountText.text = destValue.ToString("N0");
    }
}
