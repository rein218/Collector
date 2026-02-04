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
        _coin.OnCoinFlip += ZoomOut;
        transform.position = _firstPos;
        cameraComp.orthographicSize= _firstZoom;
    }

    public void ZoomOut()
    {
        _flipCount++;
        var currPos = Vector3.Lerp(transform.position, _endPos, _flipCount/_steps);
        var currZoom = Mathf.Lerp(cameraComp.orthographicSize, _endZoom, _flipCount/_steps);
        if (_flipCount >= _steps)
        {
            currPos = _endPos;
            currZoom = _endZoom;
            _coin.OnCoinFlip -= ZoomOut;
        }  
        StartCoroutine(FirstStart(currPos, currZoom));
    }

    IEnumerator FirstStart(Vector3 currPos, float currZoom, float duration = 0.2f)
    {
        Vector3 startPos = transform.position;
        float startZoom = cameraComp.orthographicSize;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            transform.position = Vector3.Lerp(startPos, currPos, t);
            cameraComp.orthographicSize = Mathf.Lerp(startZoom, currZoom, t);
            
            yield return null;
        }
        
        transform.position = currPos;
        cameraComp.orthographicSize = currZoom;
    }
}
