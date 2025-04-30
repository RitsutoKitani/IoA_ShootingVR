using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class MainGun : MonoBehaviour
{
    [SerializeField]
    [Header("メイン手")] private Transform _MainHand;
    [SerializeField]
    [Header("サブ手")] private Transform _SubHand;
    public Transform SubHand { get => _SubHand; }

    [Space(30)]

    [SerializeField]
    private bool _CanShot = true;
    public bool CanShot { get =>  _CanShot; }

    [SerializeField]
    [Header("弾")] private GameObject _Bullet;
    [SerializeField]
    [Header("発射位置")] private Transform _ShotPos;
    [SerializeField]
    [Header("サブ持ち手")] private Transform _SubHandle;
    public Transform SubHandle { get => _SubHandle; }

    [Space(10)]

    [SerializeField]
    [Header("弾ステータス")] private BulletStatus _BulletStatus;

    [SerializeField]
    [Header("マガジン最大弾数")] private int _MagazineBulletMax;
    public int MagazineBulletMax { get => _MagazineBulletMax; }

    [SerializeField]
    [Header("マガジン弾数")] private int _MagazineBullet;
    public int MagazineBullet { get => _MagazineBullet; }

    [SerializeField]
    [Header("連射間隔")] private float _Interval = 0f;
    private float _ShotTimer = 0f;

    [SerializeField]
    private bool _Reloading = false;
    public bool Reloading { get => _Reloading; }

    [SerializeField]
    [Header("リロード時間")] private float _ReloadTime;
    public float ReloadTime { get => _ReloadTime; }

    private float _ReloadTimer = 0f;
    public float ReloadTimer { get => _ReloadTimer; }

    [SerializeField]
    [Header("拡散角度（片手）")] private float _DiffAngleOH;

    [SerializeField]
    [Header("拡散角度（両手）")] private float _DiffAngleBH;
    

    [Space(30)]

    [SerializeField]
    [Header("照準UI距離")] private float _AimUIdistance;
    [SerializeField]
    [Header("照準UIサイズ[距離]")] AnimationCurve _AimUIsize;

    [SerializeField]
    [Header("ラインレンダラー")] private LineRenderer _Aimline;

    [SerializeField, Range(0f, 1f)]
    [Header("片手ブレ補正")] private float _OHstabi;
    [SerializeField, Range(0f, 1f)]
    [Header("両手ブレ補正")] private float _BHstabi;

    [SerializeField]
    [Header("武器切り替え時間")] private float _GunChangeTime;
    public float GunChangeTime { get => _GunChangeTime;}

    [Space(30)]
    [SerializeField] private GameObject _AimUI;

    #region//VR入力関連
    private bool _Left;
    private XRBaseController _MainXRBC;
    private XRBaseController _SubXRBC;
    private InputActionAsset _IAA;
    private InputAction _GunShotAct;
    private InputAction _GunReloadAct;
    #endregion

    private Animator _ani;
    private ControlMat _MatCtrl;

    [Space(30)]

    #region//リロードSE関連
    [SerializeField] private SErepeat _rSEcs;
    [SerializeField] private AudioClip _ReloadingSE;
    [SerializeField] private AnimationCurve _ReloadingVolume;
    [SerializeField] private AudioClip _ReloadFinishSE;
    #endregion

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _IAA = StageManager.instance.IAA;

        if (_Left)
        {
            _GunShotAct = _IAA.FindActionMap("GunAction L").FindAction("GunShot");
            _GunReloadAct = _IAA.FindActionMap("GunAction L").FindAction("GunReload");
        }
        else
        {
            _GunShotAct = _IAA.FindActionMap("GunAction R").FindAction("GunShot");
            _GunReloadAct = _IAA.FindActionMap("GunAction R").FindAction("GunReload");
        }
    }

    void Update()
    {
        if (!_MainHand)
        {
            Debug.Log("メイン手が設定されてませんでした");
            Destroy(gameObject);
            return;
        }

        if(CanShot) _ShotUpdate();

        _IndicatorUpdate();

        transform.position = _MainHand.position;

        Quaternion AimRot = _MainHand.rotation;
        float Stabi = _OHstabi;
        if (_SubHand)
        {
            var aim = _SubHand.position - _MainHand.position;
            AimRot = Quaternion.LookRotation(aim);
            Stabi = _BHstabi;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, AimRot, Stabi);


        if (_ani) _ani.SetBool("Reloading", _Reloading);
        if (_GunReloadAct.WasPerformedThisFrame() && _MagazineBullet < _MagazineBulletMax && !_Reloading) _ReloadStart();
    }

    private void _ShotUpdate()
    {
        _rSEcs.isPlaying = _Reloading;
        if (_Reloading)
        {
            _rSEcs.Volume = _ReloadingVolume.Evaluate(_ReloadTimer / _ReloadTime);
            if (_ReloadTimer < _ReloadTime) _ReloadTimer += Time.deltaTime;
            else _ReloadFinish();
            return;
        }

        if (_ShotTimer < _Interval)
        {
            _ShotTimer += Time.deltaTime;
            return;
        }
        
        if (_GunShotAct.IsPressed()) _Shot();
    }

    private void _Shot()
    {
        if (!ObjPool.instance) {
            Debug.LogError("ObjectPoolスクリプトがありません"); return; }

        if (!_Bullet || !_ShotPos){
            Debug.LogError("弾か発射位置が設定されていません"); return; }



        if (_MagazineBullet <= 0) //弾ないのでリロードします
        {
            _ReloadStart();
            return;
        }

        float DiffAngle = 0f;
        if (_SubHand) DiffAngle = _DiffAngleBH;
        else DiffAngle = _DiffAngleOH;

        Vector2 ShotDiff = Vector2.zero;
        ShotDiff.x = Random.Range(-DiffAngle, DiffAngle);
        ShotDiff.y = Random.Range(-DiffAngle, DiffAngle);

        GameObject bullet = ObjPool.instance.MakeObj(ObjPool.instance.BulletPool, _ShotPos.position, _ShotPos.rotation); //弾生成
        bullet.transform.Rotate(transform.right, ShotDiff.y);
        bullet.transform.Rotate(transform.up, ShotDiff.x);
        bullet.GetComponent<Bullet>().ReStatus(_BulletStatus);

        _ShotTimer = 0f;
        _MagazineBullet--;

        _MainXRBC.SendHapticImpulse(0.6f, 0.05f); //持ち手に振動
        if (_SubHand) _SubXRBC.SendHapticImpulse(0.6f, 0.05f); //反対の手にも振動
        if (_ani) _ani.SetTrigger("Shot");
    }

    private void _ReloadStart()
    {
        if(!_CanShot) return;

        _Reloading = true;
        _ReloadTimer = 0f;
        _rSEcs.clip = _ReloadingSE;
    }

    private void _ReloadFinish()
    {
        _Reloading = false;
        _ShotTimer = _Interval;
        _MagazineBullet = _MagazineBulletMax;
        GM.instance.PlayOneSE(_ReloadFinishSE, transform);
    }

    private void _IndicatorUpdate()
    {
        if (!_ShotPos) return;

        RaycastHit hit;
        bool ishit = Physics.SphereCast(_ShotPos.position,0.2f, _ShotPos.transform.forward, out hit, Mathf.Infinity, 1 << 6| 1 << 10);

        if (_Aimline)
        {
            _Aimline.SetPosition(0, _ShotPos.position);
            _Aimline.SetPosition(1, _ShotPos.position + _ShotPos.forward * 3f);
        }

        if (_AimUI)
        {
            float UIdis = hit.distance;
            if (hit.distance > _AimUIdistance || !ishit) UIdis = _AimUIdistance;
            _AimUI.transform.localScale = Vector3.one * _AimUIsize.Evaluate(UIdis);
            _AimUI.transform.localPosition = new Vector3(0, 0, UIdis);
        }
    }

    /// <summary>
    /// 銃の初期設定
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="Left"></param>
    public void InitalSetting(Transform hand, bool Left)
    {
        _MainHand = hand;
        _Left = Left;
        if (Left)
        {
            _MainXRBC = StageManager.instance.LeftHand.GetComponent<XRBaseController>();
            _SubXRBC = StageManager.instance.RightHand.GetComponent<XRBaseController>();
        }
        else
        {
            _MainXRBC = StageManager.instance.RightHand.GetComponent<XRBaseController>();
            _SubXRBC = StageManager.instance.LeftHand.GetComponent<XRBaseController>();
        }
        
    }

    /// <summary>
    /// 武器切り替え時の設定。出現させる場合は「InRot」に手の位置情報を入れてください
    /// </summary>
    /// <param name="InRot"></param>
    public void GunChangeSetting(bool Out)
    {
        if (Out)
        {
            _CanShot = false;
            _SubHand = null;
        }
        else
        {
            _CanShot = true;
        }
    }

    public void HoldSubHand(Transform subhand = null)
    {
        _SubHand = subhand;
    }
}
