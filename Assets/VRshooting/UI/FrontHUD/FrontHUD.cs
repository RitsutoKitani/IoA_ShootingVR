using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontHUD : MonoBehaviour
{
    [SerializeField][Header("‰ñ“]‘¬“x")] private float _TurnTorque;
    [SerializeField] private Toggle _TurnTestToggle;

    [Space(30)]

    [SerializeField] private Text _AmmoText;

    void Update()
    {
        if(!StageManager.instance) return;

        _CanvasUpdate();

        _GunUpdate();
    }

    private void _CanvasUpdate()
    {
        Transform Camera = StageManager.instance.CameraObj.transform;

        Vector3 LookPos = Camera.transform.position + Camera.transform.forward * 8f;

        if (!_TurnTestToggle.isOn) LookPos = transform.position + Vector3.forward * 8f;


        var diff = new Vector3(LookPos.x, transform.position.y, LookPos.z) - transform.position;
        var rot = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, _TurnTorque);
    }

    private void _GunUpdate()
    {
        if (!_AmmoText) return;

        if(GM.instance.SetMainGunsCs.Count <= 0) return;
        MainGun GunCs = GM.instance.SetMainGunsCs[GM.instance.UseGun];

        _AmmoText.text = $"{GunCs.MagazineBullet:D2}";
    }
}
