using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private bool _Left;
    public bool Left { get => _Left; }
    [SerializeField]
    [Header("セット中武器")] private GameObject _Gun;

    #region//操作
    InputActionAsset _IA;
    #endregion

    private void Start()
    {
        if (_Left) GM.instance.LeftHand = gameObject;
        else GM.instance.RightHand = gameObject;
    }

    private void Update()
    {
        if(GM.instance.LeftMain == _Left) //メイン(左利き手＝このオブジェクトが左手)
        {
            if(_Gun == null)
            {
                SetGun(GM.instance.SetMainGun[GM.instance.UseGun]);
            }
        }
        else //サブ
        {
            if (_Gun) Destroy(_Gun);
        }
    }

    private void SetGun(GameObject Gun)
    {
        GameObject gun = Instantiate(Gun, transform.position, transform.rotation, GM.instance.Player.transform);
        if (gun.GetComponent<MainGun>())
        {
            gun.GetComponent<MainGun>().InitalSetting(transform, _Left);
        }
        _Gun = gun;
        Debug.Log("武器装着");
    }
}
