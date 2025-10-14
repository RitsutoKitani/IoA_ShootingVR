using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GuardBarrier : MonoBehaviour
{
    [SerializeField] private bool _Guard;
    public bool Guard { get => _Guard; }

    [Space (30)]
    [SerializeField][Header("ガード可能時間")] private float _GuardTimeMax;
    [SerializeField, ReadOnly] private float _GuardTimer;
    [SerializeField][Header("ゲージ回復速度")] private float _GageUpSpeed;
    [SerializeField, ReadOnly]private bool _GuardLock = false;

    [Space(30)]
    [SerializeField] private Color[] _GageColor = new Color[2];

    [Space(30)]

    [SerializeField] private ToggleButton _toggle;
    [SerializeField] private Image _Gage; //ゲージ

    [Space(30)]
    [SerializeField] private AudioSource _GuardAS;


    private InputAction _GuardAct;

    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _GuardTimer = _GuardTimeMax;

        if (StageManager.instance)
        {
            string ActionMap = GM.instance.LeftMain ? "GunAction R" : "GunAction L";
            _GuardAct = StageManager.instance.IAA.FindActionMap(ActionMap).FindAction("Guard");
        }
    }

    private void Update()
    {
        if (GM.instance.IsPose) return;

        _Guard = (_toggle.Toggle || _GuardAct.IsPressed()) && !_GuardLock;
        _toggle.Interactable = !_GuardLock && !GM.instance.IsPose && StageManager.instance.StageActive;
        _ani.SetBool("Guard", _Guard);
        if (_Gage)
        {
            _Gage.fillAmount = _GuardTimer / _GuardTimeMax;
            if (_GuardLock) _Gage.color = _GageColor[0];
            else _Gage.color = _GageColor[1];
        }

        if(_Guard)
        {
            if(_GuardAS && !_GuardAS.isPlaying) _GuardAS.Play();

            if (_GuardTimer > 0) _GuardTimer -= Time.deltaTime;
            else
            {
                _GuardLock = true;
                StageManager.instance.HandUnlock();
                _toggle.ToggleSet(false);
            }
        }
        else
        {
            if(_GuardAS && _GuardAS.isPlaying) _GuardAS.Stop();

            if (_GuardTimer < _GuardTimeMax) _GuardTimer += Time.deltaTime * StageManager.instance.TimeScale * _GageUpSpeed;
            if (_GuardLock && _GuardTimer / _GuardTimeMax > 0.5f) _GuardLock = false;
        }
        
    }
    public void GuardDamage(int damage, float pene = 0.0f)
    {
        int dmg = Mathf.FloorToInt(damage * pene);
        if (dmg <= 0) return;
        StageManager.instance.DomeCs.Damage(dmg);
    }
}
