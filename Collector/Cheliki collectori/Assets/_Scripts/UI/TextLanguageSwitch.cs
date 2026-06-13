using System;
using UnityEngine;
using YG;
using UnityEngine.Localization.Settings;

[Serializable]
public class TextLanguageSwitch : MonoBehaviour
{

    void Start()
    {
        YG2.onSwitchLang += SetLanguageFromYG2;
        SetLanguageFromYG2(YG2.lang);
    }

    void OnDisable()
    {
        YG2.onSwitchLang -= SetLanguageFromYG2;
    }

    private void SetLanguageFromYG2(string yandexLanguageCode)
    {
        var locale = GetLocaleByCode(yandexLanguageCode);
        if (locale != null)
        {
            // 4. Переключаем Unity Localization на эту Locale
            LocalizationSettings.SelectedLocale = locale;
        }
        else
        {
            Debug.LogWarning($"Локаль для кода '{yandexLanguageCode}' не найдена.");
        }

    }
    private UnityEngine.Localization.Locale GetLocaleByCode(string code)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            // Сравниваем коды, приводя к нижнему регистру для надежности
            if (string.Equals(locale.Identifier.Code, code, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return locale;
            }
        }
        return null;
    }
}
