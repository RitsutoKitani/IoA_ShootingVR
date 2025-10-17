using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMmanager : MonoBehaviour
{
    [SerializeField] private AudioSource _MeloAS;
    [SerializeField, Range(0f, 1f)] private float _MeloVol;

    [Space(20)]
    [SerializeField] private AudioSource _ParcAS;
    [SerializeField, Range(0f, 1f)] private float _ParcVol;

    [Space(20)]
    [SerializeField] private AudioSource _ClearAS;
    [SerializeField, Range(0f, 1f)] private float _ClearVol;

    private Animator _BGMani;
    private int _step = 0;

    private void Start()
    {
        _BGMani = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_MeloAS) _MeloAS.volume = _MeloVol * GM.instance.BGMvol;
        if(_ParcAS) _ParcAS.volume = _ParcVol * GM.instance.BGMvol;
        if(_ClearAS) _ClearAS.volume = _ClearVol * GM.instance.BGMvol;

        if (_BGMani) _BGMani.SetInteger("Step", _step);

        if (_step == 0 && StageManager.instance.StageTimer > 0.5f) _step = 1;
        if (_step == 1 && StageManager.instance.StageTimer > StageManager.instance.TutorialFinTime) _step = 2;
    }

    public void ClearBGM()
    {
        _step = 3;
    }
}
