using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunWindow : MonoBehaviour
{
    [SerializeField] private int _SelectSlot = 0;
    [SerializeField] private GunSlot[] _slots = new GunSlot[2];

    [Space(30)]

    [SerializeField] private Text _Aname;
    [SerializeField] private GameObject _Slot3DIcon;
    private MeshRenderer _SlotMeshRenderer;
    private MeshFilter _SlotMeshFilter;

    [Space(10)]

    [SerializeField] private Text _Bname;
    [SerializeField] private GameObject _Select3DIcon;
    private MeshRenderer _SelectMeshRenderer;
    private MeshFilter _SelectMeshFilter;

    [Space(30)]

    [SerializeField] private List<GunData> _GunList = new List<GunData>();
    [SerializeField, ReadOnly] private GunData _SelectData = null;
    [SerializeField][Header("スクロールの銃アイコンUIプレハブ")] private GameObject _GunSlotObj;
    [SerializeField][Header("スクロールの親オブジェ")] private Transform _ScrollTransform;
    [SerializeField] private ScrollRect _ScrollRect;
    [SerializeField,ReadOnly] private List<GameObject> _BslotList = new List<GameObject>();

    private void Start()
    {
        _SlotMeshRenderer = _Slot3DIcon.GetComponent<MeshRenderer>();
        _SlotMeshFilter = _Slot3DIcon.GetComponent<MeshFilter>();
        _SelectMeshRenderer = _Select3DIcon.GetComponent<MeshRenderer>();
        _SelectMeshFilter = _Select3DIcon.GetComponent<MeshFilter>();

        _ScrollCreate();
        SlotSelect(0);
    }

    private void _ScrollCreate()
    {
        if (_GunList.Count <= 0) return;

        foreach(var slot in _GunList)
        {
            GameObject Slot = Instantiate(_GunSlotObj, _ScrollTransform);
            Slot.GetComponent<GunSlot>().SetData(slot);
            _BslotList.Add(Slot);
        }

        if(_ScrollRect)_ScrollRect.verticalNormalizedPosition = 1f;
        BsetData(_GunList[0]);
    }

    public void SlotSelect(int num)
    {
        _SelectSlot = num;
        GunData Data = GM.instance.SetMainGun[_SelectSlot];

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetData(GM.instance.SetMainGun[i]);
            _slots[i].interact = _SelectSlot != i;
        }

        if (!_SlotMeshFilter || !_SlotMeshRenderer) return;

        _StatusDataSet(false, Data);
    }

    public void BsetData(GunData Data)
    {
        _SelectData = Data;
        foreach (var slot in _BslotList)
        {
            GunSlot slotCs = slot.GetComponent<GunSlot>();
            slotCs.interact = _SelectData != slotCs.Data;
        }

        _StatusDataSet(true, Data);
    }

    private void _StatusDataSet(bool selectSlot = false, GunData Data = null)
    {
        if (!Data) return;
        Text NameText = _Aname;
        MeshRenderer mr = _SlotMeshRenderer;
        MeshFilter mf = _SlotMeshFilter;
        if(selectSlot)
        {
            NameText = _Bname;
            mr = _SelectMeshRenderer;
            mf = _SelectMeshFilter;
        }

        NameText.text = Data.Name;
        mr.materials = Data.IconMaterials;
        mf.mesh = Data.IconMesh;
    }
}
