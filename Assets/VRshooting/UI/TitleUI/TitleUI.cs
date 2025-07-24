using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public static TitleUI instance;

    [SerializeField] private int _WindowNum;
    public int WindowNum { get => _WindowNum; }
    [SerializeField] private GunWindow _GunWindow;
    public GunWindow GunWindow { get =>  _GunWindow; }


    [Space(30)]

    [SerializeField] private Text _HelpText;
    public Text HelpText { get => _HelpText; }

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Update()
    {
        if (_WindowNum > 3|| _WindowNum < 0) _WindowNum = 0;

        Vector3 lookVec = Vector3.forward;
        switch(_WindowNum)
        {
            case 0: lookVec = Vector3.forward; break;

            case 1: lookVec = Vector3.left; break;

            case 2: lookVec = Vector3.back; break;

            case 3: lookVec = Vector3.right; break;
        }

        Quaternion rot = Quaternion.LookRotation(lookVec, transform.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
    }

    public void ChangeWindow(int num)
    {
        _WindowNum = num;
    }

    public void ChangeMainHand()
    {
        
    }

}
