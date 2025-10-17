using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [SerializeField] private SaveData _MainData;
    public SaveData MainData { get => _MainData; }
    private string _filepath;
    public string filepath { get => _filepath; }
    private string _fileName = "VrShootingData.json";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _filepath = Application.persistentDataPath + "/" + _fileName;

        if (!File.Exists(_filepath))
        {
            Save(_MainData); //ファイルがないときファイル作成
        }

        Load(false);
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_filepath, json);
        Debug.Log(Application.persistentDataPath);

        /*
        StreamWriter wr = new StreamWriter(_filepath, false);
        wr.WriteLine(json);
        wr.Close();
        */
    }

    public void Load(bool Sort = true)
    {
        StreamReader rd = new StreamReader(filepath); //読み込み先を指定
        string json = rd.ReadToEnd(); //全て読み込む
        rd.Close();
        _MainData = JsonUtility.FromJson<SaveData>(json); //メインデータに格納
        if (Sort) DataSort();
    }

    /// <summary>
    /// データをスコア降順にソートします
    /// </summary>
    public void DataSort()
    {
        ScoreData[] data = _MainData.ScoreData;
        System.Array.Sort(data, (a, b) => b.Score.CompareTo(a.Score));
        _MainData.ScoreData = data;
    }

    public void AddData(ScoreData data, bool save = true)
    {
        DataSort();
        if (data.Score > _MainData.ScoreData[_MainData.ScoreData.Length - 1].Score) //ランキング一番下のスコアより高かったら
        {
            _MainData.ScoreData[_MainData.ScoreData.Length - 1] = data;
            DataSort();
        }
        if (save) Save(_MainData);
        Debug.Log("データ追加完了");
    }

    public void DataReset()
    {
        for(int i = 0;i < _MainData.ScoreData.Length;i++) _MainData.ScoreData[i] = new ScoreData();
        Save(_MainData);
    }
}
