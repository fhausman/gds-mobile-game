using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] bellSounds;
    public AudioSource bellSource;
    public AudioSource babaSource;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var babas = GameObject.FindGameObjectsWithTag("Slyboot");
        if (babas.Length > 0 && !babaSource.isPlaying)
        {
            babaSource.Play();
        }
        else if (babas.Length == 0)
        {
            babaSource.Stop();
        }
    }

    public void PriestBellSound()
    {
        bellSource.PlayOneShot(bellSounds[Random.Range(0, bellSounds.Length)]);
    }
}
