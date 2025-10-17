using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI_GG : MonoBehaviour
{
    [SerializeField] private GatlingGun _GunCs;

    [SerializeField] private Image _Gage;

    [SerializeField] private Renderer _HeatMr;
    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }


    private void Update()
    {
        if(!_GunCs) return;

        if(_Gage) _Gage.fillAmount = _GunCs.GageVal;

        if(_ani)
        {
            _ani.SetBool("BothHand", _GunCs.SubHand);
            _ani.SetBool("NoAmmo", _GunCs.OverHeating);
        }

        if (_HeatMr) _HeatMr.material.SetFloat("_alpha", (1f - _GunCs.GageVal) * 0.8f);
    }
}
