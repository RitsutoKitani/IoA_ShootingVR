using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ScrollEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent _TouchEvent;
    [SerializeField] private UnityEvent _UnTouchEvent;
    [SerializeField] private UnityEvent _ClickEvent;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _TouchEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _UnTouchEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //_ClickEvent.Invoke();
        //Debug.Log("クリックされたよ。");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //_ClickEvent.Invoke();
        //Debug.Log("クリックされたよ。");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _ClickEvent.Invoke();
        //Debug.Log("クリックされたよ。");
    }
}
