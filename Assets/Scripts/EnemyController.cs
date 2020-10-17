using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    public float minDistance = 3f;
    public float moveSpeed = 3f;
    public int damage = 5; //Η ζημιά που κάνει στον ήρωα.
    public int immuneCounter; //Μόλις ο ήρωας δεχτεί ζημιά, γίνεται immune για ορισμένα δευτερόλεπτα για να μην ξαναδεχτεί στα επόμενα frames.
    private bool canDealDamage; //Έλεγχος για το αν μπορεί να δεχτεί ζημιά (ο ήρωας).
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

    //Μετακίνηση προς τον παίκτη.
    void Update()
    {
        //Debug.Log(Vector3.Distance(player.position, transform.position));

        transform.LookAt(player);

        if (Vector3.Distance(player.position, transform.position) > minDistance) //Μόλις φτάσει στην καθορισμένη απόσταση σταματάει να πλησιάζει.
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


    //Αντίστροφη μέτρηση frames μέχρι να μπορεί να ξαναφάει ζημιά.
    void Countdown()
    {
        immuneCounter--;
        if (immuneCounter == 0) //Μπορεί να ξαναδεχτεί ζημιά.
        {
            canDealDamage = true;
            immuneCounter = 15; //Αντίστροφη μέτρηση frames μέχρι να μπορεί να ξαναφάει ζημιά.
        }
    }


    //Ζημιά στον ήρωα.
    void InflictDamage()
    {
        playerInfo.GainHealth(-damage);
    }


}