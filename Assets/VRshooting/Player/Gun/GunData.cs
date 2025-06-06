using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "CreateGun")]
public class GunData : ScriptableObject
{
    [Header("名前")] public string Name;
    [Header("アイコン")] public Sprite Icon;
    [Header("3Dアイコン（メッシュ）")] public Mesh IconMesh;
    [Header("3Dアイコン（マテリアル）")] public Material[] IconMaterials;

    [Space(30)]

    public GameObject GunObj;
    [Header("弾ステータス")] public BulletStatus BulletStatus;
    [Header("マガジン弾数")] public int MagazineBulletMax;
    [Header("連射間隔")] public float ShotInterval;
    [Header("リロード時間")] public float ReloadTime;
    [Header("拡散角度（片手）")] public float DiffAngleOH;
    [Header("拡散角度（両手）")] public float DiffAngleBH;
}
