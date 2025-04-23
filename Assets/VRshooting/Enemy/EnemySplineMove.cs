using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class EnemySplineMove : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline; //移動ルート
    [SerializeField] private Enemy _EneCs; //敵本体
    private GameObject _EneBody;

    [Space(20)]
    [SerializeField] [Header("移動速度")] private float _speed;
    [SerializeField] [Header("移動速度変化 [0-1]")]  private AnimationCurve _SpeedCurve;
    [SerializeField, Range(0,1)] [Header("スプライン上の位置 [0-1]")] private float _SplinePos = 0f;
    private float _SplineLength; //スプラインの長さ
    private Vector3 _OffsetPos;
    private Quaternion _OffsetRot;

    [SerializeField] private bool _Stop = false;
    [SerializeField] private float _StopTimer = 0f;
    [SerializeField] [Header("停止時減速時間")] private float _DecTime;
    [SerializeField] private float _DecTimer = 0f;
    private UnityEvent _StopEvent = null;
    private bool _EventPlay = false;

    [Space(20)]
    [SerializeField] [Header("向く方向")] private Transform _LookTarget = null;
    [SerializeField] [Header("回転補正")] private float _LookLerp;

    [Space(20)]
    [SerializeField] private List<StopAction> _Actions = new List<StopAction>();

    [Space(50)]
    [SerializeField] [Header("確認用マーカー表示")] private bool _TestGizmoPreview;
    [SerializeField, Range(0, 1)] [Header("確認用マーカーの位置（確認用）")] private float _TestSplinePos;

    private void Start()
    {
        _SplineLength = _spline.CalculateLength();
        _EneBody = _EneCs.gameObject;

        transform.parent = null;
        transform.position = _spline.EvaluatePosition(0);
        transform.rotation = Quaternion.LookRotation(_spline.EvaluateTangent(0));
        _OffsetPos = transform.InverseTransformPoint(_EneBody.transform.position);
    }

    private void Update()
    {
        if(!_EneBody.activeSelf) return;
        _LookUpdate();
        _MoveUpdate();

        if (_Stop) _StopUpdate();
        else
        {
            foreach(var action in _Actions)
            {
                if (action.StopStartPos < _SplinePos && !action.fin) _StopStart(action);
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void _MoveUpdate()
    {
        Vector3 splinePos = _spline.EvaluatePosition(_SplinePos);
        transform.position = splinePos;
        _EneBody.transform.position = transform.TransformPoint(_OffsetPos);

        float Speed = _speed * _SpeedCurve.Evaluate(_SplinePos) / _SplineLength * Time.deltaTime;

        if (_Stop || _EneCs.Hp <= 0)
        {
            if(_DecTimer > 0f) _DecTimer -= Time.deltaTime;
            else _DecTimer = 0f;
        }
        else
        {
            if (_DecTimer < _DecTime) _DecTimer += Time.deltaTime;
            else _DecTimer = _DecTime;
        }

        _SplinePos += Speed * Mathf.Lerp(0, 1, _DecTimer / _DecTime);
    }

    #region//Stop関連
    private void _StopStart(StopAction stopact)
    {
        _Stop = true;
        _StopTimer = stopact.StopTime;
        _StopEvent = stopact.Event;
        _EventPlay = false;
        if (stopact.LookTarget) _LookTarget = stopact.LookTarget;
        stopact.fin = true;
    }

    private void _StopUpdate()
    {
        if (_StopTimer > 0f)
        {
            _StopTimer -= Time.deltaTime;
            return;
        }

        if (_StopEvent != null)
        {
            if (!_EventPlay)
            {
                _StopEvent.Invoke();
                _EventPlay = true;
            }
        }
        else StopFinish();
    }

    public void StopFinish()
    {
        _Stop = false;
        _LookTarget = null;
    }
    #endregion

    /// <summary>
    /// 回転
    /// </summary>
    private void _LookUpdate()
    {
        Quaternion LookRot = Quaternion.LookRotation(_spline.EvaluateTangent(_SplinePos));
        transform.rotation = LookRot;

        if (_LookTarget) LookRot = Quaternion.LookRotation(_LookTarget.position - _EneBody.transform.position);

        _EneBody.transform.rotation = Quaternion.Slerp(_EneBody.transform.rotation, LookRot, _LookLerp);
    }

    private void OnDrawGizmos()
    {
        if (_spline && _TestGizmoPreview)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_spline.EvaluatePosition(_TestSplinePos), 1f);

            Gizmos.color = Color.yellow;
            foreach (StopAction action in _Actions)
            {
                Gizmos.DrawSphere(_spline.EvaluatePosition(action.StopStartPos), 0.6f);
            }
        }
    }
}

[System.Serializable]
public class StopAction
{
    [SerializeField, Range(0, 1)][Header("ストップ開始位置")] public float StopStartPos;
    [Header("停止時間")] public float StopTime;
    [Header("イベント")] public UnityEvent Event;
    [Header("視点先")] public Transform LookTarget;

    [Space(10)]
    public bool fin = false;
}
