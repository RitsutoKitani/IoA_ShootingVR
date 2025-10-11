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

    [SerializeField, ReadOnly][Header("使用中の銃")] private GameObject _UseGun;
    private MainGun _UseGunCs;

    [SerializeField, ReadOnly][Header("切り替え先の武器")] private int _NextNum = -1;
    
    private float _GunChangeTimer = 0f;
    private float _GunChangeTime = 0f;

    #region//VR操作
    private InputActionAsset _IAA;
    private InputAction _GunChangeAct;
    public InputAction GunChangeAct { get => _GunChangeAct; }
    #endregion

    [Space(30)]
    private HandUI _HandUI;
    [SerializeField]
    private Animator _HandAni;

    private void Start()
    {
        _IAA = StageManager.instance.IAA;
        _HandUI = GetComponent<HandUI>();

        if (_Left)
        {
            StageManager.instance.LeftHand = gameObject;
            _GunChangeAct = _IAA.FindActionMap("GunAction L").FindAction("GunChange");
        }
        else
        {
            StageManager.instance.RightHand = gameObject;
            _GunChangeAct = _IAA.FindActionMap("GunAction R").FindAction("GunChange");
        }
    }

    private void Update()
    {
        if (GM.instance.SetMainGunsCs.Count <= 0 || GM.instance.IsPose) return;

        _UseGun = GM.instance.SetMainGunsCs[GM.instance.UseGun].gameObject;
        _UseGunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];

        if (GM.instance.LeftMain == _Left) //メイン(左利き手＝＝このオブジェクトが左手)のとき
        {
            _GunDitherInUpdate();
            _GunDitherOutUpdate();

            if (_GunChangeAct.WasPressedThisFrame() && !_UseGunCs.Reloading && _NextNum < 0)
            {
                GunChangeStart(_SelectGun());
            }
        }
        else //サブ
        {
            
        }

        if (_HandAni)
        {
            _HandAni.SetBool("GunGrip", GM.instance.LeftMain == _Left && GM.instance.SetMainGunsCs.Count > 0);
            _HandAni.SetBool("Grip", (GM.instance.LeftMain != _Left) && (_UseGunCs.SubHand));
        }

        if (_HandUI) _HandUI.Active = GM.instance.LeftMain != _Left && !_UseGunCs.SubHand || GM.instance.SetMainGunsCs.Count == 0;
    }

    private void LateUpdate()
    {
        if (!_HandAni) return;

        Transform HandTra = _HandAni.gameObject.transform;
        if (!_UseGunCs || !_UseGun)
        {
            if (HandTra.parent != transform) HandTra.parent = transform;
            return;
        }

        HandTra.localPosition = Vector3.zero;
        //HandTra.localRotation = Quaternion.Euler(0, 0, 0);

        if (GM.instance.LeftMain == _Left)
        {
            if(GM.instance.SetMainGunsCs.Count > 0) HandTra.rotation = _UseGun.transform.rotation;
        }
        else
        {
            if (_UseGunCs.SubHand && _UseGunCs.SubHandle)
            {
                HandTra.parent = _UseGunCs.SubHandle.transform;
                //HandTra.position = _UseGunCs.SubHandle.position;
                //HandTra.rotation = _UseGunCs.SubHandle.rotation;
            }
            else HandTra.parent = transform;

        }
    }

    public void StageStartSetting()
    {
        if (GM.instance.LeftMain == _Left) _SetGun();
    }

    /// <summary>
    /// 手の位置に銃を生成
    /// </summary>
    private void _SetGun()
    {
        if (GM.instance.SetMainGunsCs.Count > 0)
        {
            for (int i = 0; i < GM.instance.SetMainGunsCs.Count; i++)
            {
                if (GM.instance.SetMainGunsCs[i]) Destroy(GM.instance.SetMainGunsCs[i].gameObject);
            }
        }

        GM.instance.SetMainGunsCs = new List<MainGun>();
        for(int i = 0;i < GM.instance.SetMainGun.Count;i++)
        {
            GameObject gun = Instantiate(GM.instance.SetMainGun[i].GunObj, transform.position, transform.rotation, StageManager.instance.Player.transform);
            gun.GetComponent<MainGun>().InitalSetting(transform, _Left);
            gun.GetComponent<ControlMat>().SetDither(0f);
            gun.gameObject.SetActive(false);
            GM.instance.SetMainGunsCs.Add(gun.GetComponent<MainGun>());
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

    /// <summary>
    /// 武器が消える
    /// </summary>
    private void _GunDitherOutUpdate()
    {
        if (_NextNum == -1) return;

        ControlMat ctrlMat = _UseGun.GetComponent<ControlMat>();
        
        if(_GunChangeTimer > 0)
        {
            _GunChangeTimer -= Time.deltaTime;
            ctrlMat.SetDither(_GunChangeTimer / _GunChangeTime);
        }
        else
        {
            if (_NextNum == -2) _GunRemove();
            else _GunChange(_NextNum);
        }
    }

    /// <summary>
    /// 武器が現れる
    /// </summary>
    private void _GunDitherInUpdate()
    {
        if (_NextNum != -1) return;
        if (!_UseGun.GetComponent<ControlMat>()) return;

        ControlMat ctrlMat = _UseGun.GetComponent<ControlMat>();

        if (ctrlMat.dither >= 1f) return;

        ctrlMat.SetDither(ctrlMat.dither + (Time.deltaTime / 0.5f));
    }

    /// <summary>
    /// 銃オブジェのアクティブ切り替え
    /// </summary>
    /// <param name="num"></param>
    private void _GunChange(int num)
    {
        _NextNum = -1;
        GM.instance.SetMainGunsCs[GM.instance.UseGun].gameObject.SetActive(false); //しまう武器を非アクティブに
        GM.instance.SetMainGunsCs[num].gameObject.transform.rotation = transform.rotation; //取り出す武器の位置・回転を手に同期
        GM.instance.SetMainGunsCs[num].gameObject.transform.position = transform.position;
        GM.instance.SetMainGunsCs[num].gameObject.SetActive(true); //取り出す武器をアクティブに
        GM.instance.SetMainGunsCs[num].GunChangeSetting(false);
        GM.instance.UseGun = num;
    }
    /// <summary>
    /// 武器を全て非アクティブ、削除する
    /// </summary>
    private void _GunRemove(bool destroy = false)
    {
        foreach(MainGun gunCs in GM.instance.SetMainGunsCs)
        {
            if(!destroy) gunCs.gameObject.SetActive(false);
            else Destroy(gunCs.gameObject);

        }

        GM.instance.SetMainGunsCs = new List<MainGun>(0);
        _HandAni.gameObject.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 武器を切り替える（負の数の場合は武器を削除）
    /// </summary>
    /// <param name="gunNum">切り替え先の武器番号</param>
    public void GunChangeStart(int gunNum)
    {
        if (GM.instance.SetMainGunsCs.Count < gunNum) return; //範囲外だったら
        if (gunNum == GM.instance.UseGun) return; //同じ武器（番号）の指定だったら

        Debug.Log("銃切り替え : " + gunNum);

        if(_UseGunCs.Reloading) _UseGunCs.ReloadCancel(); //リロードを中止

        if (gunNum < 0)//武器削除の場合
        {
            GM.instance.SetMainGunsCs[GM.instance.UseGun].GunChangeSetting(true);
            _NextNum = -2;
            return;
        }
        
        
        _NextNum = gunNum;
        if (_UseGun.GetComponent<ControlMat>()) //dither処理できる場合
        {
            MainGun GunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];
            GunCs.GunChangeSetting(true);
            _GunChangeTimer = GunCs.GunChangeTime;
            _GunChangeTime = GunCs.GunChangeTime;
        }
        else //できない場合すぐに
        {
            _GunChange(_NextNum);
        }
    }

    private int _SelectGun()
    {
        if (GM.instance.UseGun == 0) return 1;
        else return 0;
    }
}
