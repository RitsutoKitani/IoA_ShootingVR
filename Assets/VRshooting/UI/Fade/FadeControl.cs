using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{
    private AsyncOperation _async;
    private Animator _ani;

    [Space(30)]

    [SerializeField]
    private Volume _volume;
    private ColorAdjustments _ColorAdjust;
    [SerializeField]
    private Color _FilterColor = Color.white;
    [SerializeField, Range(0f, 1f)] private float _BlackFade;

    [Space(30)]
    [SerializeField]
    private Image _Gage;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        if (_volume) _volume.profile.TryGet(out _ColorAdjust);
    }

    private void Update()
    {
        if (!_ColorAdjust) return;
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

    IEnumerator LoadScene(int SceneNum)
    {
        _async = SceneManager.LoadSceneAsync(SceneNum);
        _async.allowSceneActivation = false;


        while (_async.progress < 0.9f)
        {
            if (_Gage) _Gage.fillAmount = _async.progress;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Debug.Log($"シーン{SceneNum}をロード完了！");
        if (_Gage) _Gage.fillAmount = 1f;
        _async.allowSceneActivation = true;
    }
}
