using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMeshButton : MonoBehaviour
{
    [SerializeField] private bool _Interactable;
    public bool Interactable { get => _Interactable; }

    [SerializeField][Header("à–¾•¶")] private string _helpText;
    private Animator _ani;

    [SerializeField, ReadOnly] private bool _Touch;
    public bool Touch { get => _Touch; }

    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    public void TouchOnOff(bool touch)
    {
        _Touch = touch;
        _ani.SetBool("Touch", touch);
        if (touch && TitleUI.instance) TitleUI.instance.HelpText.text = _helpText;
    }

    public void SendText()
    {
        if(TitleUI.instance) TitleUI.instance.HelpText.text = _helpText;
    }

    public void ChangeInteractable(bool interactable)
    {
        _Interactable = interactable;
    }
}