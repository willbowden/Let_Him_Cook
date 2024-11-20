using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioController : MonoBehaviour
{

    [Header("List of Tracks")]
    [SerializeField] private Track[] audioTracks;
    private int trackIndex;

    [Header("Text UI")]
    [SerializeField] private TMP_Text trackTextUI;

    private AudioSource radioAudioSource;



    private void Start() {
        radioAudioSource = GetComponent<AudioSource>();

        trackIndex = 0;

        radioAudioSource.clip = audioTracks[trackIndex].trackAudioClip;

        trackTextUI.text = audioTracks[trackIndex].name;

    }

    public void SkipForwardButton() {

        if (trackIndex < (audioTracks.Length - 1)) {
            trackIndex++;

            StartCoroutine(FadeOut(radioAudioSource, 0.5f));
        }
    }

    public void SkipBackwardButton() {

        if (trackIndex >= 1) {
            trackIndex--;

            StartCoroutine(FadeOut(radioAudioSource, 0.5f));
        } 
    }

    void UpdateTrack(int index) {
        radioAudioSource.clip = audioTracks[index].trackAudioClip;

        trackTextUI.text = audioTracks[index].name;
    }

    public void AudioVolume(float volume) {
        radioAudioSource.volume = volume;
    }

    public void PlayAudio() {
        StartCoroutine(FadeIn(radioAudioSource, 0.5f));
    }

    public void PauseAudio() {
        radioAudioSource.Pause();
    }

    public void StopAudio() {
        StartCoroutine(FadeOut(radioAudioSource, 0.5f));
    }

    public IEnumerator FadeOut(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0 ) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        UpdateTrack(trackIndex);
    }

    public IEnumerator FadeIn(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < startVolume ) {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = startVolume;
        UpdateTrack(trackIndex);
    }
}
