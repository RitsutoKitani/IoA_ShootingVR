using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private int _JumpCace = 0;
    [SerializeField] private List<JumpEvent> _JumpEvents = new List<JumpEvent>();
    [SerializeField] private float _TurnSpeed;
    [SerializeField] private AnimationCurve _TurnCurve;
    private Vector3 _JumpVec;
    private float _JumpPow;
    private float _JumpTurnAngle;
    private float _JumpTurnRot;
    [SerializeField, ReadOnly] private float _JumpingTimer = 0f;

    private Animator _ani;
    private Rigidbody _rb;
    private EnemySplineMoveGra _SplineMove;

    private void Start()
    {
        _ani = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _SplineMove = GetComponent<EnemySplineMoveGra>();
    }

    private void FixedUpdate()
    {
        _ani.SetBool("Fall", !_SplineMove.isGround);
        _ani.SetBool("JumpCharge", _JumpCace == 1);
        
        switch (_JumpCace)
        {
            case 1:
                _JumpChargeUpdate();
                break;

            case 2:
                _JumpingUpdate();
                break;

        }
    }

    public void JumpStart(int num)
    {
        JumpEvent JumpEve = _JumpEvents [num];
        _JumpVec = JumpEve.JumpVec;
        _JumpPow = JumpEve.JumpPow;
        _JumpTurnAngle = JumpEve.TurnAngle;
        _JumpTurnRot = JumpEve.TurnAngle;
        _JumpCace = 1;
    }

    private void _Jump()
    {
        _rb.AddRelativeForce(_JumpVec.normalized * _JumpPow, ForceMode.Impulse);
        _JumpingTimer = 0f;
        _JumpCace = 2;
    }

    private void _JumpChargeUpdate()
    {
        float rot = _TurnSpeed * Time.deltaTime * _TurnCurve.Evaluate(_JumpTurnRot / _JumpTurnAngle);

        if (_JumpTurnAngle < 0) rot *= -1;

        transform.Rotate(0f, rot, 0f);
        _JumpTurnRot -= rot;

        if (Mathf.Abs(_JumpTurnRot) > 0.1f) return;

        _ani.SetTrigger("Jump");
    }

    private void _JumpingUpdate()
    {
        _JumpingTimer += Time.deltaTime;

        if (_SplineMove.isGround && _JumpingTimer > 0.5f)
        {
            _SplineMove.StopFinish();
            _JumpCace = 0;
            Debug.Log("íÖínÅI");
        }
    }

    [System.Serializable]
    class JumpEvent
    {
        public Vector3 JumpVec;
        public float JumpPow;
        public float TurnAngle;
    }
}
