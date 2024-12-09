using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private bool R;

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

    private XRBaseController _controller;

    private float _timer = 0f;

    private InputActionAsset _IA;
    private InputAction _GunShotAct;
    private AudioSource _AS;
    private Animator _ani;

    private void Start()
    {
        if (GameObject.Find("Player").GetComponent<InputActionManager>())
        {
            _IA = GameObject.Find("Player").GetComponent<InputActionManager>().actionAssets[0];
            if (R) _GunShotAct = _IA.FindActionMap("XRI RightHand Interaction").FindAction("GunShot");
            else _GunShotAct = _IA.FindActionMap("XRI LeftHand Interaction").FindAction("GunShot");
        }

        if (transform.parent.GetComponent<XRBaseController>()) _controller = transform.parent.GetComponent<XRBaseController>();

        if(GetComponent<AudioSource>()) _AS = GetComponent<AudioSource>();

        if (GetComponent<Animator>()) _ani = GetComponent<Animator>();

        _timer = _Interval;
    }

    private void Update()
    {
        if (_timer < _Interval) _timer += Time.deltaTime;
        else if (_GunShotAct.WasPressedThisFrame())
        {
            if (!_Bullet || !_ShotPos)
            {
                Debug.LogError("弾か発射位置が設定されていません"); return;
            }

            Instantiate(_Bullet, _ShotPos.position,_ShotPos.rotation);
            _timer = 0f;

            if (_controller) _controller.SendHapticImpulse(0.6f, 0.05f);
            if (_AS && _ShotSE) _AS.PlayOneShot(_ShotSE);
            if (_ani) _ani.SetTrigger("Shot");
        }

        if (_LR && _ShotPos)
        {
            _LR.SetPosition(0, _ShotPos.position);
            _LR.SetPosition(1, _ShotPos.position + _ShotPos.forward * 3f);
        }
    }
}
