using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDome : MonoBehaviour
{
    [SerializeField] private int _HpMax;
    public int HpMax { get => _HpMax; }

    [SerializeField, ReadOnly] private int _Hp;
    public int Hp { get => _Hp; }

    [Space(30)]
    [SerializeField][Header("自動回復頻度")] private float _AutoHealInterval;
    private float _HealTimer = 0f;
    [SerializeField][Header("自動回復量")] private int _AutoHealPoint;

    [Space(30)]
    [SerializeField, ReadOnly][Header("ブレイク状態")] private bool _Breaking;
    public bool Breaking { get => _Breaking; }
    [SerializeField][Header("ブレイク回復時間")] private float _BreakHealTime;
    public float BreakHealTime { get => _BreakHealTime; }
    private float _BreakTimer = 0f;
    public float BreakTimer { get => _BreakTimer; }
    private int _UseGun = 0; //ブレイク時使用していた武器

    [Space(30)]
    [SerializeField][Header("半径")] private float _Radius;
    public float Radius { get => _Radius; }
    
    private Animator _ani;

    [Space(30)]
    [Header("ダメージSE")][SerializeField] private AudioClip _DamageSE;

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _Hp = _HpMax;
    }

    void Update()
    {
        if (GM.instance.IsPose) return;

        _AutoHealUpdate();
        _BreakUpdate();

        _ani.SetBool("Break", _Breaking);
    }

    private void _AutoHealUpdate()
    {
        if (_Hp >= _HpMax || _Breaking) return;

        if (_HealTimer < _AutoHealInterval) _HealTimer += Time.deltaTime;
        else
        {
            _Hp += _AutoHealPoint; //自動回復
            _HealTimer = 0f;
        }

        if (_Hp > _HpMax) _Hp = _HpMax;
    }

    private void _BreakUpdate()
    {
        if (!_Breaking) return;
        _BreakTimer += Time.deltaTime;
        if(_BreakTimer > _BreakHealTime)
        {
            StageManager.instance.MainSubHand().GetComponent<Hand>().GunChangeStart(_UseGun);
            _Breaking = false;
            _ani.SetTrigger("Damage");
            _Hp = _HpMax;
        }
    }

    private void _Break()
    {
        _UseGun = GM.instance.UseGun; //使用していた武器を記憶
        Hand handCs =  StageManager.instance.MainSubHand().GetComponent<Hand>();
        handCs.GunChangeStart(2); //武器をハンドガンへ
        _Breaking = true;
        _BreakTimer = 0f;
    }

    public void Damage(int damage)
    {
        if (_Breaking)
        {
            GM.instance.PlayOneSE(_DamageSE, transform, 0.7f);
            return;
        }

        GM.instance.PlayOneSE(_DamageSE, transform, 1f);
        _Hp -= damage;
        _ani.SetTrigger("Damage");

        if (_Hp <= 0)
        {
            _Break();
            Debug.Log("GAME OVER");
        }
    }
}
