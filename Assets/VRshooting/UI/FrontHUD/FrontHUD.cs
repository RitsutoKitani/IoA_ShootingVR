using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FrontHUD : MonoBehaviour
{
    [SerializeField][Header("回転速度")] private float _TurnTorque;
    [SerializeField] private Toggle _TurnTestToggle;

    [Space(30)]
    [Header("HP表示UI")][SerializeField] private Image _HpGage;
    [Header("HPバーの色")][SerializeField] private Color32[] _HpGageColor;
    [Header("HPバーの色（ブレイク状態）")][SerializeField] private Color32 _HpGageBreakColor;

    [Space(30)]

    [Header("残弾表示UI")][SerializeField] private Text _AmmoText;

    [Space(30)]

    [Header("スコアランク表示UI")][SerializeField] private Text _RankText;
    [Header("スコア表示UI")][SerializeField] private Text _ScoreText;

    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    void Update()
    {
        if(!StageManager.instance) return;

        if (_ani) _ani.SetBool("Hide", StageManager.instance.finish);

        _CanvasUpdate();

        _HpUpdate();
        _ScoreUpdate();
        _GunUpdate();
    }

    private void _CanvasUpdate()
    {
        Vector3 LookPos = Camera.main.transform.position + Camera.main.transform.forward * 8f;

        if (!_TurnTestToggle.isOn) LookPos = transform.position + Vector3.forward * 8f;


        var diff = new Vector3(LookPos.x, transform.position.y, LookPos.z) - transform.position;
        var rot = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, _TurnTorque);
    }

    /// <summary>
    /// 体力UIアップデート
    /// </summary>
    private void _HpUpdate()
    {
        if(!StageManager.instance.DomeCs || !_HpGage) return;
        PlayerDome Dome = StageManager.instance.DomeCs;
        Color32 color = _HpGageColor[0];

        if (!Dome.Breaking)
        {
            _HpGage.fillAmount = (float)Dome.Hp / Dome.HpMax;
            for (int i = 1; i < _HpGageColor.Length; i++)
            {
                if (Dome.Hp < Dome.HpMax - Dome.HpMax * i / _HpGageColor.Length) color = _HpGageColor[i];
            }
        }
        else
        {
            _HpGage.fillAmount = Dome.BreakTimer / Dome.BreakHealTime;
            color = _HpGageBreakColor;
        }
        _HpGage.color = color;
    }

    /// <summary>
    /// 弾数UIアップデート
    /// </summary>
    private void _GunUpdate()
    {
        if (!_AmmoText) return;

        if(GM.instance.SetMainGunsCs.Count <= 0) return;
        MainGun GunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];

        _AmmoText.text = $"{GunCs.MagazineBullet:D2}";
    }

    /// <summary>
    /// スコアUIアップデート
    /// </summary>
    private void _ScoreUpdate()
    {
        if(!_RankText ||  !_ScoreText) return;

        int score = StageManager.instance.Score;

        _ScoreText.text = $"{score:D6}";

        _RankText.text = "C";
        if (StageManager.instance.LankScore[0] <= score) _RankText.text = "B";
        if (StageManager.instance.LankScore[1] <= score) _RankText.text = "A";
        if (StageManager.instance.LankScore[2] <= score) _RankText.text = "S";
    }
}
