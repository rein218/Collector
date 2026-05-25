using UnityEngine;

public class CameraFixedWidth : MonoBehaviour
{
    [Header("Целевая ширина игрового поля (в мировых единицах)")]
    [SerializeField] private float targetWidth = 18f;

    [Header("Минимальная и максимальная допустимая высота")]
    [SerializeField] private float minHeight = 10f;  // чтобы на узких экранах объекты не мельчали
    [SerializeField] private float maxHeight = 30f;  // чтобы на широких экранах не было слишком много пустоты

    private Camera cam;
    private int prevWidth, prevHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    void Update()
    {
        if (Screen.width != prevWidth || Screen.height != prevHeight)
        {
            AdjustCamera();
            prevWidth = Screen.width;
            prevHeight = Screen.height;
        }
    }

    void AdjustCamera()
    {
        if (!cam.orthographic) return;

        float screenRatio = (float)Screen.width / Screen.height;

        // Высота, необходимая для сохранения targetWidth при текущем соотношении экрана
        float requiredHeight = targetWidth / screenRatio;

        // Определяем фактическую высоту, которую будет показывать камера (с учётом ограничений)
        float actualHeight = Mathf.Clamp(requiredHeight, minHeight, maxHeight);

        // Устанавливаем orthographicSize (половина высоты)
        cam.orthographicSize = actualHeight / 2f;

        // Если requiredHeight выходит за пределы [minHeight, maxHeight], нужно добавить полосы
        if (requiredHeight < minHeight)
        {
            // Экран слишком узкий – требуемая высота меньше минимальной.
            // Будем показывать ровно minHeight, но по бокам появятся чёрные полосы (pillarbox).
            float scaleWidth = requiredHeight / minHeight; // всегда < 1
            //SetRect(scaleWidth, 1f, (1f - scaleWidth) / 2f, 0f);
        }
        else if (requiredHeight > maxHeight)
        {
            // Экран слишком широкий – требуемая высота больше максимальной.
            // Будем показывать ровно maxHeight, но сверху/снизу появятся чёрные полосы (letterbox).
            float scaleHeight = maxHeight / requiredHeight; // всегда < 1
            //SetRect(1f, scaleHeight, 0f, (1f - scaleHeight) / 2f);
        }
        else
        {
            // В пределах нормы – полос не нужно
            //SetRect(1f, 1f, 0f, 0f);
        }
    }

    void SetRect(float widthScale, float heightScale, float xOffset, float yOffset)
    {
        Rect rect = cam.rect;
        rect.width = widthScale;
        rect.height = heightScale;
        rect.x = xOffset;
        rect.y = yOffset;
        cam.rect = rect;
    }
}