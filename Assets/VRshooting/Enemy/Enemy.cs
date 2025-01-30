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

    [Space(30)]

    [SerializeField]
    [Header("終了時のに削除")] private bool _FinishDel;

    private Animator _ani;

    private void Start()
    {
        Relocation();
        _ani = GetComponent<Animator>();
    }

    /// <summary>
    /// 敵の再配置（初期処理）
    /// </summary>
    public void Relocation()
    {
        _Hp = _HpMax;
        if (_ani) _ani.SetBool("Destroy", false);
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
}
