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
    private MeshRenderer _MR;

    private void Update()
    {
        if (!_GunCs) return;

        _BulletText.text = string.Format("{0:00}/{1:00}", _GunCs.MagazineBullet, _GunCs.MagazineBulletMax);
    }
}
