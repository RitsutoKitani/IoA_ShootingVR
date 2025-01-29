using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEobj : MonoBehaviour
{
    private AudioSource _AS;
    private void Awake()
    {
        _AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_AS.isPlaying) gameObject.SetActive(false); 
    }
}
