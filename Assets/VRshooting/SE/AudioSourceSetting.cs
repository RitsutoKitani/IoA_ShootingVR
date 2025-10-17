using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSetting : MonoBehaviour
{
    [SerializeField] private AudioSource[] _AS;
    private float[] _DefoVol;
    [SerializeField] private AudioType _Type;
    float _Fade = 0f;

    [Space(20)]
    [SerializeField] private Vector2 _PitchRandmize = new Vector2(1f,1f);

    private enum AudioType
    {
        SE,
        BGM
    }

    private void OnEnable()
    {
        _Fade = 0f;
        foreach(var AS in _AS) AS.pitch = Random.Range(_PitchRandmize.x, _PitchRandmize.y);
    }

    private void Start()
    {
        _DefoVol = new float[_AS.Length];
        for(int i = 0; i < _AS.Length; i++)
        {
            _DefoVol[i] = _AS[i].volume;
        }
    }

    private void Update()
    {
        if(!GM.instance || _AS.Length == 0) return;

        float volume = _Type == AudioType.SE ? GM.instance.SEvol : GM.instance.BGMvol;
        for(int i = 0; i < _AS.Length; i++)
        {
            _AS[i].volume = _DefoVol[i] * volume * _Fade;
        }

        if (_Fade < 1f) _Fade += Time.deltaTime;
        else _Fade = 1f;
    }
}
