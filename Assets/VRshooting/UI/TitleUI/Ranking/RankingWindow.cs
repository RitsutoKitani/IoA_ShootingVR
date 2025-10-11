using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingWindow : MonoBehaviour
{
    [Header("ランキングUIプレハブ")][SerializeField] private GameObject _RankingUI;
    [SerializeField] private Transform _RankingContent;
    [SerializeField] private ScrollRect _SR;

    [SerializeField] private Text _SaveFolderText;

    IEnumerator Start()
    {
        _ScrollCreate();
        if (_SaveFolderText) _SaveFolderText.text = DataManager.instance.filepath;

        yield return null; // 1フレーム待つ
        if(_SR) _SR.verticalNormalizedPosition = 1f;
    }

    private void _ScrollCreate()
    {
        if (!_RankingUI || !_RankingContent) return;

        for(int i = 0; i < SaveData.RankLen; i++)
        {
            GameObject UI = Instantiate(_RankingUI, _RankingContent);
            UI.GetComponent<RankingScore>().Initialize(i);
        }
    }
}
