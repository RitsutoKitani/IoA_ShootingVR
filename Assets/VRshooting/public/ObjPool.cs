using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public static ObjPool instance;

    public ObjPoolInfo BulletPool;

    public ObjPoolInfo ExploEffPool;

    public ObjPoolInfo WarningUI;

    [SerializeField, ReadOnly]private List<ObjPoolInfo> PoolList;

    private void Awake()
    {
        instance = this;
        PoolList = new List<ObjPoolInfo>();
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
        bool check = true;
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