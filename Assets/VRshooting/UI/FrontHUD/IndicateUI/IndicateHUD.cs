using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicateHUD : MonoBehaviour
{
    [SerializeField] private ObjPoolInfo _EneIconPool;

    [Space(30)]
    
    [SerializeField] private float _IconNear;
    [SerializeField] private float _IconFar;

    private void LateUpdate()
    {
        if (!StageManager.instance) return;

        List<Enemy> ActEnemy = StageManager.instance.ActEnemy;


        while (_EneIconPool.ObjList.Count < ActEnemy.Count)
        {
            GameObject obj = ObjPool.instance.MakeObj(_EneIconPool, transform.position, transform.rotation);
            obj.transform.SetParent(transform, false);
        }

        for (int i = 0; i < _EneIconPool.ObjList.Count; i++)
        {
            GameObject icon = _EneIconPool.ObjList[i];
            Transform camera = StageManager.instance.CameraObj.transform;

            if (ActEnemy.Count <= i)
            {
                icon.SetActive(false);
                continue;
            }

            Vector3 diff = ActEnemy[i].transform.position - transform.position;
            diff.y = 0;
            float angle = Vector3.SignedAngle(transform.up, diff, Vector3.up);

            icon.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
            float distance = Vector3.Distance(transform.position, ActEnemy[i].transform.position);
            float size = 1 - (distance - _IconNear) / (_IconFar - _IconNear);
            if(size < 0f) size = 0f;
            if(size > 1f) size = 1f;
            icon.GetComponent<IndicateIcon>().SetIcon(size);
        }
    }
}
