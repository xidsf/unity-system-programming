using System.Collections.Generic;
using System;
using UnityEngine;
using System.Data;
using Mono.Cecil;

[Serializable]
public class UserItemData
{
    public long serialNumber;
    public int itemID;

    public UserItemData(long serialNum, int itemId)
    {
        serialNumber = serialNum;
        itemID = itemId;
    }
}

//유저 정보를 저장/로드하기 위한 래퍼클래스
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> inventoryItemDataList;
}

public class UserItemStats
{
    public int attackPower;
    public int defense;

    public UserItemStats(int attackPower, int defense)
    {
        this.attackPower = attackPower;
        this.defense = defense;
    }
}

public class UserInventoryData : IUserData
{
    public UserItemData equipmentWeaponData { get; set; }
    public UserItemData equipmentShieldData { get; set; }
    public UserItemData equipmentChestArmorData { get; set; }
    public UserItemData equipmentBootsData { get; set; }
    public UserItemData equipmentGlovesData { get; set; }
    public UserItemData equipmentAccessoryData { get; set; }

    public List<UserItemData> inventoryItemDataList { get; set; } = new List<UserItemData>();

    public Dictionary<long, UserItemStats> equippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //기본적으로 시리얼넘버는 중복되지 않도록 해야함
        //여기 강의에서는 현재 시간+무작위 수 를 더해 시리얼 넘버를 제작함
        //나중에 만들때도 원하는 방법으로 만들면 됨

        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22002));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33002));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44002));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55002));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65001));
        inventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65002));

        equipmentWeaponData = new UserItemData(inventoryItemDataList[0].serialNumber, inventoryItemDataList[0].itemID);
        equipmentShieldData = new UserItemData(inventoryItemDataList[2].serialNumber, inventoryItemDataList[2].itemID);

        SetEquippedItemDictionary();
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData()");

        bool result = false;

        try
        {
            string weaponJson = PlayerPrefs.GetString("equipmentWeaponData");
            if(!string.IsNullOrEmpty(weaponJson))
            {
                equipmentWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"equipmentWeaponData: SerialNum: {equipmentWeaponData.serialNumber}  itemID: {equipmentWeaponData.itemID}");
            }

            string shieldJson = PlayerPrefs.GetString("equipmentShieldData");
            if (!string.IsNullOrEmpty(shieldJson))
            {
                equipmentShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"equipmentShieldData: SerialNum: {equipmentShieldData.serialNumber}  itemID: {equipmentShieldData.itemID}");
            }

            string chestArmorJson = PlayerPrefs.GetString("equipmentChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                equipmentChestArmorData = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"equipmentChestArmorData: SerialNum: {equipmentChestArmorData.serialNumber}  itemID: {equipmentChestArmorData.itemID}");
            }

            string bootsJson = PlayerPrefs.GetString("equipmentBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                equipmentBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"equipmentBootsData: SerialNum: {equipmentBootsData.serialNumber}  itemID: {equipmentBootsData.itemID}");
            }

            string glovesJson = PlayerPrefs.GetString("equipmentGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                equipmentGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"equipmentGlovesData: SerialNum: {equipmentGlovesData.serialNumber}  itemID: {equipmentGlovesData.itemID}");
            }

            string accessaryJson = PlayerPrefs.GetString("equipmentAccessaryData");
            if (!string.IsNullOrEmpty(accessaryJson))
            {
                equipmentAccessoryData = JsonUtility.FromJson<UserItemData>(accessaryJson);
                Logger.Log($"equipmentAccessaryData: SerialNum: {equipmentAccessoryData.serialNumber}  itemID: {equipmentAccessoryData.itemID}");
            }


            string inventoryItemListJson = PlayerPrefs.GetString("inventoryItemDataList");
            if(!string.IsNullOrEmpty(inventoryItemListJson))
            {
                UserInventoryItemDataListWrapper wrapperedItemDataList = JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemListJson);
                inventoryItemDataList = wrapperedItemDataList.inventoryItemDataList;

                Logger.Log("inventoryItemDataList");
                foreach (var item in inventoryItemDataList)
                {
                    Logger.Log($"Load: {item.serialNumber} :: {item.itemID}");
                }
            }

            SetEquippedItemDictionary();
            result = true;
        }
        catch(System.Exception e)
        {
            Logger.LogError($"Load Faild : {e.Message}");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData()");

        bool result = false;

        try
        {
            string weaponJson = JsonUtility.ToJson(equipmentWeaponData);
            PlayerPrefs.SetString("equipmentWeaponData", weaponJson);
            if(!string.IsNullOrEmpty(weaponJson))
            {
                Logger.Log($"equipmentWeaponData: SerialNum: {equipmentWeaponData.serialNumber}  itemID: {equipmentWeaponData.itemID}");
            }

            string shieldJson = JsonUtility.ToJson(equipmentShieldData);
            PlayerPrefs.SetString("equipmentShieldData", shieldJson);
            if (!string.IsNullOrEmpty(shieldJson))
            {
                Logger.Log($"equipmentShieldData: SerialNum: {equipmentShieldData.serialNumber}  itemID: {equipmentShieldData.itemID}");
            }

            string chestArmorJson = JsonUtility.ToJson(equipmentChestArmorData);
            PlayerPrefs.SetString("equipmentChestArmorData", chestArmorJson);
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                Logger.Log($"equipmentChestArmorData: SerialNum: {equipmentChestArmorData.serialNumber}  itemID: {equipmentChestArmorData.itemID}");
            }

            string bootsJson = JsonUtility.ToJson(equipmentBootsData);
            PlayerPrefs.SetString("equipmentBootsData", bootsJson);
            if (!string.IsNullOrEmpty(bootsJson))
            {
                Logger.Log($"equipmentBootsData: SerialNum: {equipmentBootsData.serialNumber}  itemID: {equipmentBootsData.itemID}");
            }

            string glovesJson = JsonUtility.ToJson(equipmentGlovesData);
            PlayerPrefs.SetString("equipmentGlovesData", glovesJson);
            if (!string.IsNullOrEmpty(glovesJson))
            {
                Logger.Log($"equipmentGlovesData: SerialNum: {equipmentGlovesData.serialNumber}  itemID: {equipmentGlovesData.itemID}");
            }

            string accessaryJson = JsonUtility.ToJson(equipmentAccessoryData);
            PlayerPrefs.SetString("equipmentAccessaryData", accessaryJson);
            if (!string.IsNullOrEmpty(accessaryJson))
            {
                Logger.Log($"equipmentAccessaryData: SerialNum: {equipmentAccessoryData.serialNumber}  itemID: {equipmentAccessoryData.itemID}");
            }


            UserInventoryItemDataListWrapper wrapperItemData = new UserInventoryItemDataListWrapper();
            wrapperItemData.inventoryItemDataList = inventoryItemDataList;
            string jsonStringItemData = JsonUtility.ToJson(wrapperItemData);
            PlayerPrefs.SetString("inventoryItemDataList", jsonStringItemData);
            PlayerPrefs.Save();

            Logger.Log("inventoryItemDataList");
            foreach (var item in inventoryItemDataList)
            {
                Logger.Log($"Saved: SerialNum:{item.serialNumber}  itemID:{item.itemID}");
            }

            result = true;
        }
        catch (Exception e)
        {
            Logger.LogError($"Save Faild : {e.Message}");
        }
        return result;
    }

    public void SetEquippedItemDictionary()
    {
        if(equipmentWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentWeaponData.itemID);
            if(itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentWeaponData.serialNumber, itemStatsData);
            }
            
        }
        if(equipmentShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentShieldData.itemID);
            if (itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentShieldData.serialNumber, itemStatsData);
            }
                
        }
        if (equipmentGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentGlovesData.itemID);
            if (itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentGlovesData.serialNumber, itemStatsData);
            }
                
        }
        if (equipmentChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentChestArmorData.itemID);
            if (itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentChestArmorData.serialNumber, itemStatsData);
            }
               
        }
        if (equipmentBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentBootsData.itemID);
            if (itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentBootsData.serialNumber, itemStatsData);
            }
                
        }
        if (equipmentAccessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(equipmentAccessoryData.itemID);
            if (itemData != null)
            {
                var itemStatsData = new UserItemStats(itemData.attackPower, itemData.defense);
                equippedItemDic.Add(equipmentAccessoryData.serialNumber, itemStatsData);
            }
        }
    }

    public bool IsEquipped(long serialNum)
    {
        return equippedItemDic.ContainsKey(serialNum);
    }

    public void EquipItem(long serialNum, int itemID)
    {
        var equippedItemData = DataTableManager.Instance.GetItemData(itemID);
        if(equippedItemData == null)
        {
            Logger.LogError($"item data does not exist. itemID:{itemID}");
            return;
        }

        var itemType = (ItemType)(itemID / 10000);

        switch(itemType)
        {
            case ItemType.Weapon:
                if(equipmentWeaponData != null)
                {
                    equippedItemDic.Remove(equipmentWeaponData.serialNumber);
                    equipmentWeaponData = null;
                }
                equipmentWeaponData = new UserItemData(serialNum, itemID);
                break;
            case ItemType.Shield:
                if (equipmentShieldData != null)
                {
                    equippedItemDic.Remove(equipmentShieldData.serialNumber);
                    equipmentShieldData = null;
                }
                equipmentShieldData = new UserItemData(serialNum, itemID);
                break;
            case ItemType.Gloves:
                if (equipmentGlovesData != null)
                {
                    equippedItemDic.Remove(equipmentGlovesData.serialNumber);
                    equipmentGlovesData = null;
                }
                equipmentGlovesData = new UserItemData(serialNum, itemID);
                break;
            case ItemType.ChestArmor:
                if (equipmentChestArmorData != null)
                {
                    equippedItemDic.Remove(equipmentChestArmorData.serialNumber);
                    equipmentChestArmorData = null;
                }
                equipmentChestArmorData = new UserItemData(serialNum, itemID);
                break;
            case ItemType.Boots:
                if (equipmentBootsData != null)
                {
                    equippedItemDic.Remove(equipmentBootsData.serialNumber);
                    equipmentBootsData = null;
                }
                equipmentBootsData = new UserItemData(serialNum, itemID);
                break;
            case ItemType.Accessory:
                if (equipmentAccessoryData != null)
                {
                    equippedItemDic.Remove(equipmentAccessoryData.serialNumber);
                    equipmentAccessoryData = null;
                }
                equipmentAccessoryData = new UserItemData(serialNum, itemID);
                break;
        }
        equippedItemDic.Add(serialNum, new UserItemStats(equippedItemData.attackPower, equippedItemData.defense));
    }

    public void UnequipItem(long serialNum, int itemID)
    {
        var itemType = (ItemType)(itemID / 10000);

        switch (itemType)
        {
            case ItemType.Weapon:
                equipmentWeaponData = null;
                break;
            case ItemType.Shield:
                equipmentShieldData = null;
                break;
            case ItemType.Gloves:
                equipmentGlovesData = null;
                break;
            case ItemType.ChestArmor:
                equipmentChestArmorData = null;
                break;
            case ItemType.Boots:
                equipmentBootsData = null;
                break;
            case ItemType.Accessory:
                equipmentAccessoryData = null;
                break;
        }
        equippedItemDic.Remove(serialNum);
    }

    public UserItemStats GetUserTotalItemStats()
    {
        int totalItemAttackPower = 0;
        int totalItemDefense = 0;

        foreach (var item in equippedItemDic)
        {
            totalItemAttackPower += item.Value.attackPower;
            totalItemDefense += item.Value.defense;
        }
        return new UserItemStats(totalItemAttackPower, totalItemDefense);
    }
}
