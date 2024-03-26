using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{

    public int shield = 1;
    public int coin = 1;
    public int health = 1;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string inputString = Input.inputString.ToLower();
        if (inputString.Contains("1"))
        {
            // Call the function to add Health
            addHelth();
        }
        if (inputString.Contains("2"))
        {
            // Call the function to add shield
            addShield();
        }
        if (inputString.Contains("3"))
        {
            // Call the function to add Coin
            addCoin();
        }
        if (inputString.Contains("4"))
        {
            // Call the function
            immortal();
        }
        if (inputString.Contains("f"))
        {
            // Call the function
            death();
        }
    }

    void addHelth()
    {
        Debug.Log("(add Health +1)");
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        player.currentHealth += health;
    }
    void addShield()
    {
        Debug.Log("(add Shield +1)");
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        player.currentShieldHealth += shield;
    }
    void addCoin()
    {
        Debug.Log("(add Coin +1)");
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        player.currentCoin += coin;
    }
    void immortal()
    {
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        player.Invulnerable();
    }
    void death()
    {
        Debug.Log("(Player is Death)");
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        player.GameOver();
    }

}
