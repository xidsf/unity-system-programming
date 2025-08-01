using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    public Image addIcon;
    public Image equippedItemIcon;
    public Image equippedItemGradeBackground;

    private UserItemData m_EquipedItemData;

    public void SetItem(UserItemData equipment)
    {
        m_EquipedItemData = equipment;

        addIcon.gameObject.SetActive(false);
        equippedItemIcon.gameObject.SetActive(true);
        equippedItemGradeBackground.gameObject.SetActive(true);

        ItemGrade itemGrade = (ItemGrade)((m_EquipedItemData.itemID / 1000) % 10);
        var backgroundTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        if(backgroundTexture != null)
        {
            equippedItemGradeBackground.sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(1f, 1f));
        }

        StringBuilder sb = new StringBuilder(m_EquipedItemData.itemID.ToString());
        sb[1] = '1';
        var itemName = sb.ToString();
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemName}");
        if(itemIconTexture != null)
        {
            equippedItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    public void ClearItem()
    {
        m_EquipedItemData = null;

        addIcon.gameObject.SetActive(true);
        equippedItemIcon.gameObject.SetActive(false);
        equippedItemGradeBackground.gameObject.SetActive(false);
    }

    public void OnClickEquippedItemIcon()
    {
        var uiData = new EquipmentUIData();
        uiData.itemID = m_EquipedItemData.itemID;
        uiData.serialNumber = m_EquipedItemData.serialNumber;
        uiData.isEquipped = true;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
