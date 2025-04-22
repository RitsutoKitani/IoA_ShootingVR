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

    [Space(30)]

    [SerializeField]
    [Header("終了時のに削除")] private bool _FinishDel;

    [Space(30)]

    private Animator _ani;

    [SerializeField]
    private Collider[] _cols;

    [SerializeField]
    private SkinnedMeshRenderer _smr;
    private Material[] materials;

    [SerializeField, Range(0f, 1f)]
    private float _dither = 1f;

    [SerializeField, Range(0f,1f)]
    private float _flash = 0f;

    private void Start()
    {
        _ani = GetComponent<Animator>();;
        if (_smr) materials = _smr.materials;
        else materials = new Material[0];
    }

    private void Update()
    {
        _MatUpdate();
    }

    private void _MatUpdate()
    {
        if (materials.Length <= 0) return;
        foreach (Material mat in materials)
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
        _Hp = _HpMax;
    }

    private void ExploEff()
    {
        ObjPool.instance.MakeObj(ObjPool.instance.ExploEffPool, transform.position, transform.rotation);
    }
}
