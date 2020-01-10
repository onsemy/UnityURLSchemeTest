using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Text _text;
    private bool _isOut;

    private void Awake()
    {
        _text = GetComponent<Text>();
        if (null == _text)
        {
            _text = GetComponentInChildren<Text>();
        }
    }

    public void OnClickURL()
    {
        Debug.Log($"[Test::OnClickURL] {_text.text}");
        Application.OpenURL(_text.text);
    }

    public void OnClickURLWithCoroutine()
    {
        Debug.Log($"[Test::OnClickURLWithCoroutine] {_text.text}");

        _isOut = false;
        Application.OpenURL(_text.text);

        StartCoroutine(WaitForOpen());
    }

    private IEnumerator WaitForOpen()
    {
        var startTime = Time.realtimeSinceStartup;
        while (_isOut == false)
        {
            if (Time.realtimeSinceStartup - startTime > 1f)
            {
                var split = _text.text.Split('/');
                split = split.Last().Split('?');
                // TODO(JJO): 에러니까 웹브라우져를 이용해서 접속한다는 팝업 알림을 띄움.
                Debug.LogError($"[Test::WaitForOpen] Time out! Try to alternative URL - https://onesto.re/{split[0]}");
                Application.OpenURL($"https://onesto.re/{split[0]}");

                break;
            }
            
            yield return null;
        }
        
        Debug.Log($"[Test::WaitForOpen] Success - {_text.text}");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log($"[Test::OnApplicationPause] {pauseStatus}");
        
        if (pauseStatus)
        {
            if (_isOut == false)
            {
                _isOut = true;
            }
        }
    }
}
