using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    public static SoundManager2 instance;
    public AudioSource audioSoundSource;
    public AudioClip overSound;
    public AudioClip winSound;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSoundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
