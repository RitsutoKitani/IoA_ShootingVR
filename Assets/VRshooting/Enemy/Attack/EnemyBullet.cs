using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;
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
        if (!GetComponent<Rigidbody>())
        {
            Debug.LogError("弾にリジッドボディが入っていません");
            _Vanish();
        }
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * _Speed;
    }

    private void Update()
    {
        if (_timer > _LimitTime) _Vanish();
        else _timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerDome>())
        {
            PlayerDome dome = other.GetComponent<PlayerDome>();
            dome.Damage(_Atk);
        }

        if (_HitEff) Instantiate(_HitEff, transform.position, transform.rotation); //エフェクトがあれば生成
        _Vanish();
    }

    private void _Vanish()
    {
        foreach (var tr in _tr) tr.gameObject.transform.parent = null; //トレイルレンダラーの子を外す
        Destroy(gameObject);
    }
}
