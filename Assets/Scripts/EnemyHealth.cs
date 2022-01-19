using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxhealth;

    public GameObject healthBarUI;
    public Slider slider;
    public Image barColor;

    private PlayerInfo playerInfo;

    public AudioSource orcSound;


    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        health = maxhealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        //Update health bar
        slider.value = CalculateHealth();
        barColor.color = CalculateColor();

        if (health < maxhealth)
        {
            healthBarUI.SetActive(true); //When it takes damage for the first time, the health bar appears.
        }

        if (health <= 0)
        {
            playerInfo.GainExp(20); //Give exp points to player
            Destroy(gameObject); //Destroy enemy object
            
        }

        if (health > maxhealth)
        {
            health = maxhealth; //Cannot exceed maxhealth
        }

    }
    

    //Return a value between 0 and 1 for ProgressBar
    float CalculateHealth()
    {
        return health / maxhealth;
    }


    //Finds the appropriate color for the Health Bar, depending on the remaining health percentage
    Color32 CalculateColor()
    {
        if (health / maxhealth >= 0.75) //Health 75-100%
        {
            return new Color32(0, 255, 0, 150); //Green
        }
        else if (health / maxhealth >= 0.5) //Health 50-75%
        {
            return new Color32(255, 255, 0, 150); //Yellow
        }
        else if (health / maxhealth >= 0.25) //Health 25-50%
        {
            return new Color32(255, 128, 0, 150); //Orange
        }
        else //Health 0-25%
        {
            return new Color32(255, 0, 0, 150); //Red
        }
    }
    

    //Damage dealt to enemy after getting hit with the sword
    public void TakeDamage(int damage)
    {
        health -= damage;
        orcSound.Play();
    }

}
