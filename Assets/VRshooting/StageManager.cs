using NUnit;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public float TimeScale = 1.0f;

    [SerializeField] private int _Score;
    [SerializeField] private int[] _LankScore = new int[3];
    public int[] LankScore { get => _LankScore; }

    public int Score { get => _Score; }
    private ScoreData _ClearData;
    public ScoreData ClearData { get => _ClearData; }
    private int _ClearRank = -1;
    public int ClearRank { get => _ClearRank; }

    [SerializeField][Header("ステージが稼働中か")] private bool _StageActive = false;
    public bool StageActive { get => _StageActive; }

    [SerializeField][Header("ステージ全体時間")] private float _StageTime;
    [SerializeField][Header("ステージ進行時間")] private float _StageTimer = 0.0f;
    public float StageTimer { get => _StageTimer; }
    private bool _finish = false;
    public bool finish { get => _finish; }

    [Space(30)]
    [SerializeField][Header("敵活動リスト")] private List<EnemyInfo> _Enemys = new List<EnemyInfo>();
    [SerializeField, ReadOnly][Header("活動中の敵")] private List<Enemy> _ActEnemys = new List<Enemy>();
    public List<Enemy> ActEnemy {  get => _ActEnemys; }
    [SerializeField][Header("イベントリスト")] private List<EventInfo> _Events = new List<EventInfo>();

    [Space(30)]
    [SerializeField][Header("チュートリアル終了時間")] private float _TutorialFinTime;
    public float TutorialFinTime { get => _TutorialFinTime; }

    [SerializeField] private UnityEvent _ClearEvent;

    [Space(30)]
    [SerializeField] private InputActionManager _IAM;
    [HideInInspector] public InputActionAsset IAA { get => _IAM.actionAssets[0]; }
    private InputAction _PoseAct;
    [SerializeField] private FadeControl _Fade;
    public FadeControl Fade { get => _Fade; }

    [Header("プレイヤー")] public GameObject Player;

    [Space(30)]
    [Header("右手オブジェクト")] public GameObject RightHand;
    [Header("左手オブジェクト")] public GameObject LeftHand;
    [Header("ガードバリア")] public GuardBarrier GuardBarrier;
    [Header("プレイヤードーム")] public PlayerDome DomeCs;


    [Space(30)]
    [SerializeField] private ResultUI _ResultCs;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        if (IAA)
        {
            string ActionMap = GM.instance.LeftMain ? "GunAction R" : "GunAction L";
            _PoseAct = IAA.FindActionMap(ActionMap).FindAction("Pose");
        }
    }

    private void Update()
    {
        if (_PoseAct.WasPressedThisFrame() && _Fade) _Fade.Pose();
        if (!_StageActive || GM.instance.IsPose) return;
        _StageUpdate();
    }

    private void _StageUpdate()
    {
        _StageTimer += Time.deltaTime * TimeScale;
        bool EneCheck = false;

        foreach (EnemyInfo enemy in _Enemys) //敵活動開始！
        {
            if (enemy.sortie) continue;
            EneCheck = true;
            if (_StageTimer < enemy.ActiveTime || !enemy.EneCs) continue;

            enemy.EneCs.gameObject.SetActive(true);
            _ActEnemys.Add(enemy.EneCs);
            enemy.sortie = true;
        }

        foreach (EventInfo eve in _Events)
        {
            if(eve.sortie || _StageTimer < eve.ActiveTime) continue;

            eve.Event.Invoke();
            eve.sortie = true;
        }

        for (int i = 0; i < _ActEnemys.Count; i++) //やられた敵はリストから除外
        {
            if (!_ActEnemys[i].gameObject || _ActEnemys[i].Hp <= 0 || !_ActEnemys[i].gameObject.activeSelf) _ActEnemys.RemoveAt(i);
        }

        if(_ActEnemys.Count > 0) EneCheck = true;

        if (_StageTimer > _StageTime && !EneCheck && !_finish) //ステージ時間を超えていて活動中の敵がいないとき
        {
            StageFinish();
            _finish = true;
        }
    }

    /// <summary>
    /// ステージを開始します
    /// </summary>
    public void StageStart()
    {
        if (RightHand) RightHand.GetComponent<Hand>().StageStartSetting();
        if (LeftHand) LeftHand.GetComponent<Hand>().StageStartSetting();
        GM.instance.UIhandLeft = !GM.instance.LeftMain;

        _StageTimer = 0f;
        _StageActive = true;
        _finish = false;

        foreach (EnemyInfo enemy in _Enemys)
        {
            enemy.EneCs.ReSetting();
            enemy.sortie = false;
        }

        if (GM.instance.TutorialSkip) StageSkip(_TutorialFinTime);
    }

    public void ScoreGet(int score)
    {
        _Score += score;
    }

    public void StageSkip(float time)
    {
        _StageTimer = time;
        foreach (EnemyInfo ene in _Enemys)
        {
            if (!ene.sortie && ene.ActiveTime <= time) ene.sortie = true;
        }
        foreach (EventInfo eve in _Events)
        {
            if (!eve.sortie && eve.ActiveTime <= time) eve.sortie = true;
        }
    }

    /// <summary>
    /// ステージを終了させるメソッドです
    /// </summary>
    public void StageFinish()
    {
        Debug.Log("ステージ終了！");

        _ClearEvent.Invoke();
        _StageActive = false;
        _ClearData = new ScoreData(_Score, "", GM.instance.SetMainGun[0].Name, GM.instance.SetMainGun[1].Name);
        ScoreData[] ranking = DataManager.instance.MainData.ScoreData;
        _ClearRank = -1;
        for (int i = 0; i < ranking.Length; i++)
        {
            if (_Score > ranking[i].Score)
            {
                _ClearRank = i;
                break;
            }
        }


        MainSubHand(false).GetComponent<Hand>().GunChangeStart(-1); //武器をしまう
        if (_ResultCs) _ResultCs.ResultStart(_ClearData, _ClearRank);
        Debug.Log("今回の順位は" + (_ClearRank < 0 ? "ランキング外でした   " : _ClearRank));
    }

    /// <summary>
    /// メインハンド、サブハンドを返します
    /// </summary>
    /// <param name="sub"></param>
    /// <returns></returns>
    public GameObject MainSubHand(bool sub = false)
    {
        return GM.instance.LeftMain == sub ? RightHand : LeftHand;
    }

    /// <summary>
    /// コントローラーを振動させます
    /// </summary>
    /// <param name="SubHand"></param>
    /// <param name="strength"></param>
    /// <param name="length"></param>

    public void MainSubHandVibe(bool SubHand = false, float strength = 1f, float length = 1f)
    {
        XRBaseController XRBC = (GM.instance.LeftMain == SubHand ? RightHand : LeftHand).GetComponent<XRBaseController>();

        XRBC.SendHapticImpulse(strength, length);
    }

    /// <summary>
    /// コントローラーを振動させます
    /// </summary>
    /// <param name="strength"></param>
    /// <param name="length"></param>
    public void UIHandVibe(float strength = 1f, float length = 1f)
    {
        XRBaseController XRBC = (GM.instance.UIhandLeft? LeftHand : RightHand).GetComponent<XRBaseController>();

        XRBC.SendHapticImpulse(strength, length);
    }

    /// <summary>
    /// ハンドUIのターゲットロックを解除します
    /// </summary>
    public void HandUnlock()
    {
        HandUI handUI = null;
        if (GM.instance.UIhandLeft) handUI = LeftHand.GetComponent<HandUI>();
        else handUI = RightHand.GetComponent<HandUI>();

        handUI.UnLock();
    }

    public void StageChangeActive(bool active)
    {
        _StageActive = active;
    }

    [System.Serializable]
    public class EnemyInfo
    {
        public Enemy EneCs;
        [Header("活動開始時間")] public float ActiveTime;

        [HideInInspector]
        public bool sortie = false;
    }

    [System.Serializable]
    public class EventInfo
    {
        public UnityEvent Event;
        [Header("発動時間")] public float ActiveTime;

        [HideInInspector]
        public bool sortie = false;
    }

}

