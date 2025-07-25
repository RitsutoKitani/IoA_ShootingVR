using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXautoInActive : MonoBehaviour
{
    private VisualEffect vfx;
    [SerializeField] private AudioClip _EffectSE;
    private bool _check = false;

    private void Start()
    {
        if (_EffectSE) GM.instance.PlayOneSE(_EffectSE, transform);
        if(!GetComponent<VisualEffect>()) Destroy(gameObject);
        vfx = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        _check = false;
        Invoke("_CheckTrue", 0.1f);
    }

    private void _CheckTrue()
    {
        _check = true;
    }

    private void Update()
    {
        if(vfx.aliveParticleCount == 0 && _check)
        {
            gameObject.SetActive(false);
        }
    }
}
