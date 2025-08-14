using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTables";

    protected override void Init()
    {
        base.Init();
        LoadChapterDataTable();
        LoadItemData();
        LoadAchievementDataTable();
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
                chapterName = data["chapter_name"].ToString(),
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

    public ItemData GetItemData(int itemID)
    {
        return itemDataTable.Where(item => item.itemID == itemID).FirstOrDefault();
    }

    #endregion

    #region ACHIEVEMENT_DATA
    private const string ACHIEVEMENT_DATA_TABLE = "AchievementDataTable";
    private List<AchievementData> achivementDataTable = new();
    public List<AchievementData> GetAchievementDataTable()
    {
        return achivementDataTable;
    }

    private void LoadAchievementDataTable()
    {
        var parseDataTable = CSVReader.Read($"{DATA_PATH}/{ACHIEVEMENT_DATA_TABLE}");

        foreach (var data in parseDataTable)
        {
            var achievementData = new AchievementData
            {
                achievementType = (AchievementType)Enum.Parse(typeof(AchievementType), data["achievement_type"].ToString()),
                achievementName = data["achievement_name"].ToString(),
                achievementGoal = Convert.ToInt32(data["achievement_goal"]),
                AchievementRewardType = (GlobalDefine.RewardType)(Enum.Parse(typeof(RewardType), data["achievement_reward_type"].ToString())),
                AchievementRewardAmount = Convert.ToInt32(data["achievement_reward_amount"])
            };

            achivementDataTable.Add(achievementData);
            Logger.Log($"Add {achievementData.achievementName} achievement");
        }
    }

    public AchievementData GetAchievementData(AchievementType achievementType)
    {
        return achivementDataTable.Where(item => item.achievementType == achievementType).FirstOrDefault();
    }

    #endregion
}


public class ChapterData
{
    public int chapterNo;
    public string chapterName;
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