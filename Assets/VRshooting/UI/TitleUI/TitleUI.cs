using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public static TitleUI instance;

    [SerializeField] private Text _HelpText;
    public Text HelpText { get => _HelpText; }

    private void Awake()
    {
        if (!instance) instance = this;
    }
}
