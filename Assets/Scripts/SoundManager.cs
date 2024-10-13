using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    [Header("Sound")]
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupSound;
    public AudioSource walkSound;
    public AudioSource treeFallingSound;
    [Header("Music")]
    public AudioSource startingZoneBGMusic;


    public void PlaySound(AudioSource sound)
    {
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
}
