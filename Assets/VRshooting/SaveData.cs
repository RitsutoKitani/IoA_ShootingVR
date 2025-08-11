using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const int RankLen = 20;
    public ScoreData[] ScoreData = new ScoreData[RankLen];

    public SaveData()
    {
        ScoreData = new ScoreData[RankLen];
    }
}

[System.Serializable]
public class ScoreData
{
    public int Score;
    public string Name;
    public string WeaponA;
    public string WeaponB;

    public ScoreData(int score = 0, string name = "", string weaponA = "", string weaponB = "")
    {
        Score = score;
        Name = name;
        WeaponA = weaponA;
        WeaponB = weaponB;
    }
}
