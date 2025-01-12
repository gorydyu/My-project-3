using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundeffect : MonoBehaviour
{
    [SerializeField] private AudioSource myAudioSource;

    public void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
