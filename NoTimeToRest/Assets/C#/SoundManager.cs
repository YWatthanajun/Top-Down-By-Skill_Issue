using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSoundSource;
    public AudioClip coinSound;
    public AudioClip shieldSound;
    public AudioClip damageSound;
    public AudioClip breakShieldSound;
    public AudioClip DashSound;
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
