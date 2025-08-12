using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _cgs;
    [SerializeField, ReadOnly] private int _CanvasNum;
    [Header("遷移時間")][SerializeField] private float _transitionTime = 1f;
    private float _timer;
    private CanvasGroup _PreCg; //前アクティブだったキャンバス
    private CanvasGroup _ActCg; //アクティブになったキャンバス

    [Tooltip("0:なし\n1;前のウィンドウが消える\n2:待ち時間\n3:次のウィンドウが現れる")]
    [SerializeField, ReadOnly] private int _step;

    private void Start()
    {
        for (int i = 0; i < _cgs.Length; i++)
        {
            _cgs[i].alpha = i == _CanvasNum ? 1 : 0;
            _cgs[i].blocksRaycasts = _CanvasNum == i;
        }
    }

    private void Update()
    {
        if (_step == 0) return;
        if (_timer < _transitionTime)
        {
            _timer += Time.deltaTime;

            _ActiveCgUpdate();
            _PreCgUpdate();
        }
        else
        {
            if (_step < 3) _step++;
            else _step = 0;
            _timer = 0f;
        }
    }

    /// <summary>
    /// アクティブ状態になるキャンバスの更新
    /// </summary>
    /// <param name="cg"></param>
    private void _ActiveCgUpdate()
    {
        switch(_step)
        {
            case 1:
                _ActCg.alpha = 0; break;

            case 2:
                _ActCg.alpha = 0; break;

            case 3:
                _ActCg.alpha = _timer / _transitionTime; break;
        }
    }

    private void _PreCgUpdate()
    {
        switch (_step)
        {
            case 1:
                _PreCg.alpha = 1 - _timer / _transitionTime; break;

            case 2:
                _PreCg.alpha = 0; break;

            case 3:
                _PreCg.alpha = 0; break;
        }
    }

    public void ChangeWindow(int num)
    {
        if (num == _CanvasNum) return;
        _PreCg = _cgs[_CanvasNum];
        _ActCg = _cgs[num];

        _CanvasNum = num;
        _step = 1;
        _timer = 0f;
        for (int i = 0; i < _cgs.Length; i++) _cgs[i].blocksRaycasts = _CanvasNum == i;
    }
}
