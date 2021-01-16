using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVariationController : MonoBehaviour
{
    AudioSource mainAudioSource;
    AudioSource secondaryAudioSource;

    [SerializeField] AudioClip mainClip;

    [SerializeField] AnimationCurve increaseVolumeCurve;
    [SerializeField] AnimationCurve decreseVolumeCurve;

    [SerializeField] [Range(0, 10)] float transitionDuration = 2f;
    bool updateTrack;
    float timer = 0f;

    private void Awake()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        mainAudioSource = audioSources[0];
        secondaryAudioSource = audioSources[1];
        RefreshAudioSource(mainAudioSource);
        RefreshAudioSource(secondaryAudioSource);
        
    }

    private void Start()
    {
        PlayMainClip();
    }


    void Update()
    {
        if (updateTrack)
        {
            secondaryAudioSource.time = mainAudioSource.time;
            
            timer += Time.deltaTime;
           
            mainAudioSource.volume = decreseVolumeCurve.Evaluate(timer);
            secondaryAudioSource.volume = increaseVolumeCurve.Evaluate(timer);

            if (timer >= transitionDuration)
            {
                updateTrack = false;
                timer = 0f;

                var tempSource = mainAudioSource;
                mainAudioSource = secondaryAudioSource;
                secondaryAudioSource = tempSource;
                RefreshAudioSource(secondaryAudioSource);
            }
        }
    }

    public void PlayMainClip()
    {
        PlayClip(mainClip);
    }

    public void PlayClip(AudioClip clip)
    {
        if (!mainAudioSource.isPlaying && !secondaryAudioSource.isPlaying)
        {
            mainAudioSource.volume = 1f;
            PlayClip(mainAudioSource, clip);
            
            return;
        }

        PlayClip(secondaryAudioSource, clip);
        updateTrack = true;
    }

    public void Pause()
    {
        RefreshAudioSource(mainAudioSource);
        RefreshAudioSource(secondaryAudioSource);
    }

    public string GetCurrentTrack()
    {
        if (!mainAudioSource.mute && mainAudioSource.clip != null)
            return mainAudioSource.clip.name;

        return "";
    }

    void PlayClip(AudioSource audioSource, AudioClip clip)
    {
        audioSource.mute = false;
        audioSource.clip = clip;
        audioSource.Play();
    }

    void RefreshAudioSource(AudioSource audioSource)
    {
        audioSource.volume = 0;
        audioSource.mute = true;
    }
}
