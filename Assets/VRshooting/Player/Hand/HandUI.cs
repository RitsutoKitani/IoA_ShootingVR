using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class HandUI : MonoBehaviour
{
    [SerializeField]
    [Header("UIに触れていないときの長さ")] private float _DefaLength;

    [Space(30)]
    [SerializeField]
    [Header("ポインター")] private Transform _pointer;
    [SerializeField]
    [Header("指先")] private Transform _FingerPoint;
    [SerializeField]
    [Header("手（回転させるObj）")] private Transform _HandObj;

    private LineRenderer _lr;
    private Animator _ani;
    private Animator _HandAni;
    private XRRayInteractor _XRRI;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _ani = GetComponent<Animator>();
        if (_HandObj) _HandAni = _HandObj.GetComponent<Animator>();
        _XRRI = GetComponent<XRRayInteractor>();
    }

    private void LateUpdate()
    {
        RaycastResult result;
        bool ishit = _XRRI.TryGetCurrentUIRaycastResult(out result);

        if(_lr) _lrUpdate(ishit, result);
    }

    private void _lrUpdate(bool ishit, RaycastResult result)
    {
        if(!_HandObj || !_FingerPoint) return;

        Vector3[] lrPos = new Vector3[2];
        lrPos[0] = transform.position;
        lrPos[1] = transform.position + transform.forward * _DefaLength;
        bool Lock = false;

        if (ishit)
        {
            if (result.gameObject.tag == "AutoUI")
            {
                Vector3 UIpos = result.gameObject.transform.position;

                var aim = UIpos - _FingerPoint.position;
                var aimRot = Quaternion.LookRotation(aim);
                _HandObj.rotation = Quaternion.Slerp(_HandObj.rotation, aimRot, 0.3f);

                lrPos[0] = _FingerPoint.position;
                lrPos[1] = UIpos;

                Lock = true;
            }
            else
            {
                _HandObj.localRotation = Quaternion.Euler(0, 0, 0);

                lrPos[0] = _FingerPoint.position;
                lrPos[1] = result.worldPosition;
            }
        }
        else _HandObj.localRotation = Quaternion.Euler(0, 0, 0);

        _lr.SetPositions(lrPos);
        _HandAni.SetBool("HandGun", ishit);
        _ani.SetBool("Touch", ishit);
        _ani.SetBool("Lock", Lock);
        if (_pointer) _pointer.position = _lr.GetPosition(1);
    }
}
