using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class MainGun : MonoBehaviour
{
    [SerializeField]
    [Header("メイン手")] private Transform _MainHand;
    [SerializeField]
    [Header("サブ手")] private Transform _SubHand;
    public Transform SubHand { get => _SubHand; }

    [SerializeField]
    [Header("弾")] private GameObject _Bullet;
    [SerializeField]
    [Header("発射位置")] private Transform _ShotPos;
    [SerializeField]
    [Header("連射間隔")] private float _Interval = 0f;
    [SerializeField]
    [Header("発射効果音")] private AudioClip _ShotSE;

    [SerializeField]
    [Header("ラインレンダラー")] private LineRenderer _LR;

    [SerializeField, Range(0f, 1f)]
    [Header("片手ブレ補正")] private float _OHstabi;
    [SerializeField, Range(0f, 1f)]
    [Header("両手ブレ補正")] private float _BHstabi;

    private float _timer = 0f;

    private bool _Left;
    private XRBaseController _MainXRBC;
    private XRBaseController _SubXRBC;
    private InputActionAsset _IAA;
    private InputAction _GunShotAct;
    private AudioSource _AS;
    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _AS = GetComponent<AudioSource>();
        _IAA = GM.instance.Player.GetComponent<InputActionManager>().actionAssets[0];
        if(_Left) _GunShotAct = _IAA.FindActionMap("XRI LeftHand Interaction").FindAction("GunShot");
        else _GunShotAct = _IAA.FindActionMap("XRI RightHand Interaction").FindAction("GunShot");
    }

    void Update()
    {
        if (_timer < _Interval) _timer += Time.deltaTime;
        else if (_GunShotAct.IsPressed())
        {
            if (!_Bullet || !_ShotPos)
            {
                Debug.LogError("弾か発射位置が設定されていません"); return;
            }

            Instantiate(_Bullet, _ShotPos.position, _ShotPos.rotation);
            _timer = 0f;

            _MainXRBC.SendHapticImpulse(0.6f, 0.05f);
            if (_SubHand) _SubXRBC.SendHapticImpulse(0.6f, 0.05f);
            if (_AS && _ShotSE) _AS.PlayOneShot(_ShotSE);
            if (_ani) _ani.SetTrigger("Shot");
        }

        if (_LR && _ShotPos)
        {
            _LR.SetPosition(0, _ShotPos.position);
            _LR.SetPosition(1, _ShotPos.position + _ShotPos.forward * 3f);
        }


        if (!_MainHand)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = _MainHand.position;

        Quaternion AimRot = _MainHand.rotation;
        float Stabi = _OHstabi;
        if (_SubHand)
        {
            var aim = _SubHand.position - _MainHand.position;
            AimRot = Quaternion.LookRotation(aim);
            Stabi = _BHstabi;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, AimRot, Stabi);
    }
    public void InitalSetting(Transform hand, bool Left)
    {
        _MainHand = hand;
        _Left = Left;
        if (Left)
        {
            _MainXRBC = GM.instance.LeftHand.GetComponent<XRBaseController>();
            _SubXRBC = GM.instance.RightHand.GetComponent<XRBaseController>();
        }
        else
        {
            _MainXRBC = GM.instance.RightHand.GetComponent<XRBaseController>();
            _SubXRBC = GM.instance.LeftHand.GetComponent<XRBaseController>();
        }
        
    }

    public void HoldSubHand(Transform subhand = null)
    {
        _SubHand = subhand;
    }
}
