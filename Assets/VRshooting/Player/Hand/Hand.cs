using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private bool _Left;
    public bool Left { get => _Left; }

    [SerializeField]
    private GameObject _UseGun;

    [SerializeField]
    private List<MainGun> _GunsCs;

    [SerializeField]
    private int _NextNum = -1;
    
    private float _GunChangeTimer = 0f;
    private float _GunChangeTime = 0f;

    #region//VR操作
    private InputActionAsset _IAA;
    private InputAction _GunChangeAct;
    #endregion

    [Space(30)]
    [SerializeField]
    private Animator _HandAni;

    private void Start()
    {
        _IAA = GM.instance.Player.GetComponent<InputActionManager>().actionAssets[0];

        if (_Left)
        {
            GM.instance.LeftHand = gameObject;
            _GunChangeAct = _IAA.FindActionMap("GunAction L").FindAction("GunChange");
        }
        else
        {
            GM.instance.RightHand = gameObject;
            _GunChangeAct = _IAA.FindActionMap("GunAction R").FindAction("GunChange");
        }

        if (GM.instance.LeftMain == _Left) _SetGun(); //開始時に銃生成【テスト】
    }

    private void Update()
    {
        if(GM.instance.LeftMain == _Left) //メイン(左利き手＝＝このオブジェクトが左手)のとき
        {
            
            if(_GunChangeAct.WasPressedThisFrame() && !_GunsCs[GM.instance.UseGun].Reloading && _NextNum < 0)
            {
                _GunChangeStart(_SelectGun());
            }
        }
        else //サブ
        {

        }

        if (_GunsCs.Count > GM.instance.UseGun)
        {
            _UseGun = _GunsCs[GM.instance.UseGun].gameObject;
            _GunDitherInUpdate();
            _GunDitherOutUpdate();
            if (_HandAni)
            {
                _HandAni.gameObject.transform.rotation = _UseGun.transform.rotation;
            }
        }
        else _UseGun = null;

        if (_HandAni) _HandAni.SetBool("Grip", GM.instance.LeftMain == _Left);
    }

    /// <summary>
    /// 手の位置に銃を生成
    /// </summary>
    /// <param name="Gun"></param>
    private void _SetGun(GameObject Gun = null)
    {
        _GunsCs = new List<MainGun>();
        for(int i = 0;i < GM.instance.SetMainGun.Count;i++)
        {
            GameObject gun = Instantiate(GM.instance.SetMainGun[i], transform.position, transform.rotation, GM.instance.Player.transform);
            if (!gun.GetComponent<MainGun>()) Debug.LogError("MainGunCsが銃オブジェクトにありません");
            gun.GetComponent<MainGun>().InitalSetting(transform, _Left);
            gun.gameObject.SetActive(false);
            _GunsCs.Add(gun.GetComponent<MainGun>());
        }

        _GunChange(GM.instance.UseGun);

        /*
        GameObject gun = Instantiate(Gun, transform.position, transform.rotation, GM.instance.Player.transform);
        if (gun.GetComponent<MainGun>())
        {
            gun.GetComponent<MainGun>().InitalSetting(transform, _Left);
        }
        _Gun = gun;
        Debug.Log("武器装着");
        */
    }

    private void _GunDitherOutUpdate()
    {
        if (_NextNum < 0) return;

        ControlMat ctrlMat = _UseGun.GetComponent<ControlMat>();
        
        if(_GunChangeTimer > 0)
        {
            _GunChangeTimer -= Time.deltaTime;
            ctrlMat.SetDither(_GunChangeTimer / _GunChangeTime);
        }
        else
        {
            _GunChange(_NextNum);
        }
    }

    private void _GunDitherInUpdate()
    {
        if (_NextNum >= 0) return;
        if (!_UseGun.GetComponent<ControlMat>()) return;

        ControlMat ctrlMat = _UseGun.GetComponent<ControlMat>();

        if (ctrlMat.dither >= 1f) return;

        ctrlMat.SetDither(ctrlMat.dither + (Time.deltaTime / 0.5f));
    }

    private void _GunChange(int num)
    {
        _NextNum = -1;
        _GunsCs[GM.instance.UseGun].gameObject.SetActive(false);
        _GunsCs[num].gameObject.transform.rotation = transform.rotation;
        _GunsCs[num].gameObject.transform.position = transform.position;
        _GunsCs[num].gameObject.SetActive(true);
        _GunsCs[num].GunChangeSetting(false);
        GM.instance.UseGun = num;
    }

    private void _GunChangeStart(int gunNum)
    {
        if (_GunsCs.Count < gunNum) return;
        if (gunNum == GM.instance.UseGun) return;

        Debug.Log("銃切り替え : " + gunNum);

        _NextNum = gunNum;

        if (_UseGun.GetComponent<ControlMat>()) //dither処理できる場合
        {
            MainGun GunCs = _GunsCs[GM.instance.UseGun];
            GunCs.GunChangeSetting(true);
            _GunChangeTimer = GunCs.GunChangeTime;
            _GunChangeTime = GunCs.GunChangeTime;
        }
        else //できない場合
        {
            _GunChange(_NextNum);
        }
    }

    private int _SelectGun()
    {
        if (_GunsCs.Count != 2) Debug.LogError("銃は2種類までにして！【テスト段階】");

        if (GM.instance.UseGun == 0) return 1;
        else return 0;
    }
}
