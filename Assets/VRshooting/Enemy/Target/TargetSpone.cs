using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TargetSpone : MonoBehaviour
{
    [SerializeField]
    private bool Active;
    [SerializeField]
    private GameObject _TargetObj;
    [SerializeField]
    private Vector3 _SponeArea;
    [SerializeField]
    private int _SponeMax;
    [SerializeField]
    private float _SponeInterval;

    private float _timer = 0f;

    private List<GameObject> _TargetList = new List<GameObject>();

    private void Update()
    {
        for (int i = 0; i < _TargetList.Count; i++) if (!_TargetList[i]) _TargetList.RemoveAt(i);

        if(!Active) return;

        if (_TargetList.Count < _SponeMax)
        {
            if (_timer < _SponeInterval) _timer += Time.deltaTime;
            else
            {
                float x = Random.Range(-_SponeArea.x / 2, _SponeArea.x / 2);
                float y = Random.Range(-_SponeArea.y / 2, _SponeArea.y / 2);
                float z = Random.Range(-_SponeArea.z / 2, _SponeArea.z / 2);
                GameObject tartget = Instantiate(_TargetObj, transform.position + new Vector3(x, y, z), Quaternion.identity);
                if(!tartget.activeSelf) tartget.SetActive(true);
                _TargetList.Add(tartget);
                _timer = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _SponeArea);
    }

}
