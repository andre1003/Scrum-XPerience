using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    public Sprite sound;
    public Sprite mute;
    private bool isMuted = false;
    public AudioSource audioSource;
    public Image image;

    public void ChangeSound() {
        isMuted = !isMuted;

        if(isMuted) {
            audioSource.volume = 0f;
            image.sprite = mute;
        }
        else {
            audioSource.volume = 0.25f;
            image.sprite = sound;
        }
    }
}