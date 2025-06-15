using UnityEngine;
using UnityEngine.Events;

public class LoopActEvent : MonoBehaviour
{
    [SerializeField] public bool Active;

    [Space(30)]
    [SerializeField] private UnityEvent _Event;
    [SerializeField] private float _RepeatTime;
    [SerializeField] private float _timer;

    private void Update()
    {
        if(!Active) return;
        if (_timer < _RepeatTime) _timer += Time.deltaTime;
        else
        {
            _Event.Invoke();
            _timer = 0f;
        }
    }
}