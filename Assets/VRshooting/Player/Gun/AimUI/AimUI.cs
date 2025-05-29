using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUI : MonoBehaviour
{
    [SerializeField] private MainGun _GunCs;
    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        _ani.SetBool("BothHand", _GunCs.SubHand);
        _ani.SetBool("NoAmmo", _GunCs.MagazineBullet <= 0);
    }
}
