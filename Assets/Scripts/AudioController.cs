using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    public Sprite sound;
    public Sprite mute;
    public AudioSource audioSource;
    public Image image;
    public Slider volumeSlider;

    private bool isMuted = false;

    public void ChangeSound() {
        isMuted = !isMuted;

        if(isMuted) {
            audioSource.volume = 0f;
            image.sprite = mute;
        }
        else {
            audioSource.volume = volumeSlider.value;
            image.sprite = sound;
        }
    }

    public void VolumeChange(float volume) {
        if(!isMuted)
            audioSource.volume = volume;
    }
}