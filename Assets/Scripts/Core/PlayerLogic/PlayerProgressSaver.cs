using System.Collections.Generic;
using UnityEngine;

public static class PlayerProgressSaver
{
    private const string KeyCoins = "PP_Coins";
    private const string KeyStage = "PP_Stage";
    private const string KeyLevel = "PP_Level";
    private const string KeyItems = "PP_Items";
    private const string KeyWeapons = "PP_Weapons";

    // Save PlayerProgress
    public static void Save(PlayerProgress progress)
    {
        PlayerPrefs.SetInt(KeyCoins, progress.coins);
        PlayerPrefs.SetInt(KeyStage, progress.savedStageIndex);
        PlayerPrefs.SetInt(KeyLevel, progress.savedLevelIndex);

        PlayerPrefs.SetString(KeyItems, JsonUtility.ToJson(new ItemListWrapper(progress.items)));
        PlayerPrefs.SetString(KeyWeapons, JsonUtility.ToJson(new StringListWrapper(progress.weapons)));

        PlayerPrefs.Save();
    }

    // Load PlayerProgress
    public static void Load(PlayerProgress progress, ItemDatabase itemDatabase)
    {
        progress.coins = PlayerPrefs.GetInt(KeyCoins, progress.coins);
        progress.savedStageIndex = PlayerPrefs.GetInt(KeyStage, progress.savedStageIndex);
        progress.savedLevelIndex = PlayerPrefs.GetInt(KeyLevel, progress.savedLevelIndex);

        string itemsJson = PlayerPrefs.GetString(KeyItems, "");
        if (!string.IsNullOrEmpty(itemsJson))
        {
            progress.items = ItemListWrapper.FromJson(itemsJson, itemDatabase);
        }

        string weaponsJson = PlayerPrefs.GetString(KeyWeapons, "");
        if (!string.IsNullOrEmpty(weaponsJson))
        {
            progress.weapons = StringListWrapper.FromJson(weaponsJson);
        }
    }

    [System.Serializable]
    private class ItemListWrapper
    {
        public List<string> itemNames = new();

        public ItemListWrapper(List<ItemData> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                    itemNames.Add(item.itemName);
            }
        }

        public static List<ItemData> FromJson(string json, ItemDatabase database)
        {
            var wrapper = JsonUtility.FromJson<ItemListWrapper>(json);
            var result = new List<ItemData>();

            foreach (var name in wrapper.itemNames)
            {
                var item = database.GetItemByName(name);
                if (item != null)
                    result.Add(item);
            }

            return result;
        }
    }

    [System.Serializable]
    private class StringListWrapper
    {
        public List<string> list = new();

        public StringListWrapper(List<string> input)
        {
            list = new List<string>(input);
        }

        public static List<string> FromJson(string json)
        {
            return JsonUtility.FromJson<StringListWrapper>(json).list;
        }
    }
}