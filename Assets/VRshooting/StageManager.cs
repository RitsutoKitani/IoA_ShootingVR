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
    private List<EnemyInfo> _Enemys = new List<EnemyInfo>();

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

        foreach (EnemyInfo enemy in _Enemys)
        {
            if (_StageTimer < enemy.ActiveTime || !enemy.EneCs) continue;
            if (enemy.EneCs.Active || enemy.EneCs.Hp <= 0) continue;

            enemy.EneCs.Activate();
        }
    }

    public void StageStart()
    {
        _StageTimer = 0f;
        _StageActive = true;
        foreach (EnemyInfo enemy in _Enemys) enemy.EneCs.ReSetting();
    }
}

[System.Serializable]

public class EnemyInfo
{
    public Enemy EneCs;
    [Header("活動開始時間")] public float ActiveTime;
}
