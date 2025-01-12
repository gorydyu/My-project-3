using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float playbackInterval;

    public void PlayTypingSound()
    {
        if (source.isPlaying && source.time < playbackInterval)
            return;

        source.Play();
    }
}
