using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField]
    [Header("本体スクリプト")] private Enemy _EneCs;
    public Enemy EneCs { get => _EneCs; }

    [SerializeField, Range(0f, 1f)]
    [Header("ダメージ貫通度")] private float _Pene = 1f;
    public float Pene { get => _Pene; }
}