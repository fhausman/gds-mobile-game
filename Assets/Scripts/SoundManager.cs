using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] bellSounds;
    public AudioSource bellSource;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PriestBellSound()
    {
        bellSource.PlayOneShot(bellSounds[Random.Range(0, bellSounds.Length)]);
    }
}
