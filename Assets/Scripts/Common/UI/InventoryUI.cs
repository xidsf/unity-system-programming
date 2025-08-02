using Gpm.Ui;
using TMPro;

public enum InventorySortType
{
    ItemGrade,
    ItemType,
}


public class InventoryUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;
    public TextMeshProUGUI SortButtonText;

    public TextMeshProUGUI totalAttackPowerText;
    public TextMeshProUGUI totalDefenseText;

    private InventorySortType m_CurrentInventorySortType = InventorySortType.ItemGrade;

    public EquippedItemSlot weaponSlot;
    public EquippedItemSlot shieldSlot;
    public EquippedItemSlot chestArmorSlot;
    public EquippedItemSlot bootsSlot;
    public EquippedItemSlot glovesSlot;
    public EquippedItemSlot accessorySlot;


    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        SetTotalStats();
        SetEquippedItems();
        SetInventory();
        SortInventory();
    }


    private void SetTotalStats()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData is null");
            return;
        }

        var itemTotalStats = userInventoryData.GetUserTotalItemStats();
        totalAttackPowerText.text = '+' + itemTotalStats.attackPower.ToString("N0");
        totalDefenseText.text = '+' + itemTotalStats.defense.ToString("N0");
    }

    private void SetEquippedItems()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("userInventoryData is null");
            return;
        }

        if(userInventoryData.equipmentWeaponData != null)
        {
            weaponSlot.SetItem(userInventoryData.equipmentWeaponData);
        }
        else
        {
            weaponSlot.ClearItem();
        }

        if (userInventoryData.equipmentShieldData != null)
        {
            shieldSlot.SetItem(userInventoryData.equipmentShieldData);
        }
        else
        {
            shieldSlot.ClearItem();
        }

        if (userInventoryData.equipmentGlovesData != null)
        {
            glovesSlot.SetItem(userInventoryData.equipmentGlovesData);
        }
        else
        {
            glovesSlot.ClearItem();
        }

        if (userInventoryData.equipmentChestArmorData != null)
        {
            chestArmorSlot.SetItem(userInventoryData.equipmentChestArmorData);
        }
        else
        {
            chestArmorSlot.ClearItem();
        }

        if (userInventoryData.equipmentBootsData != null)
        {
            bootsSlot.SetItem(userInventoryData.equipmentBootsData);
        }
        else
        {
            bootsSlot.ClearItem();
        }

        if (userInventoryData.equipmentAccessoryData != null)
        {
            accessorySlot.SetItem(userInventoryData.equipmentAccessoryData);
        }
        else
        {
            accessorySlot.ClearItem();
        }
    }
    private void SetInventory()
    {
        infiniteScrollList.Clear();

        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData != null)
        {
            foreach (var item in userInventoryData.inventoryItemDataList)
            {
                if(userInventoryData.IsEquipped(item.serialNumber))
                {
                    continue;
                }
                InventoryItemSlotData newItemData = new InventoryItemSlotData
                {
                    serialNumber = item.serialNumber,
                    itemID = item.itemID
                };

                infiniteScrollList.InsertData(newItemData);
            }
        }
    }

    private void SortInventory()
    {
        switch(m_CurrentInventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortButtonText.text = "Grade";
                infiniteScrollList.SortDataList((a, b) =>
                {
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;

                    //아이템 등급은 2번째 자리에 있음
                    int comparisionResult = ((itemB.itemID / 1000) % 10).CompareTo((itemA.itemID / 1000) % 10);

                    //등급이 같으면 종류로 구분
                    if(comparisionResult == 0)
                    {
                        var itemAIDStr = itemA.itemID.ToString();
                        var itemAComp = itemAIDStr.Substring(0, 1) + itemAIDStr.Substring(2, 3);

                        var itemBIDStr = itemB.itemID.ToString();
                        var itemBComp = itemBIDStr.Substring(0, 1) + itemBIDStr.Substring(2, 3);

                        comparisionResult = itemAComp.CompareTo(itemBComp);
                    }

                    return comparisionResult;
                });

                break;
            case InventorySortType.ItemType:
                SortButtonText.text = "Type";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;

                    var itemAIDStr = itemA.itemID.ToString();
                    var itemAComp = itemAIDStr.Substring(0, 1) + itemAIDStr.Substring(2, 3);

                    var itemBIDStr = itemB.itemID.ToString();
                    var itemBComp = itemBIDStr.Substring(0, 1) + itemBIDStr.Substring(2, 3);

                    int comparisionResult = itemAComp.CompareTo(itemBComp);

                    if(comparisionResult == 0)
                    {
                        comparisionResult = ((itemB.itemID / 1000) % 10).CompareTo((itemA.itemID / 1000) % 10);
                    }

                    return comparisionResult;
                });

                break;
            default:
                break;
        }
    }

    public void OnClickSortButton()
    {
        switch(m_CurrentInventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_CurrentInventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_CurrentInventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }
        SortInventory();
    }

    public void OnEquipItem(int itemId)
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if (userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }

        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                weaponSlot.SetItem(userInventoryData.equipmentWeaponData);
                break;
            case ItemType.Shield:
                shieldSlot.SetItem(userInventoryData.equipmentShieldData);
                break;
            case ItemType.ChestArmor:
                chestArmorSlot.SetItem(userInventoryData.equipmentChestArmorData);
                break;
            case ItemType.Gloves:
                glovesSlot.SetItem(userInventoryData.equipmentGlovesData);
                break;
            case ItemType.Boots:
                bootsSlot.SetItem(userInventoryData.equipmentBootsData);
                break;
            case ItemType.Accessory:
                accessorySlot.SetItem(userInventoryData.equipmentAccessoryData);
                break;
            default:
                break;
        }

        SetTotalStats();
        SetInventory();
        SortInventory();
    }

    public void OnUnequipItem(int itemId)
    {
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                weaponSlot.ClearItem();
                break;
            case ItemType.Shield:
                shieldSlot.ClearItem();
                break;
            case ItemType.ChestArmor:
                chestArmorSlot.ClearItem();
                break;
            case ItemType.Gloves:
                glovesSlot.ClearItem();
                break;
            case ItemType.Boots:
                bootsSlot.ClearItem();
                break;
            case ItemType.Accessory:
                accessorySlot.ClearItem();
                break;
            default:
                break;
        }

        SetTotalStats();
        SetInventory();
        SortInventory();
    }
}
