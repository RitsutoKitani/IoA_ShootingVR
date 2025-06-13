using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool _Active;

    [Space(30)]
    [Tooltip("0: èeÇåÇÇ¬\n" +
    "1: èeÇóºéËÇ≈ç\Ç¶ÇÈ\n" +
    "2: èeÇêÿÇËë÷Ç¶ÇÈ\n" +
    "3: ÉKÅ[ÉhÇ∑ÇÈ")]
    [SerializeField] private int QuestNum;
    [SerializeField, ReadOnly] private bool _Clear;

    [Space(30)]
    [SerializeField] private List<TutorialEnemy> _Enemys;

    [SerializeField] private float _SponeTime;
    private List<float> _SponeTimer = new List<float>();

    public void TutorialStart()
    {
        _Active = true;
        _Clear = false;

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
            if (enemy.EneCs.gameObject.activeSelf) return;

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
        if(_Clear || !_Active) return;

        MainGun GunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];
        GameObject MainHand = GM.instance.LeftMain ? StageManager.instance.LeftHand : StageManager.instance.RightHand;
        GameObject SubHand = GM.instance.LeftMain ? StageManager.instance.RightHand : StageManager.instance.LeftHand;

        switch (QuestNum)
        {
            case 0:
                if (GunCs.GunShotAct.WasPressedThisFrame()) _Clear = true;
                break;

            case 1:
                if (GunCs.SubHand) _Clear = true;
                break;

            case 2:
                if (MainHand.GetComponent<Hand>().GunChangeAct.WasPressedThisFrame() &&
                    !GM.instance.SetMainGunsCs[GM.instance.UseGun].Reloading) _Clear = true;
                break;

            case 3:
                if (StageManager.instance.GuardBarrier.Guard) _Clear = true;
                break;
        }
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
