using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScore : MonoBehaviour
{
    [SerializeField] private Text _RankText;
    [SerializeField] private Text _NameText;
    [SerializeField] private Text _ScoreText;

    [SerializeField] private Image _WeaponImageA;
    [SerializeField] private Image _WeaponImageB;

    [Space(30)] [SerializeField] private Sprite _GunDefaultIcon;

    public void Initialize(int rank)
    {
        if (rank < 0 || rank >= SaveData.RankLen) return;
        ScoreData data = DataManager.instance.MainData.ScoreData[rank];

        if (_RankText) _RankText.text = (rank + 1).ToString();
        if (_NameText) _NameText.text = data.Name != "" ? data.Name : "-----";
        if (_ScoreText) _ScoreText.text = data.Score.ToString("00000000");
        if (_WeaponImageA) _WeaponImageA.sprite = GM.instance.GunSearchByName(data.WeaponA) ? GM.instance.GunSearchByName(data.WeaponA).Icon : _GunDefaultIcon;
        if (_WeaponImageB) _WeaponImageB.sprite = GM.instance.GunSearchByName(data.WeaponB) ? GM.instance.GunSearchByName(data.WeaponB).Icon : _GunDefaultIcon;
    }
}
