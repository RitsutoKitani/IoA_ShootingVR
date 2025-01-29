using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniClip : MonoBehaviour
{
    private void PlaySE(AudioClip SE)
    {
        GM.instance.PlayOneSE(SE, transform);
    }
}
