using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private Text _ScoreText;
    [SerializeField, Range(0,1f)] private float _TextToScore;

    [SerializeField] private Image _ScoreGage;
    [SerializeField, Range(0, 1f)] private float _GageToScore;

    [SerializeField] private Text _RankText;

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

    private void _StageStart()
    {
        StageManager.instance.StageStart();
    }

    public void ResultCheck()
    {
        _ani.SetTrigger("Check");
    }

    public void ResultStart()
    {
        _ani.SetTrigger("Result");
    }
}
