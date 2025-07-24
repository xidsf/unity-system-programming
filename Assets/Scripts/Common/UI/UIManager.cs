using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform m_UICanvasTransform;
    public Transform m_ClosedUITransform;

    private BaseUI m_FrontUI;
    private Dictionary<System.Type, GameObject> m_OpendUIPool = new Dictionary<System.Type, GameObject>();
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();

    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (m_OpendUIPool.ContainsKey(uiType))
        {
            ui = m_OpendUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType);
        }
        else
        {
            var uiObj = Instantiate(Resources.Load<GameObject>($"UI/{uiType}"));
            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }

    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);
        Logger.Log($"{GetType()}:: OpenUI({uiType})");
        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);

        if(ui == null)
        {
            Logger.LogError($"{uiType} does not exist.");
            return;
        }

        if (isAlreadyOpen)
        {
            Logger.LogWarning($"{uiType} is already open.");
            return;
        }

        var siblingIndex = m_UICanvasTransform.childCount;
        ui.Init(m_UICanvasTransform);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        m_FrontUI = ui;
        m_OpendUIPool[uiType] = ui.gameObject;
    }

    public void CloseUI(BaseUI ui)
    {
        Logger.Log($"{GetType()}:: CloseUI({ui})");
        ui.gameObject.SetActive(false);
        m_OpendUIPool.Remove(ui.GetType());
        m_ClosedUIPool[ui.GetType()] = ui.gameObject;
        ui.transform.SetParent(m_ClosedUITransform);
        m_FrontUI = null;

        var lastChild = m_UICanvasTransform.GetChild(m_UICanvasTransform.childCount - 1);
        if(lastChild)
        {
            m_FrontUI = lastChild.GetComponent<BaseUI>();
        }
    }

    public BaseUI GetActiveUI<T>()
    {
        var uiType = typeof(T);
        return m_OpendUIPool.ContainsKey(uiType) ? m_OpendUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    public bool ExistsOpenUI()
    {
        return m_FrontUI != null;
    }

    public BaseUI GetCurrentUI()
    {
        return m_FrontUI;
    }

    public void CloseCurrentFrontUI()
    {
        m_FrontUI?.CloseUI();
    }

    public void CloseAllUI()
    {
        while(!m_FrontUI)
        {
            m_FrontUI.CloseUI();
        }
    }
}
