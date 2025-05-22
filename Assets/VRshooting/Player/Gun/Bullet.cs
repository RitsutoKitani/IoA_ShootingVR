using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    [Space(30)]

    [SerializeField]
    [Header("弾速")] private float _Speed;
    [SerializeField]
    [Header("攻撃力")] private int _Atk;
    [SerializeField]
    [Header("自動消滅時間")] private float _LimitTime;
    private float _timer = 0f;
    [SerializeField]
    [Header("ヒットエフェクト")] private GameObject _HitEff;
    [SerializeField]
    [Header("トレイルレンダラー")] private List<TrailRenderer> _tr = new List<TrailRenderer>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_timer > _LimitTime && _LimitTime > 0f) _Vanish();
        else _timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHitBox>())
        {
            EnemyHitBox ene = other.GetComponent<EnemyHitBox>();
            ene.EneCs.Damage(Mathf.FloorToInt(_Atk * ene.Pene));
        }

        if (_HitEff) Instantiate(_HitEff, transform.position, transform.rotation);
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
        _LimitTime = status.LimitTime;
        _timer = 0f;

        _rb.velocity = transform.forward * _Speed;
    }
}

[System.Serializable]
public class BulletStatus
{
    public float Speed;
    public int Atk;
    public float LimitTime;
    public GameObject HitEff;
}
