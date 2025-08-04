using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyImageColor : MonoBehaviour
{
    private Image _base;

    [SerializeField] private List<GameObject> _UIs = new List<GameObject>();

    [SerializeField] Calculation _CalcuType;
    [SerializeField, Range(0f, 1f)] private float _Lerp;
    [SerializeField][Header("アルファ値はそのまま")] private bool _IgnoreAlpha;
    private Color[] _OriginColor;

    public enum Calculation
    {
        None,
        Add,
        Multiplication,
        Lerp
    }

    void Start()
    {
        if(!GetComponent<Image>()) Destroy(gameObject);
        _base = GetComponent<Image>();

        _OriginColor = new Color[_UIs.Count];
        for (int i = 0;i < _OriginColor.Length; i++)
        {
            if (_UIs[i].GetComponent<Image>()) _OriginColor[i] = _UIs[i].GetComponent<Image>().color;
            if (_UIs[i].GetComponent<Text>()) _OriginColor[i] = _UIs[i].GetComponent<Text>().color;
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < _UIs.Count; i++)
        {
            Color color = _base.color;
            switch (_CalcuType)
            {
                case Calculation.Add:
                    color = _OriginColor[i] + _base.color; break;

                case Calculation.Multiplication:
                    color = _OriginColor[i] * _base.color; break;

                case Calculation.Lerp:
                    color = Color.Lerp(_OriginColor[i], _base.color, _Lerp); break;
            }

            if (_IgnoreAlpha) color.a = _OriginColor[i].a;
            if (_UIs[i].GetComponent<Image>()) _UIs[i].GetComponent<Image>().color = color;
            if (_UIs[i].GetComponent<Text>()) _UIs[i].GetComponent<Text>().color = color;
        }
    }
}
