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
            PlayerController player = other.GetComponent<PlayerController>();
            player.currentCoin += addCoin;
            // TODO: Play shield pickup effect or animation
            Destroy(gameObject); // Destroy the shield item after collection
        }
    }
}
