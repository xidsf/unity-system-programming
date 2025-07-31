using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTables";


    protected override void Init()
    {
        base.Init();
        LoadChapterDataTable();
        LoadItemData();
    }

    #region CHAPTER_DATA

    private const string CHAPTER_DATA_TABLE_PATH = "ChapterDataTable";
    private List<ChapterData> ChapterDataList = new List<ChapterData>();


    private void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE_PATH}");

        foreach(var data in parsedDataTable)
        {
            var chapterDataTable = new ChapterData
            {
                chapterNo = Convert.ToInt32(data["chapter_no"]),
                totalStage = Convert.ToInt32(data["total_stages"]),
                chapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"])
            };

            ChapterDataList.Add(chapterDataTable);
        }
    }

    public ChapterData GetChapterData(int chapterNo)
    {
        return ChapterDataList.Where(item => item.chapterNo == chapterNo).FirstOrDefault();
    }
    #endregion

    #region ITEM_DATA

    private const string ITEM_DATA_TABLE = "ItemDataTable";
    private List<ItemData> itemDataTable = new List<ItemData>();

    private void LoadItemData()
    {
        var parseDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        foreach (var item in parseDataTable)
        {
            var itemData = new ItemData
            {
                itemID = Convert.ToInt32(item["item_id"]),
                itemName = item["item_name"].ToString(),
                attackPower = Convert.ToInt32(item["attack_power"]),
                defense = Convert.ToInt32(item["defense"]),
            };
            itemDataTable.Add(itemData);
        }
    }

    public ItemData FindItemData(int itemID)
    {
        return itemDataTable.Where(item => item.itemID == itemID).FirstOrDefault();
    }

    #endregion
}


public class ChapterData
{
    public int chapterNo;
    public int totalStage;
    public int chapterRewardGem;
    public int ChapterRewardGold;
}

public class ItemData
{
    public int itemID;
    public string itemName;
    public int attackPower;
    public int defense;
}

public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory
}

public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary
}