using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class HandUI : MonoBehaviour
{
    public bool Active = true;
    [SerializeField] private bool _Left;

    [Space(30)]
    [SerializeField]
    [Header("UIに触れていないときの長さ")] private float _DefaLength;

    [Space(30)]
    [SerializeField]
    [Header("ポインター")] private Transform _pointer;
    [SerializeField]
    [Header("指先")] private Transform _FingerPoint;
    [SerializeField]
    [Header("手（回転させるObj）")] private Transform _HandObj;
    [SerializeField] private Transform _Interactor;
    private GameObject _AutoObj = null;

    private LineRenderer _lr;
    private Animator _RayAni;
    private Animator _HandAni;
    private Animator _PointerAni;
    private XRRayInteractor _XRRI;

    private InputAction _SelectAct;

    private void Start()
    {
        if (_Interactor)
        {
            _RayAni = _Interactor.GetComponent<Animator>();
            _XRRI = _Interactor.GetComponent<XRRayInteractor>();
            _lr = _Interactor.GetComponent<LineRenderer>();
        }
        if (_HandObj) _HandAni = _HandObj.GetComponent<Animator>();
        if(_pointer) _PointerAni = _pointer.GetComponent<Animator>();

        InputActionAsset IAA = StageManager.instance.IAA;
        if (_Left) _SelectAct = IAA.FindActionMap("XRI LeftHand Interaction").FindAction("Select");
        else _SelectAct = IAA.FindActionMap("XRI RightHand Interaction").FindAction("Select");
    }

    private void Update()
    {
        _HandActiveUpdate();
        if (GM.instance.UIhandLeft != _Left || !Active)
        {
            _HandAni.SetBool("HandGun", false);
            _PointerAni.SetBool("Active", false);
            return;
        }

        bool ishit = false;
        Vector3 HitPos = Vector3.zero;
        GameObject ObjPos = null;

        RaycastResult result;
        if(_XRRI.TryGetCurrentUIRaycastResult(out result))
        {
            ishit = true;
            HitPos = result.worldPosition;
            ObjPos = result.gameObject;
        }

        RaycastHit hit;
        if(_XRRI.TryGetCurrent3DRaycastHit(out hit) && !GM.instance.IsPose)
        {
            ishit = true;
            HitPos = hit.point;
            ObjPos = hit.collider.gameObject;
        }

        if(_lr) _lrUpdate(ishit , HitPos, ObjPos);
    }

    private void _HandActiveUpdate()
    {
        _lr.enabled = GM.instance.UIhandLeft == _Left && Active;
        _XRRI.enabled = GM.instance.UIhandLeft == _Left && Active;
        _RayAni.enabled = GM.instance.UIhandLeft == _Left && Active;
        if(GM.instance.UIhandLeft != _Left && _SelectAct.WasPressedThisFrame() && Active) GM.instance.UIhandLeft = _Left;
    }

    private void _lrUpdate(bool ishit ,Vector3 HitPos, GameObject Obj)
    {
        if(!_HandObj || !_FingerPoint) return;

        Vector3[] lrPos = new Vector3[2];
        lrPos[0] = _Interactor.position;
        lrPos[1] = _Interactor.position + _Interactor.forward * _DefaLength;

        if (_AutoObj)//ターゲットロック
        {
            var aim = _AutoObj.transform.position - _FingerPoint.position;
            var aimRot = Quaternion.LookRotation(aim);
            _HandObj.rotation = Quaternion.Slerp(_HandObj.rotation, aimRot, 0.5f);

            lrPos[0] = _FingerPoint.position;
            lrPos[1] = _AutoObj.transform.position;
        }
        else if (ishit)
        {
            if (Obj.tag == "AutoUI") _AutoObj = Obj;

            lrPos[0] = _FingerPoint.position;
            lrPos[1] = HitPos;
        }

        if (((ishit && Obj.tag != "AutoUI") || !ishit) && !_SelectAct.IsPressed())
        {
            UnLock();
        }

        _lr.SetPositions(lrPos);
        _pointer.position = lrPos[1];
        _pointer.rotation = Quaternion.LookRotation(lrPos[1] - Camera.main.transform.position);
        _HandAni.SetBool("HandGun", ishit || _AutoObj);
        _RayAni.SetBool("Touch", ishit);
        if (_PointerAni) _PointerAni.SetBool("Active", ishit || _AutoObj);
        _RayAni.SetBool("Lock", _AutoObj);
        _PointerAni.SetBool("Lock", _AutoObj);
    }

    public void UnLock()
    {
        _AutoObj = null;
        if(Active) _HandObj.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
