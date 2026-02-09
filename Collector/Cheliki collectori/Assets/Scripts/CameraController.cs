using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cameraComp;
    [Header("start")]
    [SerializeField] private Vector3 _firstPos;
    [SerializeField] private float _firstZoom;
    [Header("end")]

    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _endZoom;
    [SerializeField] private float _steps;
    private float _flipCount;
    private Coin _coin;
    public void FirstStart(Coin coin)
    {
        this._coin = coin;
        _coin.OnCoinFlipStart += ZoomOut;
        transform.position = _firstPos;
        cameraComp.orthographicSize= _firstZoom;
    }

    public void ZoomOut(int coinValue, Vector2 position)
    {
        _flipCount++;
        var currZoom = Mathf.Lerp(cameraComp.orthographicSize, _endZoom, _flipCount/_steps/2.2f);
        
        if (_flipCount < _steps)
        {
            StartCoroutine(ZoomOutSlowly(currZoom));
        }
        else    if (_flipCount >= _steps)
        {
            currZoom = _endZoom;
            _coin.OnCoinFlipStart -= ZoomOut;
            StartCoroutine(ZoomOutSlowly(currZoom, 0.6f));
        }  
        
        if(_flipCount == _steps)
        {
            StartCoroutine(MoveSlowly(_endPos, 0.6f));
        }
        
    }
    IEnumerator MoveSlowly(Vector3 currPos, float duration = 0.3f)
    {
        Vector3 startPos = transform.position;

        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            transform.position = Vector3.Lerp(startPos, currPos, t);
            
            yield return null;
        }

        transform.position = currPos;
    }


    IEnumerator ZoomOutSlowly( float currZoom, float duration = 0.2f)
    {
        
        float startZoom = cameraComp.orthographicSize;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            cameraComp.orthographicSize = Mathf.Lerp(startZoom, currZoom, t);
            
            yield return null;
        }
        
        
        cameraComp.orthographicSize = currZoom;
    }
}
