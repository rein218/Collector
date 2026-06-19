using DG.Tweening;
using UnityEngine;

public class AdThing : MonoBehaviour
{
    [SerializeField] private int whichRewarded;
    [SerializeField] private GameObject rotate;
    [SerializeField] private GameObject increase;
    [SerializeField] private float duration;
    private bool _interactible = false;
    private bool _isHiding = false;
    private Vector3 _targetSize;
    private Sequence _mainSequence;

    void Start()
    {
        // Исходный масштаб объектов
        _targetSize = transform.localScale;

        // Сжаты в 0
        increase.transform.localScale = Vector3.zero;
        rotate.transform.localScale = Vector3.zero;

        float appearTime = 1f;

        _mainSequence = DOTween.Sequence();

        // Появление
        _mainSequence.Join(increase.transform.DOScale(_targetSize, appearTime).SetEase(Ease.OutBack));
        _mainSequence.Join(rotate.transform.DOScale(_targetSize, appearTime).SetEase(Ease.OutBack));
        _mainSequence.Join(rotate.transform.DORotate(new Vector3(0, 0, 360), appearTime, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack));

        // После появления – активация и вечное вращение
        _mainSequence.AppendCallback(() =>
        {
            if (_isHiding) return;
            _interactible = true;
            rotate.transform
                .DORotate(new Vector3(0, 0, 360), 4f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        });

        // Ждём оставшееся время до автоматического исчезновения
        float remaining = duration - appearTime;
        if (remaining > 0f)
            _mainSequence.AppendInterval(remaining);

        // Автоматическое исчезновение по времени
        _mainSequence.AppendCallback(() =>
        {
            if (!_isHiding) Hide();
        });
    }

    public void Interact()
    {
        if (!_interactible || _isHiding) return;

        // Убиваем главный таймер, чтобы не сработал автозавершённый Hide
        _mainSequence?.Kill();
        Hide();
        AdController.Instance?.StartAD(whichRewarded);
    }

    private void Hide()
    {
        if (_isHiding) return;
        _isHiding = true;
        _interactible = false;

        // Останавливаем вечное вращение
        rotate.transform.DOKill();

        Sequence hideSequence = DOTween.Sequence();

        hideSequence.Join(increase.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        hideSequence.Join(rotate.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));

        // После завершения анимации уничтожаем объект
        hideSequence.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        // Гарантированно убиваем все твины, если объект удалён принудительно
        transform.DOKill();
        if (increase != null) increase.transform.DOKill();
        if (rotate != null) rotate.transform.DOKill();
    }
}
