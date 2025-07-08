using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AniClip : MonoBehaviour
{
    [SerializeField]
    [Header("アニメーションの開始位置をランダム化")] private bool _RandomStart;

    [SerializeField]
    private VisualEffect[] _vfx;
    private Animator _ani;

    private void Start()
    {
        if(GetComponent<Animator>()) _ani = GetComponent<Animator>();

        if(_ani && _RandomStart) _ani.Play(_ani.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0f, 1f));
    }

    private void PlaySE(AudioClip SE)
    {
        GM.instance.PlayOneSE(SE, transform);
    }

    private void StopActive()
    {
        gameObject.SetActive(false);
    }

    private void _VfxPlay(int num)
    {
        if (_vfx.Length <= num || !_vfx[num]) return;

        _vfx[num].Play();
    }
}