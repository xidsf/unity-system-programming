using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIData : BaseUIData
{
    public long serialNumber;
    public int itemID;
    public bool isEquipped;
}

public class EquipmentUI : BaseUI
{
    public TextMeshProUGUI itemGradeText;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPowerText;
    public TextMeshProUGUI itemDefenseText;
    public TextMeshProUGUI equipButtonText;

    public Image itemIconImage;
    public Image itemGradeBackground;

    private EquipmentUIData m_EquipmentUIData;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        m_EquipmentUIData = data as EquipmentUIData;
        if(m_EquipmentUIData == null)
        {
            Logger.LogError($"{GetType()}:: m_EquipementUIData is Invalid");
            return;
        }

        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.itemID);

        if(itemData == null)
        {
            Logger.LogError($"Item data is Invalid. ItemData: {m_EquipmentUIData.itemID}");
            return;
        }

        var itemGrade = (ItemGrade)((itemData.itemID / 1000) % 10);
        var backgroundTexture = Resources.Load<Texture2D>($"Textures/{itemGrade.ToString()}");
        if(backgroundTexture != null)
        {
            itemGradeBackground.sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(1f, 1f));
        }
        
        var hexColor = string.Empty;
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "#EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }

        Color color;
        if(ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            itemGradeText.color = color;
        }

        StringBuilder sb = new StringBuilder(m_EquipmentUIData.itemID.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if (itemIconTexture != null)
        {
            itemIconImage.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        itemGradeText.text = itemGrade.ToString();
        itemNameText.text = itemData.itemName;
        itemPowerText.text = '+'+itemData.attackPower.ToString();
        itemDefenseText.text = '+' + itemData.defense.ToString();

        equipButtonText.text = m_EquipmentUIData.isEquipped ? "Unequip" : "Equip";
    }

    public void OnClickEquipButton()
    {
        var intenvoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(intenvoryData == null)
        {
            Logger.LogError("inventoryData is null");
            return;
        }

        if (m_EquipmentUIData.isEquipped)
        {
            intenvoryData.UnequipItem(m_EquipmentUIData.serialNumber, m_EquipmentUIData.itemID);
        }
        else
        {
            intenvoryData.EquipItem(m_EquipmentUIData.serialNumber, m_EquipmentUIData.itemID);
        }

        intenvoryData.SaveData();

        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        if(inventoryUI != null)
        {
            if (m_EquipmentUIData.isEquipped)
            {
                inventoryUI.OnUnequipItem(m_EquipmentUIData.itemID);
            }
            else
            {
                inventoryUI.OnEquipItem(m_EquipmentUIData.itemID);
            }
        }

        CloseUI();
    }
}
