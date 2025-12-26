using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;
    private SoundFxManager _instance;
    public void PlaySound()
    {
        if (_instance == null) _instance = SoundFxManager.instance;
        _instance.PlaySoundFXClip(_audioClip, transform, _volume);
    }

}
