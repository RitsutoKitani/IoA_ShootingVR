using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMat : MonoBehaviour
{

    [SerializeField]
    private SkinnedMeshRenderer[] _smr;
    [SerializeField] private MeshRenderer[] _mr;
    [SerializeField, ReadOnly] private List<Material> _mats;

    [SerializeField, Range(0f, 1f)]
    private float _dither = 1f;
    public float dither { get => _dither; }
    
    [SerializeField]
    [Header("‰Šú‚Édither‚ğƒ[ƒ‚É")] private bool _FirstDitherZero;

    [SerializeField, Range(0f, 1f)]
    private float _flash = 0f;

    private void Start()
    {
        _mats = new List<Material>();
        if (_smr.Length > 0)
        {
            foreach (var smr in _smr) _mats.AddRange(smr.materials);
        }
        if (_mr.Length > 0)
        {
            foreach (var mr in _mr) _mats.AddRange(mr.materials);
        }

        if (_mats.Count > 0 && _FirstDitherZero)
        {
            foreach (var mat in _mats) mat.SetFloat("_dither", 0f);
        }
    }

    private void Update()
    {
        if (_mats.Count <= 0) return;
        foreach (Material mat in _mats)
        {
            mat.SetFloat("_dither", _dither);
            mat.SetFloat("_flash", _flash);
        }
    }

    public void SetDither(float num)
    {
        _dither = num;
    }
}
