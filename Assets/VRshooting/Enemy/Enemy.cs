using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private bool _Active;
    public bool Active { get => _Active; }

    [SerializeField] private int _HpMax;
    public int HpMax { get => HpMax; }

    [SerializeField] private int _Hp;
    public int Hp { get => _Hp; }

    [SerializeField] private int _Atk;
    public int Atk { get => _Atk; }

    [Space(30)]

    [SerializeField]
    [Header("終了時のに削除")] private bool _FinishDel;

    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();;
    }


    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        _Hp -= damage;

        if (_Hp <= 0)
        {
            if (_ani) _ani.SetBool("Destroy",true);
            else Finish();
        }
        else if (_ani) _ani.SetTrigger("Damage");
    }

    public void Finish()
    {
        if (_FinishDel) Destroy(gameObject);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 活動開始
    /// </summary>
    public void Activate()
    {
        _Active = true;
    }

    public void ReSetting()
    {
        _Hp = _HpMax;
        _Active = false;
    }
}
