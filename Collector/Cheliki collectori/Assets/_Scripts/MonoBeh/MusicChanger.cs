using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> musicTracks;
    [SerializeField] private float delayBetweenTracks = 8f;
    [SerializeField] private float fadeDuration = 2f;
    
    private int currentTrackIndex = 0;
    private Coroutine musicCoroutine;
    
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        StartMusic();
    }
    
    void StartMusic()
    {
        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);
            
        musicCoroutine = StartCoroutine(PlayMusicRoutine());
    }
    
    IEnumerator PlayMusicRoutine()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            if (musicTracks.Count == 0)
                yield break;
                
            // Начинаем трек с fade in
            audioSource.clip = musicTracks[currentTrackIndex];
            audioSource.volume = 0f;
            audioSource.Play();
            
            // Плавное увеличение громкости
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            
            // Ждем пока трек почти закончится
            float trackLength = audioSource.clip.length;
            yield return new WaitForSeconds(trackLength - fadeDuration * 2);
            
            // Плавное уменьшение громкости (fade out)
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                yield return null;
            }
            
            // Задержка между треками
            if (delayBetweenTracks > 0)
                yield return new WaitForSeconds(delayBetweenTracks);
            
            // Переходим к следующему треку
            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;
        }
    }
    
    void OnDestroy()
    {
        if (musicCoroutine != null)
            StopCoroutine(musicCoroutine);
    }
}
