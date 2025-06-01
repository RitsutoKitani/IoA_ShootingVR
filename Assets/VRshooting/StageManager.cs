using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField] private int _Score;
    [SerializeField] private int[] _LankScore = new int[3];
    public int[] LankScore { get => _LankScore; }

    public int Score { get => _Score; }

    [SerializeField][Header("ステージが稼働中か")] private bool _StageActive = false;
    public bool StageActive { get => _StageActive; }

    [SerializeField][Header("ステージ全体時間")] private float _StageTime;
    [SerializeField, ReadOnly][Header("ステージ進行時間")] private float _StageTimer = 0f;
    private bool _finish = false;

    [SerializeField][Header("敵活動リスト")] private List<EnemyInfo> _Enemys = new List<EnemyInfo>();

    [SerializeField, ReadOnly][Header("活動中の敵")] private List<Enemy> _ActEnemys = new List<Enemy>();
    public List<Enemy> ActEnemy {  get => _ActEnemys; }

    [Space(30)]
    [SerializeField]
    private InputActionManager _IAM;
    [HideInInspector]
    public InputActionAsset IAA { get => _IAM.actionAssets[0]; }


    [Header("プレイヤー")] public GameObject Player;
    [Header("カメラ")] public GameObject CameraObj;

    [Header("右手オブジェクト")] public GameObject RightHand;
    [Header("左手オブジェクト")] public GameObject LeftHand;
    [Header("プレイヤードーム")] public PlayerDome DomeCs;


    [Space(30)]
    [SerializeField] private ResultUI _ResultCs;

    private void Awake()
    {
        instance= this;
    }

    private void Update()
    {
        if (!_StageActive || GM.instance.IsPose) return;
        _StageUpdate();
    }

    private void _StageUpdate()
    {
        _StageTimer += Time.deltaTime;
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

        for (int i = 0; i < _ActEnemys.Count; i++) //やられた敵はリストから除外
        {
            if (!_ActEnemys[i].gameObject || _ActEnemys[i].Hp <= 0 || !_ActEnemys[i].gameObject.activeSelf) _ActEnemys.RemoveAt(i);
        }

        if(_ActEnemys.Count > 0) EneCheck = true;

        if (_StageTimer > _StageTime && !EneCheck && !_finish)
        {
            StageFinish();
            _finish = true;
        }
    }

    public void StageStart()
    {
        _StageTimer = 0f;
        _StageActive = true;
        _finish = false;

        foreach (EnemyInfo enemy in _Enemys)
        {
            enemy.EneCs.ReSetting();
            enemy.sortie = false;
        }
    }

    public void StandByStart()
    {
        if(RightHand) RightHand.GetComponent<Hand>().StageStartSetting();
        if(LeftHand) LeftHand.GetComponent<Hand>().StageStartSetting();
        GM.instance.UIhandLeft = !GM.instance.LeftMain;

        if (_ResultCs) _ResultCs.GetComponent<Animator>().SetTrigger("Start");
        else StageStart();
    }

    public void ScoreGet(int score)
    {
        _Score += score;
    }

    public void StageFinish()
    {
        Debug.Log("ステージ終了！");
        if (_ResultCs) _ResultCs.ResultStart();
    }

    public void HandUnlock()
    {
        HandUI handUI = null;
        if (GM.instance.UIhandLeft) handUI = LeftHand.GetComponent<HandUI>();
        else handUI = RightHand.GetComponent<HandUI>();

        handUI.UnLock();
    }
}

[System.Serializable]

public class EnemyInfo
{
    public Enemy EneCs;
    [Header("活動開始時間")] public float ActiveTime;

    [HideInInspector]
    public bool sortie = false;
}
