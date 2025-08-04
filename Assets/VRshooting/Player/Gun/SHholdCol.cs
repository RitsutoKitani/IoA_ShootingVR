using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
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
        if (GM.instance.IsPose) return;
        if((other.gameObject == StageManager.instance.RightHand && GM.instance.LeftMain) || (other.gameObject == StageManager.instance.LeftHand && !GM.instance.LeftMain))
        {
            if (!_GunCs.SubHand && _HoldAction.IsPressed() && _GunCs.CanShot) _GunCs.HoldSubHand(other.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_GunCs.SubHand || GM.instance.IsPose) return;
        GameObject hand = GM.instance.LeftMain ? StageManager.instance.RightHand : StageManager.instance.LeftHand;
        if (other.gameObject == hand)
        {
            StageManager.instance.MainSubHandVibe(true, 0.6f, 0.05f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_GunCs.SubHand || GM.instance.IsPose) return;
        GameObject hand = GM.instance.LeftMain ? StageManager.instance.RightHand : StageManager.instance.LeftHand;
        if (other.gameObject == hand)
        {
            StageManager.instance.MainSubHandVibe(true, 0.3f, 0.05f);
        }
    }

    private void Update()
    {
        if (_GunCs.SubHand && !_HoldAction.IsPressed()) _GunCs.HoldSubHand(); //‚Â‚©‚Ý‚ð‚â‚ß‚½‚Æ‚«
    }
}
