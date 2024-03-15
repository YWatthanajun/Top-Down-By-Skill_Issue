using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    public int shieldHealthToAdd = 1; // Or any value you desire

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.audioSoundSource.PlayOneShot(SoundManager.instance.shieldSound);
            PlayerController player = other.GetComponent<PlayerController>();
            player.currentShieldHealth += shieldHealthToAdd;
            // TODO: Play shield pickup effect or animation
            Destroy(gameObject); // Destroy the shield item after collection
        }
    }
}
