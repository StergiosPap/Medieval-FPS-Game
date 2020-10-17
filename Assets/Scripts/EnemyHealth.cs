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
        slider.value = CalculateHealth();
        barColor.color = CalculateColor();

        if (health < maxhealth)
        {
            healthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            playerInfo.GainExp(20);
            Destroy(gameObject);
            
        }

        if (health > maxhealth)
        {
            health = maxhealth;
        }


    }
    

    //Χρειαζόμαστε τιμή μεταξύ 0 και 1 για το ProgressBar.
    float CalculateHealth()
    {
        return health / maxhealth;
    }


    //Βρίσκει το χρώμα του Health Bar ανάλογα με το ποσοστό ζωής.
    Color32 CalculateColor()
    {
        if (health / maxhealth >= 0.75) //Health 75-100%
        {
            return new Color32(0, 255, 0, 150); //Πράσινο
        }
        else if (health / maxhealth >= 0.5) //Health 50-75%
        {
            return new Color32(255, 255, 0, 150); //Κίτρινο
        }
        else if (health / maxhealth >= 0.25) //Health 25-50%
        {
            return new Color32(255, 128, 0, 150); //Πορτοκαλί
        }
        else //Health 0-25%
        {
            return new Color32(255, 0, 0, 150); //Κόκκινο
        }
    }


    //Ζημιά που θα γίνεται μετά από χτύπημα με το σπαθί.
    public void TakeDamage(int damage)
    {
        health -= damage;
        orcSound.Play();
    }

}
