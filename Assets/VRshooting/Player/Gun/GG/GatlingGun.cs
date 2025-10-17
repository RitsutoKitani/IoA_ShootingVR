using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GatlingGun : MainGun
{
    [Space(60)]
    [Header("ガトリングガン固有ステータス")]
    [SerializeField, Tooltip("片手持ちの際の角度")] private float _OHangle;
    [SerializeField] private Transform _SHaimPos;

    [Space(30)]
    [SerializeField, Tooltip("オーバーヒートまでの時間")] private float _FireTime;
    [SerializeField, Tooltip("回復速度")] private float _CoolSpeed;
    [SerializeField, Tooltip("オーバーヒート回復時間")] private float _OverHeatTime;
    [SerializeField, ReadOnly] private float _HeatTimer = 0f;
    [SerializeField, ReadOnly] private float _ShotTimer = 0f;
    [SerializeField, ReadOnly] private bool _OverHeating = false;
    public bool OverHeating { get => _OverHeating; }

    private float _GageVal;
    public float GageVal { get => _GageVal; }


    protected override void _GunRotUpdate()
    {
        Vector3 MainHandForward = _MainHand.transform.forward;
        Vector3 HorizonForward = new Vector3(MainHandForward.x, 0f, MainHandForward.z).normalized;
        Quaternion HorizonRot = Quaternion.LookRotation(HorizonForward, Vector3.up);

        Quaternion AimRot = HorizonRot * Quaternion.AngleAxis(_OHangle, Vector3.Cross(Vector3.up, HorizonForward));    //水平方向に任意の角度回転

        float Stabi = _OHstabi;
        if (_SubHand)
        {
            Vector3 SHoffset = Vector3.zero;
            if (_SHaimPos) SHoffset = _SHaimPos.position - SubHandle.position;

            var aim = _SubHand.position + SHoffset - _MainHand.position;
            AimRot = Quaternion.LookRotation(aim);
            Stabi = _BHstabi;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, AimRot, Stabi);
    }

    protected override void _ShotUpdate()
    {
        float TimeScale = StageManager.instance.TimeScale;
        if (_ani)
        {
            _ani.SetBool("OverHeat", _OverHeating);
            _ani.SetBool("Rolling", !_OverHeating && _GunShotAct.IsPressed());
        }

        if (!CanShot) return;

        if (!_OverHeating)
        {
            _GageVal = _HeatTimer / _FireTime;

            if (_GunShotAct.IsPressed())
            {
                if (_ShotTimer < _Data.ShotInterval) _ShotTimer += Time.deltaTime * TimeScale;      //射撃タイマー更新
                else
                {
                    _ShotTimer = 0f;
                    _Shot();
                }

                _HeatTimer -= Time.deltaTime * TimeScale;                                           //ヒートタイマー減少
                if (_HeatTimer < 0f) _OverHeat();
            }
            else if (_HeatTimer < _FireTime)
            {
                _HeatTimer += Time.deltaTime * _CoolSpeed * TimeScale;                              //ヒートタイマー回復（通常）
            }
        }
        else 
        {
            _GageVal = _HeatTimer / _OverHeatTime;
            if (_HeatTimer < _OverHeatTime) _HeatTimer += Time.deltaTime * TimeScale;               //ヒートタイマー回復（オーバーヒート）
            else _HeatCool();
        }

        _MagazineBullet = Mathf.FloorToInt(_GageVal * 100);
    }

    private void _OverHeat()
    {
        _OverHeating = true;
        _HeatTimer = 0f;
    }

    private void _HeatCool()
    {
        _OverHeating = false;
        _HeatTimer = _FireTime;
    }

    public override void InitalSetting(Transform hand, bool Left)
    {
        base.InitalSetting(hand, Left);
    }
}
