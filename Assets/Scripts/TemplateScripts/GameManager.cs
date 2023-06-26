using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    //managerde bulunacak

    public enum GameStat
    {
        intro = 0,
        start = 1,
        finish = 2
    }


    [Header("Game_Main_Field")]
    [Space(10)]

    public GameStat gameStat;
    public int level;
    public int money;
    public int power;
    public int sound;

    public void Start()
    {
        PlayerPrefsPlacement();
        ItemData.Instance.AwakeID();
    }

    public void GameStatStart()
    {
        gameStat = GameStat.start;
    }

    //oyun baþý ana deðiþkenleri playerpreften getirip iþler
    private void PlayerPrefsPlacement()
    {
        if (PlayerPrefs.HasKey("money"))
            money = PlayerPrefs.GetInt("money");
        else
            PlayerPrefs.SetInt("money", money);
        MoneySystem.Instance.MoneyTextRevork(0);

        if (PlayerPrefs.HasKey("power"))
            power = PlayerPrefs.GetInt("power");
        else
            PlayerPrefs.SetInt("power", power);
        MoneySystem.Instance.PowerTextRevork(0);

        if (PlayerPrefs.HasKey("level"))
            level = PlayerPrefs.GetInt("level");
        else
            PlayerPrefs.SetInt("level", level);

        if (PlayerPrefs.HasKey("sound"))
            sound = PlayerPrefs.GetInt("sound");
        else
            PlayerPrefs.SetInt("sound", sound);

        if (PlayerPrefs.HasKey("first"))
        {
            ItemData.Instance.factor = FactorPlacementRead();
            GridSystem.Instance.mainGrid = GridPlacementRead();
        }
        else
        {
            PlayerPrefs.SetInt("first", 1);
            FactorPlacementWrite(ItemData.Instance.factor);
            GridPlacementWrite(GridSystem.Instance.mainGrid);
        }
    }

    //grid ve ýtemdatadaki factorleri kaydedip oyun baþý iþler
    public void FactorPlacementWrite(ItemData.Field factor)
    {
        string jsonData = JsonUtility.ToJson(factor);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/FactorData.json", jsonData);
    }
    public void GridPlacementWrite(GridSystem.MainGrid grid)
    {
        string jsonData = JsonUtility.ToJson(grid);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GridData.json", jsonData);
    }
    public ItemData.Field FactorPlacementRead()
    {
        string jsonRead = System.IO.File.ReadAllText(Application.persistentDataPath + "/FactorData.json");
        ItemData.Field factor = new ItemData.Field();
        factor = JsonUtility.FromJson<ItemData.Field>(jsonRead);
        return factor;
    }
    public GridSystem.MainGrid GridPlacementRead()
    {
        string jsonRead = System.IO.File.ReadAllText(Application.persistentDataPath + "/GridData.json");
        GridSystem.MainGrid grid = new GridSystem.MainGrid();
        grid = JsonUtility.FromJson<GridSystem.MainGrid>(jsonRead);
        return grid;
    }

    //oyunun ana deðiþkelerini kaydediyor
    public void SetMoney()
    {
        PlayerPrefs.SetInt("money", money);
    }
    public void SetPower()
    {
        PlayerPrefs.SetInt("power", power);
    }
    public void SetSound()
    {
        PlayerPrefs.SetInt("sound", sound);
    }
    public void SetLevel()
    {
        level++;
        PlayerPrefs.SetInt("level", level);
    }
}
