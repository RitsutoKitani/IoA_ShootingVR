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
    private float _timer = 0f;
    [SerializeField][Header("トレイルレンダラー")] private List<TrailRenderer> _tr = new List<TrailRenderer>();

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveUpdate();
        if (_timer > _Status.LimitTime && _Status.LimitTime > 0) _Vanish();
        else _timer += Time.deltaTime;
    }

    protected virtual void MoveUpdate()
    {
        _rb.velocity = transform.forward * _Status.Speed;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDome>())
        {
            PlayerDome dome = other.GetComponent<PlayerDome>();
            dome.Damage(_Status.Atk);
            _Vanish();
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
    }
}
