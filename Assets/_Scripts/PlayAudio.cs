using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

    }
}