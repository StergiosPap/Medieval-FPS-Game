using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private Collider swordCollider;
    private EnemyHealth enemyHealth;
    public int immuneCounter; //Μόλις ο ήρωας δεχτεί ζημιά, γίνεται immune για ορισμένα δευτερόλεπτα για να μην ξαναδεχτεί στα επόμενα frames.
    private bool canDealDamage; //Έλεγχος για το αν μπορεί να δεχτεί ζημιά (ο ήρωας).
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

           
    //Αν το σπαθί πετύχει Enemy, του κάνει ζημιά.
    void InflictDamage(GameObject enemy)
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
        enemyHealth.TakeDamage(baseDamage + (playerInfo.GetLevel()-1)*2); //Αυξανόμενη ζημιά ανάλογα με το level. Στο level 1 το σπαθί κάνει 10 πόντους ζημιάς, μετά 12,14,16 κλπ.

        for (int i = 0; i < 200; i++)
        {
            enemy.transform.position -= enemy.transform.forward * 1f * Time.deltaTime;
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



    private void OnTriggerExit(Collider c)
    {
        if (c.tag == "Enemy")
        {
            swordCollider.enabled = false;
            c.enabled = true;
        }
    }
}
