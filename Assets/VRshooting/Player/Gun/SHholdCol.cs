using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class SHholdCol : MonoBehaviour
{
    [SerializeField]
    private MainGun _GunCs;

    private InputActionAsset _IAA;
    private InputAction _HoldAction;

    private void Start()
    {
        _IAA = StageManager.instance.IAA;
        if (GM.instance.LeftMain) _HoldAction = _IAA.FindActionMap("GunAction R").FindAction("Hold");
        else _HoldAction = _IAA.FindActionMap("GunAction L").FindAction("Hold");
    }

    private void OnTriggerStay(Collider other)  
    {
        if((other.gameObject == StageManager.instance.RightHand && GM.instance.LeftMain) || (other.gameObject == StageManager.instance.LeftHand && !GM.instance.LeftMain))
        {
            if (!_GunCs.SubHand && _HoldAction.IsPressed() && _GunCs.CanShot) _GunCs.HoldSubHand(other.transform);
        }
    }

    private void Update()
    {
        if (_GunCs.SubHand && !_HoldAction.IsPressed()) _GunCs.HoldSubHand();
    }
}
