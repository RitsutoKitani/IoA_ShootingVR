using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField]
    [Header("本体スクリプト")] private Enemy _EneCs;
    public Enemy EneCs { get => EneCs; }
}