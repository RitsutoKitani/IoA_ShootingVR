using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GM : MonoBehaviour
{
    [Header("ポーズ中")] public bool IsPose;

    [Space(30)]
    [Header("ステージマネージャー")] public StageManager StageManager;

    [Space(30)]
    [Header("左手が利き手")] public bool LeftMain;
    [Header("チュートリアルスキップ")] public bool TutorialSkip;
    [Header("UIハンド")] public bool UIhandLeft;

    [Space(30)]
    [Header("セット中武器")] public List<GunData> SetMainGun;
    [Header("使用武器番号")] public int UseGun = 0;
    [ReadOnly] public List<MainGun> SetMainGunsCs; //セット武器オブジェクト
    [SerializeField][Header("銃リスト")] private GunData[] _GunList;
    public GunData[] GunList { get => _GunList; }

    [Space(30)]
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
        IsPose = false;
    }

    public GunData GunSearchByName(string name)
    {
        GunData gunData = null;
        for(int i = 0; i < _GunList.Length; i++)
        {
            if(_GunList[i].Name == name) return _GunList[i];
        }
        return gunData;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="pos"></param>
    public void PlayOneSE(AudioClip clip, Transform pos, float volume = 1f, float pitch = 1f)
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
        seAS.pitch = pitch;
        seAS.Play();
    }
}
