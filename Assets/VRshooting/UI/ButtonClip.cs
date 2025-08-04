using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClip : MonoBehaviour
{
    private Animator _ani;
    public bool InterActive = true;
    [SerializeField] private UnityEvent _Event;

    [Space(30)]
    [SerializeField][Header("タッチ時")] private EventInfo _TouchInfo;
    [SerializeField][Header("クリック時")] private EventInfo _ClickInfo;

    private void Start()
    {
        if(GetComponent<Animator>()) _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_ani) _ani.SetBool("Active", InterActive);
    }

    public void Touch()
    {
        if (!InterActive) return;
        StageManager.instance.UIHandVibe(_TouchInfo.strength, _TouchInfo.length);
        GM.instance.PlayOneSE(_TouchInfo.SE, transform);

        if(_ani) _ani.SetBool("Touch", true);
    }

    public void UnTouche()
    {
        if (_ani) _ani.SetBool("Touch", false);
    }

    public void Click()
    {
        if (!InterActive) return;
        StageManager.instance.UIHandVibe(_ClickInfo.strength, _ClickInfo.length);
        GM.instance.PlayOneSE(_ClickInfo.SE, transform);

        _Event.Invoke();

        if (_ani) _ani.SetTrigger("Click");
    }

    [System.Serializable]
    public class EventInfo
    {
        public AudioClip SE;

        public float strength;
        public float length;
    }
}
