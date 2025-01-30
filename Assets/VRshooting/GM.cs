using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GM : MonoBehaviour
{
    [Header("プレイヤー")] public GameObject Player;
    [Header("右手スクリプト")] public GameObject RightHand;
    [Header("左手スクリプト")] public GameObject LeftHand;

    [Space(20)]
    [Header("左手が利き手")] public bool LeftMain;

    [Space(20)]
    [Header("セットメイン武器")] public GameObject[] SetMainGun = new GameObject[2];
    [Header("使用メイン武器")] public int UseGun = 0;
    [Header("セットサブ武器")] public GameObject SetSubGun;

    [Space(30)]
    [SerializeField] private List<GameObject> _SEobjList = new List<GameObject>();
    [SerializeField] private GameObject SEobj;

    private InputActionAsset _IAA;

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

    private void Start()
    {
        if (Player) _IAA = Player.GetComponent<InputActionManager>().actionAssets[0];
    }

    private void Update()
    {
        if(!_IAA) return;
        if(_IAA.FindActionMap("XRI RightHand Interaction").FindAction("LRchange").WasPerformedThisFrame()) //左右持ち替え（テスト用）
        {
            LeftMain = !LeftMain;
        }
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="pos"></param>
    public void PlayOneSE(AudioClip clip, Transform pos)
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
            se = Instantiate(SEobj); Debug.Log("SE生成");
            _SEobjList.Add(se);
        }

        AudioSource seAS = se.GetComponent<AudioSource>();
        se.transform.position = pos.position;
        seAS.PlayOneShot(clip);
    }
}
