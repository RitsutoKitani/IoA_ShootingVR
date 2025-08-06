using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const int RankLen = 20;
    public int[] Score = new int[RankLen];
    public string[] Name = new string[RankLen];
}
