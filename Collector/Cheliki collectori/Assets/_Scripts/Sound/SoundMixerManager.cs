using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private float volMaster = 1f;
    private float volSFX = 1f;
	private float volMusic = 1f;
    private const string VOL_MASTER = "masterVolume";
	private const string VOL_SFX = "soundFXVolume";
    private const string VOL_MUSIC = "musicVolume";

    

    public float GetMasterVolume() 
    {
        return volMaster;
    }

    public float GetSoundVolume()
    {
        return volSFX;
    }

    public float GetMusicVolume()
    {
        return volMusic;
    }



    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat(VOL_MASTER, Mathf.Log10(level)*20f);
        volMaster = level;
    }

    public void SetSoundVolume(float level)
    {
        _audioMixer.SetFloat(VOL_SFX, Mathf.Log10(level)*20f);
        volSFX = level;
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat(VOL_MUSIC, Mathf.Log10(level)*20f);
        volMusic = level;
    }
    
   

    public void Save()
    {
		PlayerPrefs.SetFloat(VOL_MASTER, volMaster);
		PlayerPrefs.SetFloat(VOL_SFX, volSFX);
        PlayerPrefs.SetFloat(VOL_MUSIC, volMusic);
    }

    public void Load()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(VOL_MASTER,1f));
        SetSoundVolume(PlayerPrefs.GetFloat(VOL_SFX,1f));
		SetMusicVolume(PlayerPrefs.GetFloat(VOL_MUSIC,1f));
    }
}
