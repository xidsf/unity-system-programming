using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTables";

    private const string CHAPTER_DATA_TABLE_PATH = "ChapterDataTable";
    private List <ChapterData> ChapterDataList = new List<ChapterData>();

    protected override void Init()
    {
        base.Init();
        LoadChapterDataTable();
    }

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
}


public class ChapterData
{
    public int chapterNo;
    public int totalStage;
    public int chapterRewardGem;
    public int ChapterRewardGold;
}