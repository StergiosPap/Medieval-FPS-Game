using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private Collider swordCollider;
    private EnemyHealth enemyHealth;
    public int immuneCounter; //Whenever the player takes damage he becomes immune for a few seconds and cannot take damage in the next few frames
    private bool canDealDamage; //Checks if the player can get hit
    private int baseDamage = 10;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        swordCollider = GetComponent<Collider>();
        canDealDamage = true;
        immuneCounter = 1;
    }


    void Update()
    {
        if (!canDealDamage)
        {
            Countdown();
        }
    }

    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            GameObject enemy = collider.gameObject;
            if (canDealDamage)
            {
                InflictDamage(enemy);
                canDealDamage = false;
            }
           
        }
    }


    void InflictDamage(GameObject enemy)
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
        enemyHealth.TakeDamage(baseDamage + (playerInfo.GetLevel()-1)*2); //Increasing damage depending on the level. In level 1 it deals 10 damage, then 12, 14, 16 etc.

        for (int i = 0; i < 200; i++)
        {
            enemy.transform.position -= enemy.transform.forward * 1f * Time.deltaTime;
        }
    }


    //Countdown until the enemy can take damage again
    void Countdown()
    {
        immuneCounter--;
        if (immuneCounter == 0) //Can take damage
        {
            canDealDamage = true;
            immuneCounter = 15; //Countdown
        }
    }


    private void OnTriggerExit(Collider c)
    {
        if (c.tag == "Enemy")
        {
            swordCollider.enabled = false;
            c.enabled = true;
        }
    }
}
