using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class IndicateIcon : MonoBehaviour
{

    [SerializeField] private Image _Icon;

    public void SetIcon(float size = 1f, Sprite sprite = null)
    {
        if (!_Icon) return;
        _Icon.transform.localScale = new Vector3(size, size, size);
        if(sprite) _Icon.sprite = sprite;
    }
}
