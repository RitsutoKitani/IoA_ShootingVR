using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{
    private AsyncOperation _async;
    private Animator _ani;

    [SerializeField] private bool _AutoFadeIn;

    [Space(30)]

    [SerializeField][Header("ライティングのボリュームコンポーネント")] private Volume _volume;
    private ColorAdjustments _ColorAdjust;
    [SerializeField][Header("フェード時の背景色")]private Color _FilterColor = Color.white;
    [SerializeField, Range(0f, 1f)] private float _BlackFade;

    [Space(30)]
    [SerializeField]
    [Header("ポーズUI")] private CanvasGroup _PoseGroup;
    [SerializeField, Range(0f, 1f)] private float _PoseAlpha;

    [Space(30)]
    [SerializeField][Header("読み込みゲージ")] private Image _Gage;

    [Space(30)]
    [SerializeField][Header("フェードイン終了時のイベント")] private UnityEvent _FadeInEvent;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        if (_volume) _volume.profile.TryGet(out _ColorAdjust);
        if (_AutoFadeIn) FinFadeIn();
    }

    private void Update()
    {
        if (!_ColorAdjust) return;
        if (_PoseGroup) _PoseGroup.alpha = _PoseAlpha;
        _ColorAdjust.colorFilter.value = Color.Lerp(_FilterColor, Color.black, _BlackFade);
    }

    public void StartFadeIn(int nextScene)
    {

        transform.position = Camera.main.transform.TransformPoint(new Vector3(0, 0, 1f));
        var aim = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(aim);

        _ani.SetTrigger("FadeOut");

        StartCoroutine(LoadScene(nextScene));
    }

    public void Pose()
    {
        Debug.Log("ポーズ");
        GM.instance.IsPose = !GM.instance.IsPose;
        _ani.SetBool("Pose", GM.instance.IsPose);
        if (GM.instance.IsPose)
        {
            StageManager.instance.TimeScale = 0f;
            //StageManager.instance.OriginCmaeraChange(1);
            if (_PoseGroup) _PoseGroup.blocksRaycasts = true;
        }
        else
        {
            StageManager.instance.TimeScale = 1f;
            //StageManager.instance.OriginCmaeraChange(0);
            if (_PoseGroup) _PoseGroup.blocksRaycasts = false;
        }
    }

    IEnumerator LoadScene(int SceneNum)
    {
        _async = SceneManager.LoadSceneAsync(SceneNum);
        _async.allowSceneActivation = false;


        while (_async.progress < 0.9f)
        {
            if (_Gage) _Gage.fillAmount = _async.progress;
            yield return null;
        }

        GM.instance.SceneReset();
        yield return new WaitForSeconds(0.5f);
        Debug.Log($"シーン{SceneNum}をロード完了！");
        if (_Gage) _Gage.fillAmount = 1f;
        _async.allowSceneActivation = true;
    }

    public void FinFadeIn()
    {
        _ani.SetTrigger("FadeIn");
    }

    private void _FadeInEveClip()
    {
        _FadeInEvent.Invoke();
    }
}
