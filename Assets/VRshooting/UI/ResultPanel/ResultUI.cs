using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("テキスト（スコア）")][SerializeField] private Text _ScoreText;
    [SerializeField, Range(0,1f)] private float _TextToScore;

    [Header("ゲージ（スコア）")][SerializeField] private Image _ScoreGage;
    [SerializeField, Range(0, 1f)] private float _GageToScore;

    [Header("評価（A、Sなど）テキスト")][SerializeField] private Text _RankText;

    [Space(30)]
    [SerializeField] private RankingScore _RankUI;
    [SerializeField] private KeyBoard _keyBoardUI;

    private int _rank = -1; //順位
    private ScoreData _ClearData;

    private Animator _ani;

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!StageManager.instance) return;

        _TextUpdate();
        _GageUpdate();
    }

    private void _TextUpdate()
    {
        if(!_ScoreText) return;

        int score = Mathf.FloorToInt(StageManager.instance.Score * _TextToScore);
        _ScoreText.text = $"SCORE : {score:D6}";
    }

    private void _GageUpdate()
    {
        if(!_ScoreGage) return;

        float score = StageManager.instance.Score * _GageToScore;
        _ScoreGage.fillAmount = score / StageManager.instance.LankScore[StageManager.instance.LankScore.Length - 1];

        if(!_RankText) return;

        _RankText.text = "C";
        if (StageManager.instance.LankScore[0] <= score) _RankText.text = "B";
        if (StageManager.instance.LankScore[1] <= score) _RankText.text = "A";
        if (StageManager.instance.LankScore[2] <= score) _RankText.text = "S";

    }

    public void ResultCheck()
    {
        AnimatorStateInfo state = _ani.GetCurrentAnimatorStateInfo(0);
        if(state.normalizedTime < 1f) //アニメーション再生中であればスキップ
        {
            _ani.Play(state.fullPathHash, 0, 0.999f);
            _ani.Update(0f);
            return;
        }

        if (_rank < 0 || !_RankUI) _ani.SetTrigger("RetryBack"); //ランキング外の場合
        else //ランキング内の場合
        {
            _RankUI.InitializeByData(_ClearData, _rank);
            _ani.SetTrigger("Ranking");
        }
    }

    /// <summary>
    /// 入力した名前でランキングに追加
    /// </summary>
    public void RankingDataSet()
    {
        _ClearData.Name = _keyBoardUI.TextData;
        DataManager.instance.AddData(_ClearData);
        _ani.SetTrigger("RetryBack");
    }

    public void ResultStart(ScoreData data, int rank = -1)
    {
        _ClearData = data;
        _rank = rank;
        _ani.SetTrigger("Result");
    }
}
