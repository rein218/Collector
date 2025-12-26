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



    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat(VOL_MASTER, Mathf.Log10(level)*20);
        volMaster = Mathf.Log10(level)*20;
        Save();
    }

    public float GetMasterVolume()
    {
        _audioMixer.GetFloat(VOL_MASTER, out var g);
        return g;
    }

    public void SetSoundVolume(float level)
    {
        _audioMixer.SetFloat(VOL_SFX, Mathf.Log10(level)*20);
        volSFX = Mathf.Log10(level)*20;
    }

    public float GetSoundVolume()
    {
        _audioMixer.GetFloat(VOL_SFX, out var g);
        return g;
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat(VOL_MUSIC, Mathf.Log10(level)*20);
        volMusic = Mathf.Log10(level)*20;
    }
    
    public float GetMusicVolume()
    {
        _audioMixer.GetFloat(VOL_MUSIC, out var g);
        return g;
    }

    public void Save()
    {
		PlayerPrefs.SetFloat(VOL_MASTER, volMaster);
		PlayerPrefs.SetFloat(VOL_SFX, volSFX);
        PlayerPrefs.SetFloat(VOL_MUSIC, volMusic);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(VOL_MASTER))
			volMaster = PlayerPrefs.GetFloat(VOL_MASTER);
        if (PlayerPrefs.HasKey(VOL_SFX))
			volSFX = PlayerPrefs.GetFloat(VOL_SFX);
		if (PlayerPrefs.HasKey(VOL_MUSIC))
			volMusic = PlayerPrefs.GetFloat(VOL_MUSIC);
    }
}
