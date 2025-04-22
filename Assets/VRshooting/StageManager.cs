using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    [Header("ステージが稼働中か")] private bool _StageActive = false;
    public bool StageActive { get => _StageActive; }

    [SerializeField]
    [Header("ステージ進行時間")] private float _StageTimer = 0f;

    [SerializeField]
    [Header("敵活動リスト")] private List<EnemyInfo> _Enemys = new List<EnemyInfo>();

    [Space(30)]
    [Header("活動中の敵")] private List<Enemy> _ActEnemys = new List<Enemy>();

    private void Start()
    {
        GM.instance.StageManager = this;

        StageStart(); //スタートテスト
    }

    private void Update()
    {
        if (!_StageActive || GM.instance.IsPose) return;

        StageUpdate();
    }

    private void StageUpdate()
    {
        _StageTimer += Time.deltaTime;

        foreach (EnemyInfo enemy in _Enemys) //敵活動開始！
        {
            if (_StageTimer < enemy.ActiveTime || !enemy.EneCs) continue;
            if (enemy.EneCs.gameObject.activeSelf || enemy.EneCs.Hp <= 0) continue;

            enemy.EneCs.gameObject.SetActive(true);
            _ActEnemys.Add(enemy.EneCs);
        }

        for (int i = 0; i < _ActEnemys.Count; i++) //やられた敵はリストから除外
        {
            if (!_ActEnemys[i] || _ActEnemys[i].Hp <= 0) _ActEnemys.RemoveAt(i);
        }
    }

    public void StageStart()
    {
        _StageTimer = 0f;
        _StageActive = true;
        foreach (EnemyInfo enemy in _Enemys)
        {
            enemy.EneCs.ReSetting();
            enemy.EneCs.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]

public class EnemyInfo
{
    public Enemy EneCs;
    [Header("活動開始時間")] public float ActiveTime;
}
