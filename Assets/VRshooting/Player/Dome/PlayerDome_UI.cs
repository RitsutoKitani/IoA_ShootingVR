using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDome_UI : MonoBehaviour
{
    private PlayerDome _DomeCs;

    [SerializeField] private Image _HpGage;
    [SerializeField] private Text _HpText;

    private void Start()
    {
        if (StageManager.instance.DomeCs) _DomeCs = StageManager.instance.DomeCs;
        else
        {
            Debug.LogWarning("プレイヤードームのスクリプトが設定されていません");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _HpGage.fillAmount = (float)_DomeCs.Hp / _DomeCs.HpMax;
        //_HpText.text = string.Format("{0} / {1}", _DomeCs.Hp, _DomeCs.HpMax);
    }
}
