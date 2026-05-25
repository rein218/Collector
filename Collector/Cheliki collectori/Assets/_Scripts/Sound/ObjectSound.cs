using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;
    [Header("pitch")]
    [SerializeField] private float _min = 0.8f;
    [SerializeField] private float _max = 1.2f;
    private SoundFxManager _instance;

    public void PlaySound()
    {
        if (_instance == null) _instance = SoundFxManager.instance;
        _instance.PlaySoundFXClip(_audioClip, transform, _volume, _min, _max);
    }
}
