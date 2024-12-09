using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GM : MonoBehaviour
{
    [Header("プレイヤー")] public GameObject Player;
    [Header("右手スクリプト")] public GameObject RightHand;
    [Header("左手スクリプト")] public GameObject LeftHand;

    [Header("左手が利き手")] public bool LeftMain;

    [Header("セットメイン武器")] public GameObject[] SetMainGun = new GameObject[2];
    [Header("使用メイン武器")] public int UseGun = 0;
    [Header("セットサブ武器")] public GameObject SetSubGun;

    public static GM instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
