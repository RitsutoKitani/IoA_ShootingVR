using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GM : MonoBehaviour
{
    [Header("ポーズ中")] public bool IsPose;

    [Space(40)]
    [Header("ステージマネージャー")] public StageManager StageManager;

    [Space(20)]
    [Header("左手が利き手")] public bool LeftMain;
    [Header("チュートリアルスキップ")] public bool TutorialSkip;
    [Header("UIハンド")] public bool UIhandLeft;

    [Space(20)]
    [Header("セットメイン武器")] public List<GunData> SetMainGun;
    [Header("使用メイン武器番号")] public int UseGun = 0;
    [Header("セットサブ武器")] public GameObject SetSubGun;
    [ReadOnly] public List<MainGun> SetMainGunsCs; //セット武器オブジェクト

    [Space(40)]
    [SerializeField, Range(0f, 1f)]
    [Header("BGM音量")] public float BGMvol;
    [SerializeField, Range(0f, 1f)]
    [Header("SE音量")] public float SEvol;

    [SerializeField] private List<GameObject> _SEobjList = new List<GameObject>();
    [SerializeField] private GameObject SEobj;

    public static GM instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SceneReset()
    {
        SetMainGunsCs = new List<MainGun>();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="pos"></param>
    public void PlayOneSE(AudioClip clip, Transform pos, float volume = 1f)
    {
        GameObject se = null;
        foreach (GameObject obj in _SEobjList)
        {
            if (!obj.activeSelf)
            {
                se = obj;
                obj.SetActive(true);
                break;
            }
        }
        if (!se)
        {
            se = Instantiate(SEobj);
            se.transform.parent = transform;
            _SEobjList.Add(se);
        }

        AudioSource seAS = se.GetComponent<AudioSource>();
        se.transform.position = pos.position;
        seAS.clip = clip;
        seAS.volume = SEvol * volume;
        seAS.Play();
    }
}
