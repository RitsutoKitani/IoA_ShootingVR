using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleButton : MonoBehaviour
{
    public bool Interactable = true;
    [SerializeField][Header("’·‰Ÿ‚µƒ^ƒCƒv")] private bool _PressType = false;

    [SerializeField, ReadOnly] private bool _Toggle;
    public bool Toggle { get => _Toggle; }

    private Animator _ani;
    private XRSimpleInteractable _XRSI;

    [SerializeField] private AudioClip _TouchSE;
    [SerializeField] private AudioClip _ClickSE;

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _XRSI = GetComponent<XRSimpleInteractable>();
    }

    private void Update()
    {
        _XRSI.enabled = !GM.instance.IsPose;
        if (GM.instance.IsPose && _PressType && _Toggle) ToggleSet(false);
    }

    public void ToggleChange()
    {
        if (_PressType || GM.instance.IsPose) return;
        if (_ClickSE) GM.instance.PlayOneSE(_ClickSE, transform);
        _Toggle = !_Toggle;
        _ani.SetTrigger("Click");
        _ani.SetBool("Toggle", _Toggle);
    }

    public void TogglePress(bool press)
    {
        if(!_PressType || !Interactable || GM.instance.IsPose) return;
        if (_ClickSE) GM.instance.PlayOneSE(_ClickSE, transform);
        _Toggle = press;
        _ani.SetBool("Toggle", _Toggle);
        if (press) _ani.SetTrigger("Click");
    }

    public void ToggleSet(bool toggle)
    {
        _Toggle = toggle;
        _ani.SetBool("Toggle", _Toggle);
    }

    public void TouchOnOff(bool touch)
    {
        if (_TouchSE) GM.instance.PlayOneSE(_TouchSE, transform);
        _ani.SetBool("Touch", touch);
    }
}
