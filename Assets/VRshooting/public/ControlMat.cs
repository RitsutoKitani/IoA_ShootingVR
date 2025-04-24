using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMat : MonoBehaviour
{

    [SerializeField]
    private SkinnedMeshRenderer _smr;
    private Material[] materials;

    [SerializeField, Range(0f, 1f)]
    private float _dither = 1f;
    
    [SerializeField]
    [Header("‰Šú‚Édither‚ðƒ[ƒ‚É")] private bool _FirstDitherZero;

    [SerializeField, Range(0f, 1f)]
    private float _flash = 0f;

    private void Awake()
    {
        if (_smr) materials = _smr.materials;
        else materials = new Material[0];

        if (materials.Length > 0 && _FirstDitherZero)
        {
            foreach (var mat in materials) mat.SetFloat("_dither", 0f);
        }
    }

    private void Update()
    {
        if (materials.Length <= 0) return;
        foreach (Material mat in materials)
        {
            mat.SetFloat("_dither", _dither);
            mat.SetFloat("_flash", _flash);
        }
    }
}
