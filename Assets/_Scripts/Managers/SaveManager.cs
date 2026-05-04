using Items;
using Leon.Singleton;
using System;
using System.IO;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public class SaveManager : Singleton<SaveManager>
{
    public SaveSetup saveSetup;
    private string _path = Application.streamingAssetsPath + "/save.txt";

    public Action<SaveSetup> FileLoaded;

    protected override void Awake()
    {
        base.Awake();
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(Load), .1f);
    }

    #region LOAD
    private void Load()
    {
        string fileLoaded = "";
        if (File.Exists(_path))
        {
            fileLoaded = File.ReadAllText(_path);
            saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);
        }
        else
        {
            CreateNewSave();
            Save();
        }

        FileLoaded?.Invoke(saveSetup);
    }

    private void CreateNewSave()
    {
        saveSetup = new();
        saveSetup.health = 15;
        saveSetup.playerName = "Leo";
    }
    #endregion

    #region SAVE
    [NaughtyAttributes.Button]
    private void Save()
    {
        if (!File.Exists(_path))
        {
            CreateNewSave();
        }

        string setupToJson = JsonUtility.ToJson(saveSetup, true);
        SaveFile(setupToJson);
    }



    public void SaveCheckpoint(int checkpoint, int hp)
    {
        saveSetup.lastCheckpoint = checkpoint;
        SaveItems();
        SaveHealth(hp);
        Save();
    }

    public void SaveItems()
    {
        saveSetup.coins = ItemManager.Instance.GetItemByType(ItemType.COIN).soInt.Value;
        saveSetup.lifePacks = ItemManager.Instance.GetItemByType(ItemType.LIFE_PACK).soInt.Value;
        Save();
    }

    public void SaveHealth(int health)
    {
        saveSetup.health = health;
        Save();
    }

    public void SaveName(string name)
    {
        saveSetup.playerName = name;
        Save();
    }
    #endregion


    private void SaveFile(string json)
    {
        File.WriteAllText(_path, json);
    }
}

[System.Serializable]
public class SaveSetup
{
    public int lastCheckpoint;
    public int coins;
    public int health;
    public int lifePacks;

    public string playerName;
}
