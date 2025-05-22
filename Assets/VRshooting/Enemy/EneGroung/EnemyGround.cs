using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private int _ActCace = 0;

    [Space(30)]
    [SerializeField] private List<JumpEvent> _JumpEvents = new List<JumpEvent>();
    private Vector3 _JumpVec;
    private float _JumpPow;
    private float _JumpingTimer = 0f;

    [Space(30)]
    [SerializeField] private List<ShotEvent> _ShotEvents = new List<ShotEvent>();
    [SerializeField] private GameObject _Bullet;
    [SerializeField] private Transform _ShotPos;
    [SerializeField] private float _ShotChargeTime;
    private float _ShotChargeTimer = 0f;
    private int _ShotCount;
    private float _ShotUpAngle;
    private float _NoGuideTime;
    private Vector3 _AimPos;


    [Space(30)]
    [SerializeField][Header("方向転換速度")] private float _TurnSpeed;
    [SerializeField][Header("方向転換カーブ")] private AnimationCurve _TurnCurve;
    private float _TurnAngle;
    private float _TurnRot;

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
        _ani.SetBool("JumpCharge", _ActCace == 1);
        _ani.SetBool("ShotCharge", _ActCace == 4 || _ActCace == 3);
        
        switch (_ActCace)
        {
            case 1:
                _TurnUpdate();
                break;

            case 2:
                _JumpingUpdate();
                break;

            case 3:
                _TurnUpdate();
                break;

            case 4:
                _ShotChargeUpdate();
                break;

        }
    }

    private void _TurnUpdate()
    {
        float rot = _TurnSpeed * Time.deltaTime * _TurnCurve.Evaluate(_TurnRot / _TurnAngle);

        if (_TurnAngle < 0) rot *= -1;

        transform.Rotate(0f, rot, 0f);
        _TurnRot -= rot;

        if (Mathf.Abs(_TurnRot) > 0.1f) return;

        if (_ActCace == 1) _ani.SetTrigger("Jump");
        if (_ActCace == 3) _ActCace = 4;
    }

    public void JumpStart(int num)
    {
        JumpEvent JumpEve = _JumpEvents [num];
        _JumpVec = JumpEve.JumpVec;
        _JumpPow = JumpEve.JumpPow;
        _TurnAngle = JumpEve.TurnAngle;
        _TurnRot = JumpEve.TurnAngle;
        _ActCace = 1;
    }

    private void _Jump()
    {
        _rb.AddRelativeForce(_JumpVec.normalized * _JumpPow, ForceMode.Impulse);
        _JumpingTimer = 0f;
        _ActCace = 2;
    }


    private void _JumpingUpdate()
    {
        _JumpingTimer += Time.deltaTime;

        if (_SplineMove.isGround && _JumpingTimer > 0.5f)
        {
            _SplineMove.StopFinish();
            _ActCace = 0;
            Debug.Log("着地！");
        }
    }

    public void ShotStart(int num)
    {
        ShotEvent ShotEve = _ShotEvents [num];
        _ShotCount = ShotEve.ShotCount;
        _ShotChargeTimer = _ShotChargeTime;
        _ShotUpAngle = ShotEve.ShotAngle.y;
        _NoGuideTime = ShotEve.NoGuideTime;
        _AimPos = StageManager.instance.DomeCs.gameObject.transform.position;

        float Angle = ShotEve.ShotAngle.x;
        if (StageManager.instance)
        {
            Angle += Vector3.SignedAngle(transform.forward, 
                _AimPos - transform.position, Vector3.up);
        }

        _TurnAngle = Angle;
        _TurnRot = Angle;
        _ActCace = 3;
    }

    private void _ShotChargeUpdate()
    {
        if (_ShotChargeTimer > 0f)
        {
            _ShotChargeTimer -= Time.deltaTime;
            return;
        }

        _ani.SetTrigger("Shot");
        _ShotChargeTimer = _ShotChargeTime;
        _ShotCount--;

        if (_ShotCount <= 0) _ActCace = 0;
    }

    private void _Shot()
    {
        if(!_Bullet || !_ShotPos) return;
        GameObject bullet = Instantiate(_Bullet, _ShotPos.position, _ShotPos.rotation);

        if(bullet.GetComponent<EnemyMissile>())
        {
            bullet.transform.Rotate(-_ShotUpAngle, 0f, 0f, Space.Self);
            bullet.GetComponent<EnemyMissile>().MissileSetting(_AimPos, _NoGuideTime);
        }
    }

    private void _ShotFinish()
    {
        if (_ActCace != 4) _SplineMove.StopFinish();
    }

    [System.Serializable]
    class JumpEvent
    {
        public Vector3 JumpVec;
        public float JumpPow;
        public float TurnAngle;
    }

    [System.Serializable]
    class ShotEvent
    {
        public int ShotCount;
        public Vector2 ShotAngle;
        public float NoGuideTime;
    }
}
