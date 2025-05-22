using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDome : MonoBehaviour
{
    [SerializeField] private int _HpMax;
    public int HpMax { get => _HpMax; }

    [SerializeField] private int _Hp;
    public int Hp { get => _Hp; }

    [Space(30)]
    [SerializeField][Header("Ž©“®‰ñ•œ•p“x")] private float _AutoHealInterval;
    private float _HealTimer = 0f;
    [SerializeField][Header("Ž©“®‰ñ•œ—Ê")] private int _AutoHealPoint;

    [Space(30)]
    [SerializeField][Header("”¼Œa")] private float _Radius;
    public float Radius { get => _Radius; }

    private void Start()
    {
        _Hp = _HpMax;
    }

    void Update()
    {
        if (!GM.instance.StageManager) return;
        if (GM.instance.IsPose || !GM.instance.StageManager.StageActive) return;

        AutoHealUpdate();
    }

    private void AutoHealUpdate()
    {
        if (_Hp >= _HpMax) return;

        if (_HealTimer < _AutoHealInterval) _HealTimer += Time.deltaTime;
        else
        {
            _Hp += _AutoHealPoint; //Ž©“®‰ñ•œ
            _HealTimer = 0f;
        }

        if (_Hp > _HpMax) _Hp = _HpMax;
    }

    public void Damage(int damage)
    {
        _Hp -= damage;

        if (_Hp < 0)
        {
            Debug.Log("GAME OVER");

        }
    }
}
