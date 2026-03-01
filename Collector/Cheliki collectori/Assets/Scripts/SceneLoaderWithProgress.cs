using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderWithProgress : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private string sceneToLoad = "NextScene"; // Имя загружаемой сцены
    [SerializeField] private Slider progressSlider;            // Слайдер для отображения прогресса
    [SerializeField] private bool autoActivate = true;         // Автоматически активировать сцену после загрузки

    private AsyncOperation loadOperation;

    void Start()
    {
        if (progressSlider != null)
            progressSlider.value = 0f;

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Начинаем асинхронную загрузку сцены
        loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Если autoActivate = false, запрещаем автоматическую активацию (останавливаемся на 90%)
        if (!autoActivate)
            loadOperation.allowSceneActivation = false;

        // Пока операция не завершена
        while (!loadOperation.isDone)
        {
            float progressValue;

            if (autoActivate)
            {
                // При autoActivate = true прогресс идёт от 0 до 1
                progressValue = loadOperation.progress;
            }
            else
            {
                // При allowSceneActivation = false прогресс останавливается на 0.9
                // Нормализуем значение, чтобы шкала заполнилась до 100% перед активацией
                progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            }

            // Обновляем слайдер
            if (progressSlider != null)
                progressSlider.value = progressValue;

            // Если загрузка достигла 90% и autoActivate = false, разрешаем активацию
            if (!autoActivate && loadOperation.progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}