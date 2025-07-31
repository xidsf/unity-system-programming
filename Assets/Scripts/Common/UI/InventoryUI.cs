using Gpm.Ui;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public enum InventorySortType
{
    ItemGrade,
    ItemType,
}


public class InventoryUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;
    public TextMeshProUGUI SortButtonText;
    private InventorySortType m_CurrentInventorySortType = InventorySortType.ItemGrade;

    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);
        SetInventory();
        SortInventory();
    }
    private void SetInventory()
    {
        infiniteScrollList.Clear();

        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData != null)
        {
            foreach (var item in userInventoryData.inventoryItemDataList)
            {
                InventoryItemSlotData newItemData = new InventoryItemSlotData
                {
                    serialNumber = item.serialNumber.ToString(),
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
}
