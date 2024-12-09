using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class SHholdCol : MonoBehaviour
{
    [SerializeField]
    private MainGun _GunCs;

    private bool _HandIn;
    private InputActionAsset _IAA;
    private InputAction _HoldAction;

    private void Start()
    {
        _IAA = GM.instance.Player.GetComponent<InputActionManager>().actionAssets[0];
        if (GM.instance.LeftMain) _HoldAction = _IAA.FindActionMap("XRI RightHand Interaction").FindAction("Hold");
        else _HoldAction = _IAA.FindActionMap("XRI LeftHand Interaction").FindAction("Hold");
    }

    private void OnTriggerStay(Collider other)
    {
        if((other.gameObject == GM.instance.RightHand && GM.instance.LeftMain) || (other.gameObject == GM.instance.LeftHand && !GM.instance.LeftMain))
        {
            Debug.Log("In!!");
            if (!_GunCs.SubHand && _HoldAction.IsPressed()) _GunCs.HoldSubHand(other.transform);
        }
    }

    private void Update()
    {
        if (_GunCs.SubHand && !_HoldAction.IsPressed()) _GunCs.HoldSubHand();
    }
}
