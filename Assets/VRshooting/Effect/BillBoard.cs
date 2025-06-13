using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public enum BillboardType
    {
        Full,       // 両軸回転（XとY）
        YOnly,      // Y軸のみ回転（よくある2D立ち絵風）
        XOnly       // X軸のみ回転（珍しいが選択可能）
    }

    [SerializeField] private BillboardType _Type = BillboardType.Full;
    [SerializeField] private Camera _Camera;

    void Start()
    {
        // カメラが未指定ならメインカメラを自動設定
        if (_Camera == null)
        {
            _Camera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (_Camera == null) return;

        Vector3 targetPosition = _Camera.transform.position;
        Vector3 direction = targetPosition - transform.position;

        switch (_Type)
        {
            case BillboardType.Full:
                // 全軸回転
                transform.forward = direction.normalized;
                break;

            case BillboardType.YOnly:
                direction.y = 0; // Y方向の変化を無視
                if (direction.sqrMagnitude > 0.001f)
                    transform.forward = direction.normalized;
                break;

            case BillboardType.XOnly:
                direction.x = 0; // X方向の変化を無視
                if (direction.sqrMagnitude > 0.001f)
                    transform.forward = direction.normalized;
                break;
        }
    }
}
