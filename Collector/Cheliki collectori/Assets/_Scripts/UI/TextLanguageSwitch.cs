using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class TextLanguageSwitch : MonoBehaviour
{
    private string _pendingLanguageCode;
    private void Awake()
    {
        YG2.onSwitchLang += OnYandexLanguageChanged;
        if (!string.IsNullOrEmpty(YG2.lang))
            _pendingLanguageCode = YG2.lang;

        StartCoroutine(InitLocalizationAndApply());
    }


    private void OnDestroy()
    {
        YG2.onSwitchLang -= OnYandexLanguageChanged;
    }

    private void OnYandexLanguageChanged(string newLang)
    {
        _pendingLanguageCode = newLang;
        // Если локализация уже готова, применяем сразу
        if (LocalizationSettings.InitializationOperation.IsValid() && LocalizationSettings.InitializationOperation.IsDone)
            ApplyLanguage(_pendingLanguageCode);
        
    }

    private IEnumerator InitLocalizationAndApply()
    {
        var initOp = LocalizationSettings.InitializationOperation;
        if (initOp.IsValid() && !initOp.IsDone)
            yield return initOp;

        if (!string.IsNullOrEmpty(_pendingLanguageCode))
            ApplyLanguage(_pendingLanguageCode);
    }

    private void ApplyLanguage(string code)
    {
        var locale = GetLocaleByCode(code);
        if (locale != null)
            LocalizationSettings.SelectedLocale = locale;
    }


    private Locale GetLocaleByCode(string code)
    {
        code = code.Trim();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (string.Equals(locale.Identifier.Code, code, StringComparison.InvariantCultureIgnoreCase))
                return locale;
        }
        return null;
    }
}