using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [SerializeField][Header("弾発射位置")] private Transform _ShotPos;
    [SerializeField][Header("攻撃弾")] private GameObject _Bullet;
    [SerializeField][Header("チャージ時間")] private float _ChargeTime;
    [SerializeField][Header("連射間隔")] private float _ShotInterval;

    [Space(30)]
    [SerializeField, Range(0f, 1f)] private float _TurnSpeed;

    [Space(30)]
    [SerializeField] private Color _PointerColor;
    [SerializeField, Range(0,1f)] private float _PointerAlpha;
    [SerializeField] private LineRenderer[] _lrs;
    private GameObject _WarningUI;

    private bool _Charging;
    private float _timer = 0f;
    private int _BulletNum = 0;
    private Vector3 _AimPos;

    [Space(30)]
    [SerializeField] private Animator _ani;
    [SerializeField] private EnemySplineMove _splineMoveCs;

    private void OnEnable()
    {
        _Charging = false;
        _timer = 0f;
        _BulletNum = 0;

        //if (_splineMoveCs) _splineMoveCs.Resetting();
    }

    private void OnDisable()
    {
        if (_WarningUI) _WarningUI.SetActive(false);
    }

    private void Update()
    {
        if(_Charging) _ChargeUpdate();
        if(_ani) _ani.SetBool("Charging", _Charging);
        if(!_splineMoveCs.enabled) _splineMoveCs.enabled = true;
        _PointerUpdate();
    }

    public void ShotStart(int num = 1)
    {
        _timer = _ChargeTime;
        _BulletNum = num;
        _Charging = true;

        if (!StageManager.instance) return;

        Transform domeTra = StageManager.instance.DomeCs.transform;
        Vector3 dir;
        do dir = Random.onUnitSphere; //単位面のランダムベクトル生成
        while (Vector3.Dot(dir, Vector3.forward) < 0.5f //前方上方で横0.8以内に制限
        || Vector3.Dot(dir, Vector3.up) < 0.1f);

        Vector3 worldDir = domeTra.TransformDirection(dir);
        _AimPos = domeTra.position + worldDir * StageManager.instance.DomeCs.Radius;

        if (ObjPool.instance)
        {
            Vector3 diff = _AimPos - StageManager.instance.DomeCs.transform.position;
            Quaternion rot = Quaternion.LookRotation(diff);
            _WarningUI = ObjPool.instance.MakeObj(ObjPool.instance.WarningUI, _AimPos, rot);
        }
    }

    private void _ChargeUpdate()
    {

        var diff = _AimPos - transform.position;
        var targetRot = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _TurnSpeed);

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
            if (_WarningUI) _WarningUI.SetActive(false);
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
            lr.endColor = _PointerColor * new Color(1, 1, 1, 0);
            lr.SetPosition(0, _ShotPos.position);
            lr.SetPosition(1, _AimPos);
        }
    }
}
