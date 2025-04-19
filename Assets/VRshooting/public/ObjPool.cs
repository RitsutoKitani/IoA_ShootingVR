using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public static ObjPool instance;

    [Space(30)]
    [SerializeField]
    private GameObject _Bullet;
    [SerializeField]
    private List<GameObject> _BulletList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void MakeBullet(Transform pos, BulletStatus status)
    {
        GameObject bullet = null;
        foreach (GameObject obj in _BulletList)
        {
            if(!obj.activeSelf)
            {
                bullet = obj;
                bullet.transform.position = pos.position;
                bullet.transform.rotation = pos.rotation;
                bullet.SetActive(true);
                break;
            }
        }

        if(!bullet)
        {
            bullet = Instantiate(_Bullet, pos.position, pos.rotation);
            _BulletList.Add(bullet);
        }

        bullet.GetComponent<Bullet>().ReStatus(status);
    }
}