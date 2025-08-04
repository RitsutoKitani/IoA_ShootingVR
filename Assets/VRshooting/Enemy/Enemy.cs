using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int _HpMax;
    public int HpMax { get => HpMax; }

    [SerializeField] private int _Hp;
    public int Hp { get => _Hp; }

    [SerializeField] private int _Atk;
    public int Atk { get => _Atk; }

    [SerializeField] private int _Score;
    public int Score { get => _Score; }

    [Space(30)]
    [SerializeField]
    [Header("終了時のに削除")] private bool _FinishDel;

    [Space(30)]
    [SerializeField, ReadOnly] private Animator _ani;

    [SerializeField]
    [Header("コライダー")] private Collider[] _cols;

    [Space(30)]
    [SerializeField][Header("ダメージ効果音（効果抜群）")] private AudioClip _DamageEffSE;
    [SerializeField][Header("ダメージ効果音（通常）")] private AudioClip _DamageSE;

    private Vector3 _FirstPos;
    public Vector3 FirstPos { get => _FirstPos; }
    private bool _FirstTime = true;


    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _FirstPos = transform.position;
    }

    private void Update()
    {
        if(_ani) _ani.speed = GM.instance.IsPose ? 0.0f : 1.0f;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage, float pene = 1.0f)
    {
        _Hp -= Mathf.FloorToInt(damage * pene);

        if (_DamageEffSE && _DamageSE)
        {
            AudioClip DamageClip = _DamageSE;
            if (pene > 0.9) DamageClip = _DamageEffSE;
            GM.instance.PlayOneSE(DamageClip, transform, 1f, Random.Range(0.9f, 1.1f)); //ダメージ効果音再生
        }

        if (_Hp <= 0)
        {
            if (StageManager.instance) StageManager.instance.ScoreGet(Score);
            foreach(Collider col in _cols) col.enabled = false;
            if (_ani) _ani.SetBool("Destroy",true);
            else Finish();
        }
        else if (_ani) _ani.SetTrigger("Damage");
    }

    public void Finish()
    {
        if (_FinishDel) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public void ReSetting()
    {
        if(!_FirstTime) transform.position = _FirstPos;
        _FirstTime = false;

        foreach (Collider col in _cols) col.enabled = true;
        _Hp = _HpMax;
        gameObject.SetActive(false);
    }

    private void ExploEff()
    {
        ObjPool.instance.MakeObj(ObjPool.instance.ExploEffPool, transform.position, transform.rotation);
    }
}
