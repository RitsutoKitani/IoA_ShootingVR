using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public enum ToggleContent
    {
        MainHand,
        TutorialSkip
    }

    [SerializeField] private bool _toggle;
    public bool toggle { get => _toggle; }

    [SerializeField] private ToggleContent _content;

    [Space(30)]
    [SerializeField] private Text _text;
    [SerializeField] private string[] _TextWord = new string[2];

    [SerializeField] private Image _back;
    [SerializeField] private Color[] _BackColor = new Color[2];

    private void Start()
    {
        switch(_content)
        {
            case ToggleContent.MainHand:
                _toggle = GM.instance.LeftMain;
                break;

            case ToggleContent.TutorialSkip:
                _toggle = GM.instance.TutorialSkip;
                break;
        }

        _TextImageUpdate();
    }

    public void ToggleClick()
    {
        _toggle = !_toggle;
        SetValue();
        _TextImageUpdate();
    }

    public void SetValue()
    {
        switch (_content)
        {
            case ToggleContent.MainHand:
                GM.instance.LeftMain = _toggle;
                break;

            case ToggleContent.TutorialSkip:
                GM.instance.TutorialSkip = _toggle;
                break;
        }
    }

    private void _TextImageUpdate()
    {
        int val = _toggle ? 1 : 0; //オンのとき0、オフのとき1

        if(_text) _text.text = _TextWord[val];
        if(_back) _back.color = _BackColor[val];
    }
}
