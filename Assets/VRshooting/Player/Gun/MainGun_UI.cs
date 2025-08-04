using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGun_UI : MonoBehaviour
{
    [SerializeField] private MainGun _GunCs;

    [SerializeField][Header("ÉJÉÅÉâï˚å¸Çå¸Ç≠")] private bool _LookCamera;

    [SerializeField] private float _LookSmooth;

    [Space(30)]

    [SerializeField]
    private Text _BulletText;

    [Space(30)]

    [SerializeField] private SkinnedMeshRenderer _MR;

    private Material[] mats;

    [SerializeField] private float _matFillMax;
    [SerializeField] private float _matFillMin;

    [SerializeField, Range(0,1)] private float _alpha;
    [SerializeField, Range(0, 1)] private float _flash;

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

        _BulletText.text = string.Format("{0:00}/{1:00}", _GunCs.MagazineBullet, _GunCs.Data.MagazineBulletMax);

        if (_LookCamera)
        {
            var aim = transform.position - Camera.main.transform.position;
            Quaternion AimRot = Quaternion.LookRotation(aim);
            transform.rotation = Quaternion.Slerp(transform.rotation, AimRot, _LookSmooth);
        }

        if(!_MR) return;

        foreach(Material mat in mats)
        {
            mat.SetFloat("_fill", _GunCs.ReloadTimer / _GunCs.Data.ReloadTime);
            mat.SetFloat("_alpha", _alpha);
            mat.SetFloat("_flash", _flash);
        }
    }
}
