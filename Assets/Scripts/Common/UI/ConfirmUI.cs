using System;
using TMPro;
using UnityEngine.UI;

public enum confirmType
{
    OK,
    OK_CANCEL,
}

public class ConfirmUIData : BaseUIData
{
    public confirmType confirmType;
    public string titleText;
    public string DescriptionText;

    public string OKButtonText;
    public Action OnClickOKButton;
    public string CancelButtonText;
    public Action OnClickCancelButton;
}

public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button OKButton;
    public Button cancelButton;
    public TextMeshProUGUI OKButtonText;
    public TextMeshProUGUI cancelButtonText;

    private ConfirmUIData m_ConfirmData;
    private Action m_OnClickOKButton;
    private Action m_OnClickCancelButton;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);
        m_ConfirmData = data as ConfirmUIData;

        titleText.text = m_ConfirmData.titleText;
        descriptionText.text = m_ConfirmData.DescriptionText;
        OKButtonText.text = m_ConfirmData.OKButtonText;
        cancelButtonText.text = m_ConfirmData.CancelButtonText;
        m_OnClickOKButton = m_ConfirmData.OnClickOKButton;
        m_OnClickCancelButton = m_ConfirmData.OnClickCancelButton;

        OKButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(m_ConfirmData.confirmType == confirmType.OK_CANCEL);

    }

    public void OnClickOKButton()
    {
        m_OnClickOKButton?.Invoke();
        m_OnClickOKButton = null;
        CloseUI();
    }

    public void OnClickCancelButton()
    {
        m_OnClickCancelButton?.Invoke();
        m_OnClickCancelButton = null;
        CloseUI();
    }

}
