using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static int index = 0;
    [SerializeField]
    private AudioClip[] GameMusic;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip chosenAudioClip = GameMusic[index];
        DataTransmission.chosenAudioClip = chosenAudioClip;
        audioSource.clip = chosenAudioClip;

        audioSource.Play();
    }

    private void Update()
    {

    }

    // Update is called once per frame
    public void ChangeMusicForward()
    {
        if (index >= (GameMusic.Length - 1))
        {
            index = -1;
        }
        index++;
        audioSource.clip = GameMusic[index];
        audioSource.Play();


    }

    public void ChangeMusicBackward()
    {
        if (index <= 0)
        {
            index = GameMusic.Length - 1;
        }
        index--;
        audioSource.clip = GameMusic[index];
        audioSource.Play();

        
    }
}
