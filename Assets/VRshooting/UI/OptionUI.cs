using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField]
    private Slider _BGMslider;
    [SerializeField]
    private Slider _SEslider;

    private void Start()
    {
        if(_BGMslider) _BGMslider.value = GM.instance.BGMvol;
        if(_SEslider) _SEslider.value = GM.instance.SEvol;
    }

    public void BGMvolSet()
    {
        if (!_BGMslider) return;
        GM.instance.SEvol = _BGMslider.value;
    }

    public void SEvolSet()
    {
        if (!_SEslider) return;
        GM.instance.SEvol = _SEslider.value;
    }
}
