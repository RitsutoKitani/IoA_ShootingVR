using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : MonoBehaviour
{
    [SerializeField] private MainGun _GunCs;
    [SerializeField] private Text _BulletText;
    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_GunCs) return;

        _ani.SetBool("BothHand", _GunCs.SubHand);
        _ani.SetBool("NoAmmo", _GunCs.MagazineBullet <= 0);
        if (_BulletText) _BulletText.text = _GunCs.MagazineBullet.ToString("00");
    }
}
