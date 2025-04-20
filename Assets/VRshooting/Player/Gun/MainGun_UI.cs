using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGun_UI : MonoBehaviour
{
    [SerializeField]
    private MainGun _GunCs;

    [Space(30)]

    [SerializeField]
    private Text _BulletText;

    [SerializeField]
    private SkinnedMeshRenderer _MR;
    private Material[] mats;

    [SerializeField]
    private float _matFillMax;
    [SerializeField]
    private float _matFillMin;
    [SerializeField]
    private float _alpha;
    [SerializeField]
    private float _flash;

    private void Start()
    {
        if (!_MR) return;
        mats = _MR.materials;
        foreach (var mat in mats)
        {
            mat.SetFloat("_FillMax", _matFillMax);
            mat.SetFloat("_FillMin", _matFillMin);
        }
    }

    private void Update()
    {
        if (!_GunCs) return;

        _BulletText.text = string.Format("{0:00}/{1:00}", _GunCs.MagazineBullet, _GunCs.MagazineBulletMax);

        if(!_MR) return;

        foreach(Material mat in mats)
        {
            mat.SetFloat("_fill", _GunCs.ReloadTimer / _GunCs.ReloadTime);
            mat.SetFloat("_alpha", _alpha);
            mat.SetFloat("_flash", _flash);
        }
    }
}
