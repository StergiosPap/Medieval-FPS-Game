using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    public float minDistance = 3f;
    public float moveSpeed = 3f;
    public int damage = 5; //Damage dealt to the hero
    public int immuneCounter; //Whenever the enemy takes damage he becomes immune for a few seconds and cannot take damage in the next few frames
    private bool canDealDamage; //Check if player can take damage
    private Transform orc;
    private PlayerInfo playerInfo;
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        playerInfo = FindObjectOfType<PlayerInfo>();
        player = GameObject.Find("PlayerBody").transform;
        orc = this.transform;
        immuneCounter = 1;
        canDealDamage = true;
    }

    void Update()
    {
        //Move towards the player
        transform.LookAt(player);

        if (Vector3.Distance(player.position, transform.position) > minDistance) //Whenever it reaches a certain distance from the player, it stops approaching.
        {
            if (!anim.GetBool("isWalking"))
            {
                anim.SetBool("isWalking", true);
            }
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (anim.GetBool("isWalking"))
            {
                anim.SetBool("isWalking", false);
            }

            if (canDealDamage)
            {
                InflictDamage();
                canDealDamage = false;
            }
        }

        if (!canDealDamage)
        {
            Countdown();
        }
    }

    //Countdown until the enemy can take damage again
    void Countdown()
    {
        immuneCounter--;
        if (immuneCounter == 0) //Can deal damage
        {
            canDealDamage = true;
            immuneCounter = 15; //Countdown
        }
    }
    
    void InflictDamage()
    {
        playerInfo.GainHealth(-damage);
    }
    
}