using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private SoundMixerManager _soundMixerManager;
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _soundVolume;
    [SerializeField] private Slider _musicVolume;


    void OnEnable()
    {
        _soundMixerManager.Load();
        _masterVolume.value = _soundMixerManager.GetMasterVolume();
        _soundVolume.value  = _soundMixerManager.GetSoundVolume(); 
        _musicVolume.value  = _soundMixerManager.GetMusicVolume();
    }

    void OnDisable()
    {
        _soundMixerManager.Save();
    }
}
