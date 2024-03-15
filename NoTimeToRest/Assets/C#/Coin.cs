using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int addCoin = 1; // Or any value you desire

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.coinSource.PlayOneShot(SoundManager.instance.coinSound);
            PlayerController player = other.GetComponent<PlayerController>();
            player.currentCoin += addCoin;
            Destroy(gameObject); // Destroy the coin item after collection
        }
    }
}