using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [SerializeField] private Camera cam;
    private int prevWidth, prevHeight;
    [Header("start")]
    [SerializeField] private Vector3 _firstPos;
    [SerializeField] private float _firstZoom;
    [Header("end")]

    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _endZoom;
    [SerializeField] private float _steps;
     [Header("Целевая ширина игрового поля (в мировых единицах)")]
    [SerializeField] private float targetWidth = 18f;

    [Header("Минимальная и максимальная допустимая высота")]
    [SerializeField] private float minHeight = 10f;  // чтобы на узких экранах объекты не мельчали
    [SerializeField] private float maxHeight = 30f;  // чтобы на широких экранах не было слишком много пустоты
    private float _flipCount;
    private bool _doNotAdjustCam = false;

    public void Awake()
    {
        if(instance == null) instance = this;
        if(cam == null) cam = FindAnyObjectByType<Camera>();
    }

    public void Init()
    {
        if(GameManager.Instance.FirstStart()) FirstStart();
        else NormalStart();
    }


    public void NormalStart()
    {
        if(cam == null) return;
        AdjustCamera();
    }

    public void FirstStart()
    {
        if(cam == null) return;
        _doNotAdjustCam = true;
        EventBus.OnCoinFlipStart += ZoomOut;
        cam.transform.position = _firstPos;
        cam.orthographicSize= _firstZoom;
    }

    void OnDisable()
    {
        EventBus.OnCoinFlipStart -= ZoomOut;
    }

    void Update()
    {
        if(cam == null) return;
        if (_doNotAdjustCam) return;
        if (Screen.width != prevWidth || Screen.height != prevHeight)
        {
            AdjustCamera();
            prevWidth = Screen.width;
            prevHeight = Screen.height;
        }
    }

    public void ZoomOut(Vector2 position)
    {
        
        _flipCount++;
        var currZoom = Mathf.Lerp(cam.orthographicSize, _endZoom, _flipCount/_steps/2.2f);
        
        if (_flipCount < _steps)
        {
            StartCoroutine(ZoomOutSlowly(currZoom));
        }
        else if (_flipCount >= _steps)
        {
            currZoom = _endZoom;
            EventBus.OnCoinFlipStart -= ZoomOut;
            AdjustCamera(true); 
        }  
        
        if(_flipCount == _steps)
        {
            StartCoroutine(MoveSlowly(_endPos, 0.6f));
        }
    }

    IEnumerator MoveSlowly(Vector3 currPos, float duration = 0.3f)
    {
        Vector3 startPos = cam.transform.position;

        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            cam.transform.position = Vector3.Lerp(startPos, currPos, t);
            
            yield return null;
        }

        cam.transform.position = currPos;
    }

    IEnumerator ZoomOutSlowly( float currZoom, float duration = 0.2f)
    {
        
        float startZoom = cam.orthographicSize;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            cam.orthographicSize = Mathf.Lerp(startZoom, currZoom, t);
            
            yield return null;
        }
        
        
        cam.orthographicSize = currZoom;
    }

    void AdjustCamera(bool likeSlow = false)
    {
        if (!cam.orthographic) return;

        float screenRatio = (float)Screen.width / Screen.height;

        // Высота, необходимая для сохранения targetWidth при текущем соотношении экрана
        float requiredHeight = targetWidth / screenRatio;

        // Определяем фактическую высоту, которую будет показывать камера (с учётом ограничений)
        float actualHeight = Mathf.Clamp(requiredHeight, minHeight, maxHeight);


        // Устанавливаем orthographicSize (половина высоты)
        var size = actualHeight / 2f;
        if (likeSlow)
        StartCoroutine(ZoomOutSlowly(size, 0.6f));
        else
        cam.orthographicSize = size;  
        _doNotAdjustCam = false;
    }
}
