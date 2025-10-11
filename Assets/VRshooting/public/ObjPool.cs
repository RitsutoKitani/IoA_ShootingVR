using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public static ObjPool instance;

    public ObjPoolInfo BulletPool;

    public ObjPoolInfo ExploEffPool;

    public ObjPoolInfo WarningUI;

    [SerializeField, ReadOnly]private List<ObjPoolInfo> PoolList = new List<ObjPoolInfo>();

    [SerializeField] private GameObject[] _PreVFX;
    private List<GameObject> _PreObjs;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _PreObjs = new List<GameObject>();

        foreach (var pre in _PreVFX)
        {
            ObjPoolInfo opi = new ObjPoolInfo();
            opi.Obj = pre;
            opi.ObjList = new List<GameObject>();

            GameObject obj = Instantiate(pre, transform.position, transform.rotation);
            opi.ObjList.Add(obj);

            //PoolList.Add(opi);
            _PreObjs.Add(obj);
        }

        Invoke("_PreFin", 0.5f);
    }

    private void _PreFin()
    {
        foreach (var obj in _PreObjs) obj.SetActive(false);
        if (StageManager.instance.Fade) StageManager.instance.Fade.FinFadeIn();
    }

    public GameObject MakeObj(ObjPoolInfo opi, Vector3 pos, Quaternion rot)
    {
        GameObject obj = null;

        foreach (GameObject list in opi.ObjList)
        {
            if (!list.activeSelf)
            {
                obj = list;
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(true);
                break;
            }
        }

        if (!obj)
        {
            obj = Instantiate(opi.Obj, pos, rot);
            opi.ObjList.Add(obj);
        }

        return obj;
    }

    private void AddList(GameObject PoolObj)
    {
        
    }

    public GameObject MakeObjByList(GameObject PoolObj, Vector3 pos, Quaternion rot)
    {
        foreach (ObjPoolInfo list in PoolList)
        {
            if (list.Obj == PoolObj)
            {
                return MakeObj(list, pos, rot);
            }
        }

        ObjPoolInfo opi = new ObjPoolInfo();
        opi.Obj = PoolObj;
        opi.ObjList = new List<GameObject>();
        PoolList.Add(opi);
        return MakeObj(opi, pos, rot);
    }
}

[System.Serializable]
public class ObjPoolInfo
{
    public GameObject Obj;
    public List<GameObject> ObjList;
}