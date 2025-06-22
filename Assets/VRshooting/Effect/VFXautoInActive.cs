using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXautoInActive : MonoBehaviour
{
    private VisualEffect vfx;
    [SerializeField] private AudioClip _EffectSE;

    private void Start()
    {
        if (_EffectSE) GM.instance.PlayOneSE(_EffectSE, transform);
        if(!GetComponent<VisualEffect>()) Destroy(gameObject);
        vfx = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if(vfx.aliveParticleCount == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
