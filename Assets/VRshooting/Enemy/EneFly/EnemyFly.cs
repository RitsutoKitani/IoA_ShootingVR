using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [SerializeField][Header("’e”­ŽËˆÊ’u")] private Transform _ShotPos;
    [SerializeField][Header("UŒ‚’e")] private GameObject _Bullet;
    [SerializeField][Header("ƒ`ƒƒ[ƒWŽžŠÔ")] private float _ChargeTime;
    [SerializeField][Header("˜AŽËŠÔŠu")] private float _ShotInterval;

    [Space(30)]
    [SerializeField] private Color _PointerColor;
    [SerializeField, Range(0,1f)] private float _PointerAlpha;
    [SerializeField] private LineRenderer[] _lrs;

    private bool _Charging;
    private float _timer = 0f;
    private int _BulletNum = 0;
    private Vector3 _AimPos;

    [Space(30)]
    [SerializeField] private Animator _ani;
    [SerializeField] private EnemySplineMove _splineMoveCs;



    private void Update()
    {
        if(_Charging) _ChargeUpdate();
        if (_ani) _ani.SetBool("Charging", _Charging);
        _PointerUpdate();
    }

    public void ShotStart(int num = 1)
    {
        _timer = _ChargeTime;
        _BulletNum = num;
        _Charging = true;

        if (StageManager.instance.DomeCs.gameObject) _AimPos = StageManager.instance.DomeCs.gameObject.transform.position;
        else _AimPos = transform.position + transform.forward * 100f;
    }

    private void _ChargeUpdate()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            return;
        }

        _Shot();
        _BulletNum--;

        if (_BulletNum < 1)
        {
            _Charging = false;
            if (_splineMoveCs) _splineMoveCs.Invoke("StopFinish", 1f);
        }
        else
        {
            _timer = _ShotInterval;
        }
    }

    private void _Shot()
    {
        if (!_ShotPos || !_Bullet) return;

        Quaternion ShotRot = Quaternion.LookRotation(_AimPos - _ShotPos.position);
        Instantiate(_Bullet, _ShotPos.position, ShotRot);
        if (_ani) _ani.SetTrigger("Shot");
    }

    private void _PointerUpdate()
    {
        foreach(LineRenderer lr in _lrs)
        {
            lr.startColor = _PointerColor * new Color(1,1,1,_PointerAlpha);
            lr.endColor = _PointerColor * new Color(1, 1, 1, _PointerAlpha);
                        lr.SetPosition(0, _ShotPos.position);
            lr.SetPosition(1, _AimPos);
        }
    }
}
