using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheckDistance : MonoBehaviour
{
    [SerializeField]
    private GameObject _Canvas;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Transform _target;

    private void Update()
    {
        if(!_target || !_text) return;

        float distance = Vector3.Distance(transform.position, _target.position);
        _text.text = string.Format("{0:0.0} m", distance);

        if (GM.instance.CameraObj)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _target.position);
        }

        if (_Canvas)
        {
            _Canvas.transform.localScale = Vector3.one * distance / 40;
        }
    }
}
