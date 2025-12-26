
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager instance;
    [SerializeField] private AudioSource _soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
		{
			Destroy(gameObject);
		}
    }


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float minPitch, float maxPitch)
    {
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }
    
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rnd = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rnd];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, float minPitch, float maxPitch)
	{
        int rnd = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rnd];
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
	}
}
