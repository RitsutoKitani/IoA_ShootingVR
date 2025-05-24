using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontHUD : MonoBehaviour
{
    [SerializeField][Header("‰ñ“]‘¬“x")] private float _TurnTorque;

    void Update()
    {
        if(!StageManager.instance) return;

        Transform Camera = StageManager.instance.CameraObj.transform;
        Vector3 LookPos = Camera.transform.position + Camera.transform.forward * 8f;

        var diff = new Vector3(LookPos.x, transform.position.y, LookPos.z) - transform.position;
        var rot = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, _TurnTorque);

    }
}
