using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionWindowButton : MonoBehaviour
{
    public enum ToggleContent
    {
        MainHand,
        TutorialSkip,
        BGM,
        SE
    }

    [SerializeField, ReadOnly] private bool _toggle;
    public bool toggle { get => _toggle; }
    private Slider _slider;

    [SerializeField] private ToggleContent _content;

    [Space(30)]
    [SerializeField] private Text _text;
    [SerializeField] private string[] _TextWord = new string[2];

    [SerializeField] private Image _back;
    [SerializeField] private Color[] _BackColor = new Color[2];

    private void Start()
    {
        if (GetComponent<Slider>()) _slider = GetComponent<Slider>();

        switch (_content)
        {
            case ToggleContent.MainHand:
                _toggle = GM.instance.LeftMain;
                _ToggleUpdate();
                break;

            case ToggleContent.TutorialSkip:
                _toggle = GM.instance.TutorialSkip;
                _ToggleUpdate();
                break;

            case ToggleContent.BGM:
                _SliderUpdate(GM.instance.BGMvol);
                break;

            case ToggleContent.SE:
                _SliderUpdate(GM.instance.SEvol);
                break;
        }
    }

    public void ToggleClick()
    {
        _toggle = !_toggle;
        SetValue();
    }

    public void SetValue()
    {
        switch (_content)
        {
            case ToggleContent.MainHand:
                GM.instance.LeftMain = _toggle;
                _ToggleUpdate();
                break;

            case ToggleContent.TutorialSkip:
                GM.instance.TutorialSkip = _toggle;
                _ToggleUpdate();
                break;

            case ToggleContent.BGM:
                GM.instance.BGMvol = _slider.value;
                _SliderUpdate(GM.instance.BGMvol);
                break;

            case ToggleContent.SE:
                GM.instance.SEvol = _slider.value;
                _SliderUpdate(GM.instance.SEvol);
                break;
        }
    }

    private void _SliderUpdate(float value)
    {
        if (_slider) _slider.value = value;
        if (_text) _text.text = Mathf.FloorToInt(_slider.value * 100).ToString();
    }

    private void _ToggleUpdate()
    {
        int val = _toggle ? 1 : 0; //オンのとき0、オフのとき1
        if (_text) _text.text = _TextWord[val];
        if (_back) _back.color = _BackColor[val];
    }
}
