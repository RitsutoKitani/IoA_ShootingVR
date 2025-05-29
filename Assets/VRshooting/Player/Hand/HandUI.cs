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

    private LineRenderer _lr;
    private Animator _RayAni;
    private Animator _HandAni;
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

        InputActionAsset IAA = StageManager.instance.IAA;
        if (_Left) _SelectAct = IAA.FindActionMap("XRI LeftHand Interaction").FindAction("Select");
        else _SelectAct = IAA.FindActionMap("XRI RightHand Interaction").FindAction("Select");
    }

    private void LateUpdate()
    {
        _HandActiveUpdate();
        if (GM.instance.UIhandLeft != _Left || !Active)
        {
            _HandAni.SetBool("HandGun", false);
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
        if(_XRRI.TryGetCurrent3DRaycastHit(out hit))
        {
            ishit = true;
            HitPos = hit.point;
            ObjPos = hit.collider.gameObject;
        }

        if(_lr) _lrUpdate(ishit , HitPos, ObjPos);
    }

    private void _HandActiveUpdate()
    {
        _Interactor.gameObject.SetActive(GM.instance.UIhandLeft == _Left && Active);
        if(GM.instance.UIhandLeft != _Left && _SelectAct.WasPressedThisFrame() && Active) GM.instance.UIhandLeft = _Left;
    }

    private void _lrUpdate(bool ishit ,Vector3 HitPos, GameObject Obj)
    {
        if(!_HandObj || !_FingerPoint) return;

        Vector3[] lrPos = new Vector3[2];
        lrPos[0] = _Interactor.position;
        lrPos[1] = _Interactor.position + _Interactor.forward * _DefaLength;
        bool Lock = false;
        
        if(ishit)
        {
            if (Obj.gameObject.tag == "AutoUI")
            {
                var aim = Obj.transform.position - _FingerPoint.position;
                var aimRot = Quaternion.LookRotation(aim);
                _HandObj.rotation = Quaternion.Slerp(_HandObj.rotation, aimRot, 0.3f);

                lrPos[0] = _FingerPoint.position;
                lrPos[1] = Obj.transform.position;

                Lock = true;
            }
            else
            {
                _HandObj.localRotation = Quaternion.Euler(0, 0, 0);

                lrPos[0] = _FingerPoint.position;
                lrPos[1] = HitPos;
            }
        }
        else _HandObj.localRotation = Quaternion.Euler(0, 0, 0);

        _lr.SetPositions(lrPos);
        _HandAni.SetBool("HandGun", ishit);
        _RayAni.SetBool("Touch", ishit);
        _RayAni.SetBool("Lock", Lock);
        if (_pointer) _pointer.position = _lr.GetPosition(1);
    }
}
