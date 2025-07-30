using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNotation : MonoBehaviour
{
    [SerializeField] private Text _text;
    private Animator _ani;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }

    public void Initialize(string text, float time = 0.5f, Color? color = null, float scale = 1.0f)
    {
        if (!_text || !_ani) return;
        _text.color = color ?? Color.white; //色変更

        _text.text = text; //文字変更

        _ani.SetFloat("Speed", 1 / time); //スピード変更

        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        transform.localScale = Vector3.one * distance * scale / 40; //スケール変更

        gameObject.SetActive(true);
    }
}
