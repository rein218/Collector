using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoldBackGround : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Image back;
    [SerializeField] private float fadeDuration = 0.5f; // длительность одного фейда (вход/выход)

    private Sequence _currentSequence;

    private void OnEnable()
    {
        EventBus.OnMultiplyTrigger += TriggerEffect;
    }

    private void OnDisable()
    {
        EventBus.OnMultiplyTrigger -= TriggerEffect;
        KillEffect();
        // Мгновенно скрываем фон при выключении
        back.DOKill();
        back.DOFade(0f, 0f);
    }

    private void TriggerEffect(int duration)
    {
        // Убиваем предыдущую анимацию, если есть
        KillEffect();

        // Запускаем частицы
        if (particle != null)
            particle.Play();

        // Создаём новую Sequence
        _currentSequence = DOTween.Sequence()
            .Append(back.DOFade(1f, fadeDuration))
            .AppendInterval(duration)
            .Append(back.DOFade(0f, fadeDuration))
            .OnComplete(() =>
            {
                // Останавливаем частицы после завершения всей анимации
                if (particle != null)
                    particle.Stop();
                _currentSequence = null;
            })
            .SetAutoKill(true); // автоматически убиваем при завершении
    }

    private void KillEffect()
    {
        if (_currentSequence != null && _currentSequence.IsActive())
        {
            _currentSequence.Kill();
            _currentSequence = null;
        }
        // Если Sequence прервана, сами остановим частицы
        if (particle != null && particle.isPlaying)
            particle.Stop();
    }
}