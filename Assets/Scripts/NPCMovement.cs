using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed;
    public Transform transform;
    private Vector3 target, looktarget;

    private PlayerInfo playerInfo;
    private Animator anim;

    private bool checkForMovement;

    void Start()
    {
        checkForMovement = true;
        anim = gameObject.GetComponent<Animator>();
        playerInfo = FindObjectOfType<PlayerInfo>();
    }

    void Update()
    {
        //Responsible for NPC animations (walking/not walking)
        if (checkForMovement)
        {
            SetTarget();
            if (Vector3.Distance(transform.position, target) > 1f)
            {
                if (!anim.GetBool("isWalking"))
                {
                    anim.SetBool("isWalking", true);
                }
                transform.LookAt(target);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("isWalking", false);
                transform.LookAt(looktarget);
            }
        }
    }

    //Depending on the story phase, Eliza moves to a certain point and looks in a certain direction when that point is reached.
    private void SetTarget()
    {
        switch (playerInfo.GetPhase())
        {
            case 0: //starting point
                target = new Vector3(3.17f, 3.45f, -30.35f);
                looktarget = new Vector3(0f, 3.45f, -30.35f);
                break;
            case 1: //statue
                target = new Vector3(-0.9f, 3.45f, 72f);
                looktarget = new Vector3(-4f, 3.45f, -36.51f);
                break;
            case 2: //forest
                target = new Vector3(-60f, 3.45f, 85f);
                looktarget = new Vector3(-50f, 3.45f, 85f);
                break;
            case 3: //shops
                target = new Vector3(-18.3f, 3.45f, 119.72f);
                looktarget = new Vector3(-20f, 3.45f, 119.72f);
                break;
            case 4: //tavern
                target = new Vector3(15.34f, 3.45f, 114.55f);
                looktarget = new Vector3(-18.3f, 3.45f, 119.72f);
                break;
            case 5: //after tavern
                target = new Vector3(15.34f, 3.45f, 114.55f);
                looktarget = new Vector3(-18.3f, 3.45f, 119.72f);
                break;
            default:
                checkForMovement = false;
                break;
        }
    }
   
}
