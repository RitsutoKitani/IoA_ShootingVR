using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClip : MonoBehaviour
{
    [SerializeField]
    [Header("タッチ時バイブレーション")] private VibeInfo _TouchVibe;
    [SerializeField]
    [Header("タッチ時SE")] private AudioClip _TouchSE;

    [Space(30)]
    [SerializeField]
    [Header("クリック時バイブレーション")] private VibeInfo _ClickVibe;
    [SerializeField]
    [Header("クリック時SE")] private AudioClip _ClickSE;


    public void Touch()
    {
        StageManager.instance.UIHandVibe(_TouchVibe.strength, _TouchVibe.length);
        GM.instance.PlayOneSE(_TouchSE, transform);
    }

    public void Click()
    {
        StageManager.instance.UIHandVibe(_ClickVibe.strength, _ClickVibe.length);
        GM.instance.PlayOneSE(_ClickSE, transform);
    }

    [System.Serializable]
    public class VibeInfo
    {
        public float strength;
        public float length;
    }
}
