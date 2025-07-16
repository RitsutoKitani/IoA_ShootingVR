using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    [Space(30)]

    [SerializeField, ReadOnly][Header("弾速")] private float _Speed;
    [SerializeField, ReadOnly][Header("攻撃力")] private int _Atk;
    [SerializeField, ReadOnly][Header("射程距離")] private float _LimitDistance;
    private Vector3 _StartPos;
    [SerializeField][Header("ヒットエフェクト（弱点）")] private GameObject _HitEffWeak;
    [SerializeField][Header("ヒットエフェクト（通常）")] private GameObject _HitEffNormal;
    [SerializeField][Header("ヒットエフェクト（壁）")] private GameObject _HitEffWall;
    [SerializeField][Header("トレイルレンダラー")] private List<TrailRenderer> _tr = new List<TrailRenderer>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _StartPos = transform.position;
    }

    private void Update()
    {
        float distance = (transform.position - _StartPos).sqrMagnitude;
        if (distance > _LimitDistance * _LimitDistance) _Vanish();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject Effect = _HitEffWall;

        if (other.GetComponent<EnemyHitBox>())
        {
            EnemyHitBox ene = other.GetComponent<EnemyHitBox>();
            ene.EneCs.Damage(Mathf.FloorToInt(_Atk * ene.Pene));
            if (ene.Pene <= 0.9f) Effect = _HitEffNormal;
            else Effect = _HitEffWeak;
        }

        if (Effect) ObjPool.instance.MakeObjByList(Effect, transform.position, transform.rotation);

        _Vanish();
    }

    private void _Vanish()
    {
        _rb.velocity = Vector3.zero;

        foreach (TrailRenderer tr in _tr)
        {
            tr.Clear();
        }

        gameObject.SetActive(false);
    }

    public void ReStatus(BulletStatus status)
    {
        _Speed = status.Speed;
        _Atk = status.Atk;
        _LimitDistance = status.LimitDistance;
        if (_LimitDistance <= 0) _LimitDistance = 1000;

        _rb.velocity = transform.forward * _Speed;
    }
}

[System.Serializable]
public class BulletStatus
{
    public float Speed;
    public int Atk;
    public float LimitDistance;
    public GameObject HitEff;
}
