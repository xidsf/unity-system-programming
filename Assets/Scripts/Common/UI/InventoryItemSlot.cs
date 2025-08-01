using Gpm.Ui;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlotData : InfiniteScrollData
{
    public long serialNumber;
    public int itemID;
}


public class InventoryItemSlot : InfiniteScrollItem
{
    public Image itemGradeBackground;
    public Image itemIcon;

    private InventoryItemSlotData m_InventorySlotData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        m_InventorySlotData = scrollData as InventoryItemSlotData;

        if(m_InventorySlotData == null)
        {
            Logger.LogError($"{GetType()}:: m_InventorySlotData is null");
            return;
        }

        var itemGrade = (ItemGrade)((m_InventorySlotData.itemID / 1000) % 10);
        var gradeBackgroundTexture = Resources.Load<Texture2D>($"Textures/{itemGrade.ToString()}");
        if(gradeBackgroundTexture != null)
        {
            itemGradeBackground.sprite = Sprite.Create(gradeBackgroundTexture, new Rect(0, 0, gradeBackgroundTexture.width, gradeBackgroundTexture.height), new Vector2(1f, 1f));
        }

        StringBuilder sb = new StringBuilder(m_InventorySlotData.itemID.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName.ToString()}");
        if(itemIconTexture != null)
        {
            itemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    public void OnClickItemSlot()
    {
        var equipementUIData = new EquipmentUIData();
        equipementUIData.itemID = m_InventorySlotData.itemID;
        equipementUIData.serialNumber = m_InventorySlotData.serialNumber;
        equipementUIData.isEquipped = false;
        UIManager.Instance.OpenUI<EquipmentUI>(equipementUIData);
    }
}
