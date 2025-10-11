using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] protected BulletStatus _Status;
    [SerializeField] private float _GuardPene = 0f;
    private Vector3 _StartPos;
    private float _StartDis;
    private Vector3 _StartSize;
    [Header("距離におけるサイズ")][SerializeField] private AnimationCurve _DisSizeCurve;
    [SerializeField][Header("トレイルレンダラー")] private List<TrailRenderer> _tr = new List<TrailRenderer>();

    [Space(30)]
    [SerializeField] protected GameObject _BarrierHitEffect;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _StartPos = transform.position;
        _StartDis = Vector3.SqrMagnitude(transform.position - StageManager.instance.DomeCs.transform.position);
        _StartSize = transform.localScale;
    }

    private void Update()
    {
        if (GM.instance.IsPose)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        MoveUpdate();

            float distance = (transform.position - _StartPos).sqrMagnitude;
        if (distance > _Status.LimitDistance * _Status.LimitDistance) _Vanish();
    }

    protected virtual void MoveUpdate()
    {
        float dis = Vector3.SqrMagnitude(transform.position - StageManager.instance.DomeCs.transform.position);
        transform.localScale = _StartSize * _DisSizeCurve.Evaluate(dis / _StartDis);
        _rb.velocity = transform.forward * _Status.Speed * StageManager.instance.TimeScale;

        if (_tr.Count <= 0) return;
        foreach (var tr in _tr)
        {
            tr.startWidth = transform.localScale.x;
            tr.endWidth = transform.localScale.x;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDome>())
        {
            PlayerDome dome = other.GetComponent<PlayerDome>();
            dome.Damage(_Status.Atk);
            _Vanish();
            if (_BarrierHitEffect) ObjPool.instance.MakeObjByList(_BarrierHitEffect, transform.position, transform.rotation);
        }

        if (other.GetComponent<GuardBarrier>())
        {
            GuardBarrier guard = other.GetComponent<GuardBarrier>();
            guard.GuardDamage(_Status.Atk, _GuardPene);
            _Vanish();
        }


        if (other.tag == "Wall")
        {
            if(_Status.HitEff) Instantiate(_Status.HitEff, transform.position, transform.rotation); //エフェクトがあれば生成
            _Vanish();
        }

    }

    private void _Vanish()
    {
        foreach (var tr in _tr) tr.gameObject.transform.parent = null; //トレイルレンダラーの子を外す
        Destroy(gameObject);
    }

    public void Setting(BulletStatus status)
    {
        _Status = status;
        if (_Status.LimitDistance <= 0) _Status.LimitDistance = 1000;
    }
}
