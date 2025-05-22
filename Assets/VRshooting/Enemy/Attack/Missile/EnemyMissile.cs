using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : EnemyBullet
{
    [SerializeField] private Vector3 _TargetPos;
    [SerializeField, Range(0f, 0.1f)][Header("誘導調整")] private float _torque;
    private float _NoGuideTimer = 0f;

    [Space(10)]
    private bool _purge = false;
    [SerializeField] private GameObject _meshL;
    [SerializeField][Header("ミサイルをパージする距離")] private float _PurgeDis;
    [SerializeField][Header("パージ時速度アップ")] private float _PurgeUp;

    protected override void MoveUpdate()
    {
        float purgeUp = 1f;
        if (_purge) purgeUp = _PurgeUp;

        if (_NoGuideTimer > 0f) _NoGuideTimer -= Time.deltaTime;
        else
        {
            var diff = _TargetPos - transform.position;
            var targetRot = Quaternion.LookRotation(diff);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _torque * purgeUp);
        }

        if (_meshL && !_purge)
        {
            if (Vector3.Distance(_TargetPos, transform.position) < _PurgeDis)
            {
                _meshL.SetActive(false);
                _purge = true;
            }
        }

        _rb.velocity = transform.forward * _Status.Speed * purgeUp;
    }

    public void MissileSetting(Vector3 targetPos, float NoGuideTime = 0f)
    {
        _NoGuideTimer = NoGuideTime;
        _TargetPos = targetPos;
    }
}
