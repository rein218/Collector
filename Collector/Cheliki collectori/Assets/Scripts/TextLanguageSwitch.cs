using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using YG;

[Serializable]
public class TextLanguageSwitch : MonoBehaviour
{
    public List<TextPoint> textsList;
    public TMP_Text tMP_Text;

    public void GetAsset(TMP_Text tMP_Text)
    {
        this.tMP_Text = tMP_Text;
        SwitchLanguage(YG2.lang);
    }

    public void GetList(List<TextPoint> textsList)
    {
        this.textsList = textsList;
        SwitchLanguage(YG2.lang);
    }


    void OnEnable()
    {
        YG2.onSwitchLang += SwitchLanguage;
        SwitchLanguage(YG2.lang);
    }

    void OnDisable()
    {
        YG2.onSwitchLang -= SwitchLanguage;
    }

    private void SwitchLanguage(string obj)
    {
        if (tMP_Text==null) return;
        if (textsList==null) return;
        var type = StringToLanguage(obj);
        var newText = textsList.FirstOrDefault(q => q.type == type);

        if (newText == null|| newText.text == null) return;
        tMP_Text.text = newText.text;
        if(newText.fontSize!=0)
        tMP_Text.fontSize = newText.fontSize;
    }



    public LanguageType StringToLanguage(string languageString)
    {
        if (string.IsNullOrEmpty(languageString))
            return LanguageType.en; // Значение по умолчанию
        
        // Безопасная конвертация с игнорированием регистра
        if (Enum.TryParse<LanguageType>(languageString, true, out LanguageType language))
        {
            return language;
        }
        
        // Если строка не распознана - возвращаем значение по умолчанию
        Debug.LogWarning($"Unknown language: '{languageString}', defaulting to 'en'");
        return LanguageType.en;
    }
}

[Serializable]
public class TextPoint
{
    public LanguageType type;
    public string text;
    public float fontSize;
}



public enum LanguageType
{
    ru,
    en
}
