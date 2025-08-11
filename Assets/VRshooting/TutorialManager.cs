using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool _Active;

    [Space(30)]
    [Tooltip("0: èeÇåÇÇ¬\n" +
    "1: èeÇóºéËÇ≈ç\Ç¶ÇÈ\n" +
    "2: ÉäÉçÅ[ÉhÇ∑ÇÈ\n" +
    "3: èeÇêÿÇËë÷Ç¶ÇÈ\n" +
    "4: ÉKÅ[ÉhÇ∑ÇÈ")]
    [SerializeField] private int _QuestNum;
    [SerializeField, ReadOnly] private bool _Clear;

    [Space(30)]
    [SerializeField, Multiline(3)] private string _HelpString;
    [SerializeField] private bool _SubController;
    [SerializeField] private Color[] _TextColor = new Color[2];
    [SerializeField] private LineRenderer _GuideLine;
    private Vector3[] _GuidePos = new Vector3[2];

    [Space(30)]
    [SerializeField] private List<TutorialEnemy> _Enemys = new List<TutorialEnemy>();

    [SerializeField] private float _SponeTime;

    [Space(30)]
    [SerializeField] private Animator _ani;
    [SerializeField] private Animator _ControllerAni;
    [SerializeField] private Text _helpText;
    [SerializeField] private Text _ControllerText;
    [SerializeField] private Transform _GuardButton;

    public void TutorialStart()
    {
        if (!_ani) return;

        _Active = true;
        _Clear = false;
        StageManager.instance.StageChangeActive(false);


        _ani.SetBool("Active", true);
        _ani.SetBool("Clear", false);
        _ani.SetBool("Guide", _GuideLine);

        if (_ControllerAni)
        {
            _ControllerAni.SetInteger("Angle", _QuestNum);
            _ControllerAni.SetBool("Left", GM.instance.LeftMain != _SubController);
        }

        if (_ControllerText)
        {
            if(GM.instance.LeftMain != _SubController)
            {
                _ControllerText.text = "L";
                _ControllerText.color = _TextColor[0];
            }
            else
            {
                _ControllerText.text = "R";
                _ControllerText.color= _TextColor[1];
            }
        }

        if (_helpText) _helpText.text = _HelpString; 

        foreach (TutorialEnemy enemy in _Enemys)
        {
            enemy.EneCs.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        _QuestUpadate();
        _EneSponeUpdate();
        _FinCheck();
    }

    private void _EneSponeUpdate()
    {
        if (_Clear || !_Active) return;

        foreach (TutorialEnemy enemy in _Enemys)
        {
            if (enemy.EneCs.gameObject.activeSelf) continue;

            enemy.SponeTimer += Time.deltaTime;
            if (enemy.SponeTimer > _SponeTime)
            {
                enemy.EneCs.ReSetting();
                enemy.EneCs.gameObject.SetActive(true);
                enemy.SponeTimer = 0;
            }
        }
    }

    private void _QuestUpadate()
    {
        if(_Clear || !_Active || GM.instance.SetMainGunsCs.Count < 3) return;

        MainGun GunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];
        GameObject MainHand = GM.instance.LeftMain ? StageManager.instance.LeftHand : StageManager.instance.RightHand;
        GameObject SubHand = GM.instance.LeftMain ? StageManager.instance.RightHand : StageManager.instance.LeftHand;

        if(_GuideLine)
        {
            _GuideLine.gameObject.transform.position = _GuidePos[0];
            _GuideLine.SetPositions(_GuidePos);
        }

        switch (_QuestNum)
        {
            case 0:
                if (GunCs.GunShotAct.WasPressedThisFrame()) _QuestClear();
                break;

            case 1:
                _GuidePos[0] = SubHand.transform.position;
                _GuidePos[1] = GM.instance.SetMainGunsCs[GM.instance.UseGun].SubHandle.position;
                if (GunCs.SubHand) _QuestClear();
                break;

            case 2:
                if (GunCs.GunReloadAct.WasPressedThisFrame()) _QuestClear();
                break;

            case 3:
                if (MainHand.GetComponent<Hand>().GunChangeAct.WasPressedThisFrame() &&
                    !GM.instance.SetMainGunsCs[GM.instance.UseGun].Reloading) _QuestClear();
                break;

            case 4:
                _GuidePos[0] = SubHand.transform.position;
                _GuidePos[1] = _GuardButton.position;
                if (StageManager.instance.GuardBarrier.Guard) _QuestClear();
                break;
        }
    }

    private void _QuestClear()
    {
        _Clear = true;
        _ani.SetBool("Clear", true);
        _ani.SetBool("Guide", false);
    }

    private void _FinCheck()
    {
        if(!_Clear || !_Active) return;

        bool eneCheck = false;
        foreach (TutorialEnemy ene in _Enemys)
        {
            if(ene.EneCs.gameObject.activeSelf) eneCheck = true;
        }

        if(! eneCheck)
        {
            _Active = false;
            if(_ani) _ani.SetBool("Active", false);
            StageManager.instance.StageChangeActive(true);
        }
    }

    [System.Serializable]
    public class TutorialEnemy
    {
        public Enemy EneCs;
        [ReadOnly] public float SponeTimer = 0f;
    }
}
