using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    [Header("サイズ倍率")] private float[] _Size = new float[2];
    [SerializeField]
    [Header("ヒット音")] private AudioClip _HitSE;

    private AudioSource _as;
    private Animator _ani;

    private void Start()
    {
        float size = Random.Range(_Size[0], _Size[1]);
        transform.localScale = new Vector3(size, size, size);

        if (GetComponent<AudioSource>()) _as = GetComponent<AudioSource>();
        _ani = GetComponent<Animator>();
    }

    public void Hit()
    {
        if (_as && _HitSE) _as.PlayOneShot(_HitSE);
        GetComponent<Collider>().enabled = false;
        if(_ani) _ani.SetTrigger("Hit");
        else Destroy(gameObject);
    }

    private void _Vanish()
    {
        Destroy(gameObject);
    }
}
