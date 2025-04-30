using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SErepeat : MonoBehaviour
{
    [SerializeField]
    private AudioSource _as;

    public AudioClip clip;
    public bool isPlaying;

    [SerializeField, Range(0f,1f)]
    public float Volume;



    private void Update()
    {
        if (!_as) return;
        if (isPlaying && !_as.isPlaying) _as.Play();
        if(!isPlaying &&  _as.isPlaying) _as.Stop();
        if(_as.clip != clip) _as.clip = clip;
        _as.volume = Volume * GM.instance.SEvol;
    }
}
