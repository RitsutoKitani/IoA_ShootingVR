using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;



public class EnemySplineMoveGra : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField, ReadOnly][Header("スプライン上の位置 [0-1]")] private float _SplinePos = 0f;
    [SerializeField] private float _BaseSpeed;
    [SerializeField] private AnimationCurve _SpeedCurve;

    [Space(30)]
    private bool _isGround;
    public bool isGround { get => _isGround; }
    [SerializeField][Header("足場判定距離")] private float _GroundCheckDis;
    [SerializeField][Header("足場判定レイヤー")] private LayerMask _GoundLayer;

    [Space(30)]
    [SerializeField, ReadOnly] private bool _Stop = false;
    public bool Stop { get => _Stop; }

    [SerializeField] private List<StopAction> _StopAct;
    [SerializeField][Header("減速時間")] private float _DecTime;
    [SerializeField, ReadOnly] private float _DecTimer = 0f;
    [SerializeField, ReadOnly] private float _StopTimer = 0f;
    private UnityEvent _StopEvent;
    private bool _StopEventPlay = false;

    private Vector3 _offsetVec;
    private bool _touchStart = false;

    private Enemy _EneCs;
    private Rigidbody _rb;
    private Animator _ani;


    [Space(30)]
    [SerializeField] private bool _TestGizmoPreview;

    private void Start()
    {
        _EneCs = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponent<Animator>();

        Vector3 splineStartPos = _spline.EvaluatePosition(0f);
        Vector3 tangent = _spline.EvaluateTangent(0f);
        Vector3 up = _spline.EvaluateUpVector(0f);

        Quaternion splineRotation = Quaternion.LookRotation(tangent, up);
        Matrix4x4 splineMatrix = Matrix4x4.TRS(splineStartPos, splineRotation, Vector3.one);
        _offsetVec = splineMatrix.inverse.MultiplyPoint3x4(transform.position);
    }

    private void FixedUpdate()
    {
        if (!_EneCs) return;
        _isGround = Physics.Raycast(transform.position, Vector3.down, _GroundCheckDis, _GoundLayer);

        if (!_touchStart)
        {
            _touchStart = _isGround;
            return;
        }

        _MoveUpdate();
        _StopUpdate();
        _SearchEvent();
    }

    private void _MoveUpdate()
    {
        Vector3 splinePos = _spline.EvaluatePosition(_SplinePos);
        Vector3 tangent = _spline.EvaluateTangent(_SplinePos);
        Vector3 up = _spline.EvaluateUpVector(_SplinePos);
        Quaternion splineRot = Quaternion.LookRotation(tangent, up);

        Vector3 targetPos = splinePos + splineRot * _offsetVec;

        if (Vector3.Distance(targetPos, transform.position) > 50f)
        {
            Debug.Log("路線から大きく離脱");
            _EneCs.Finish();
        }

        float currentSpeed = _BaseSpeed * _SpeedCurve.Evaluate(_SplinePos) * Mathf.Lerp(0, 1, _DecTimer / _DecTime);
        Vector3 direction = (targetPos - transform.position).normalized;

        if (_isGround && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _GroundCheckDis))//地面についているとき
        {
            direction = Vector3.ProjectOnPlane(direction, hit.normal).normalized; //スプラインの進行方向（地面に沿って）
        }

        Vector3 movement = direction * currentSpeed;
        movement.y = _rb.velocity.y;
        if (_DecTimer > 0f) _rb.velocity = movement;

        _ani.SetFloat("Speed", currentSpeed, 0.2f, Time.deltaTime);


        if (direction.sqrMagnitude > 0.001f && _isGround && !_Stop && _DecTimer > 0f) //回転
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 4f * Time.fixedDeltaTime);
        }

        float splineLength = _spline.CalculateLength();
        float step = (currentSpeed * Time.fixedDeltaTime) / splineLength;
        _SplinePos += step;
        _SplinePos = Mathf.Clamp01(_SplinePos);
    }

    private void _SearchEvent()
    {
        if (_Stop) return;

        foreach (var action in _StopAct)
        {
            if (action.StopStartPos < _SplinePos && !action.fin) StopStart(action);
        }

        if (_SplinePos >= 1f) _EneCs.Finish();
    }

    public void StopStart(StopAction stopact)
    {
        _Stop = true;
        _StopTimer = stopact.StopTime;
        _StopEvent = stopact.Event;
        _StopEventPlay = false;
        stopact.fin = true;
    }

    private void _StopUpdate()
    {
        if (_Stop || _EneCs.Hp <= 0)
        {
            if (_DecTimer > 0f) _DecTimer -= Time.deltaTime;
            else _DecTimer = 0f;
        }
        else
        {
            if (_DecTimer < _DecTime) _DecTimer += Time.deltaTime;
            else _DecTimer = _DecTime;
        }

        if (!_Stop) return;
        if (_StopTimer > 0f)
        {
            _StopTimer -= Time.deltaTime;
            return;
        }

        if (_StopEvent.GetPersistentEventCount() > 0)
        {
            if (!_StopEventPlay)
            {
                _StopEvent.Invoke();
                _StopEventPlay = true;
            }
        }
        else StopFinish();
    }

    public void StopFinish()
    {
        _Stop = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _GroundCheckDis);

        if (!_spline || !_TestGizmoPreview) return;
        Gizmos.color = Color.yellow;
        foreach (StopAction action in _StopAct)
        {
            Gizmos.DrawSphere(_spline.EvaluatePosition(action.StopStartPos), 0.6f);
        }
    }
}
