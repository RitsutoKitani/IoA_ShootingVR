using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int _HpMax;
    public int HpMax { get => HpMax; }

    [SerializeField] private int _Hp;
    public int Hp { get => _Hp; }

    [SerializeField] private int _Atk;
    public int Atk { get => _Atk; }

    [SerializeField] private int _Score;
    public int Score { get => _Score; }

    [Space(30)]
    [SerializeField]
    [Header("終了時のに削除")] private bool _FinishDel;

    [Space(30)]
    private Animator _ani;

    [SerializeField]
    private Collider[] _cols;

    [SerializeField]
    private SkinnedMeshRenderer[] _smr;
    [SerializeField]
    private MeshRenderer[] _mr;
    private List<Material> _materials;

    [SerializeField, Range(0f, 1f)]
    private float _dither = 1f;

    [SerializeField, Range(0f,1f)]
    private float _flash = 0f;

    private Vector3 _FirstPos;
    public Vector3 FirstPos { get => _FirstPos; }
    private bool _FirstTime = true;


    private void Awake()
    {
        _ani = GetComponent<Animator>();;

        _materials = new List<Material>();
        if (_smr.Length > 0) foreach (var mat in _smr) _materials.AddRange(mat.materials);
        if (_mr.Length > 0) foreach (var mat in _mr) _materials.AddRange(mat.materials);

        _FirstPos = transform.position;
    }

    private void Update()
    {
        _MatUpdate();
    }

    private void _MatUpdate()
    {
        if (_materials.Count <= 0) return;
        foreach (Material mat in _materials)
        {
            mat.SetFloat("_dither", _dither);
            mat.SetFloat("_flash", _flash);
        }
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        _Hp -= damage;

        if (_Hp <= 0)
        {
            if (StageManager.instance) StageManager.instance.ScoreGet(Score);
            foreach(Collider col in _cols) col.enabled = false;
            if (_ani) _ani.SetBool("Destroy",true);
            else Finish();
        }
        else if (_ani) _ani.SetTrigger("Damage");
    }

    public void Finish()
    {
        if (_FinishDel) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public void ReSetting()
    {
        if(!_FirstTime) transform.position = _FirstPos;
        _FirstTime = false;

        foreach (Collider col in _cols) col.enabled = true;
        _Hp = _HpMax;
        gameObject.SetActive(false);
    }

    private void ExploEff()
    {
        ObjPool.instance.MakeObj(ObjPool.instance.ExploEffPool, transform.position, transform.rotation);
    }
}
