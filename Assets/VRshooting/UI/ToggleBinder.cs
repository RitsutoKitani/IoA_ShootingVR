using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBinder : MonoBehaviour
{
    [SerializeField] private bool _toggle;
    public System.Func<bool> getter;
    public System.Action<bool> setter;

    public void Bind(System.Func<bool> get, System.Action<bool> set)
    {
        getter = get;
        setter = set;
        _toggle = getter();
    }
}
