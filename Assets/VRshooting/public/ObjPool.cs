using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public static ObjPool instance;

    public ObjPoolInfo BulletPool;

    public ObjPoolInfo ExploEffPool;

    private void Awake()
    {
        instance = this;
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
}

[System.Serializable]
public class ObjPoolInfo
{
    public GameObject Obj;
    public List<GameObject> ObjList;
}