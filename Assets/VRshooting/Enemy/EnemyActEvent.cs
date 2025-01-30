using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActEvent : MonoBehaviour
{
    private Animator _ani;
    [SerializeField] private EnemySplineMove _SplineMoveCs;

    [SerializeField] [Header("íeî≠éÀà íu")] private Transform _ShotPos;
    [SerializeField] [Header("çUåÇíe")] private GameObject _Bullet;

    private void Start()
    {
        if (GetComponent<Animator>()) _ani = GetComponent<Animator>();
    }

    public void ShotEvent()
    {
        _ShotClip();
        _SplineMoveCs.StopFinish();

        /*
        if (_ani) _ani.SetTrigger("Shot");
        else
        {
            _ShotClip();
            _SplineMoveCs.StopFinish();
        }
        */
    }

    private void _ShotClip()
    {
        Instantiate(_Bullet, _ShotPos.position, _ShotPos.rotation);
    }
}
