using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Transform ChatBackground;
    public Transform NPCCharacter;

    private DialogueSystem dialogueSystem;

    public string name;

    [TextArea(3, 10)]
    public string[] sentences, sentences1, sentences2, sentences3, sentences4, sentences5, sentences6, sentences7, sentences8, sentences9, sentences10, sentences11, sentences12, sentences13;

    public bool pressedE;

    private PlayerInfo playerInfo;
    private Inventory inventory;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        inventory = FindObjectOfType<Inventory>();
        pressedE = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressedE = true;
            if (pressedE)
            {
                this.gameObject.GetComponent<NPC>().enabled = true;
                dialogueSystem.Names = name;

                //Έλεγχος αν κάποιο quest έχει ολοκληρωθεί.

                //Bob's Quest
                if (playerInfo.GetLevel() >= 5 && playerInfo.GetPhase() == 12 && name.Equals("Bob"))
                {
                    playerInfo.NextPhase();
                }

                //Derrek's Quest
                if (playerInfo.GetPhase() == 7 && name.Equals("Derrek") && inventory.quest2Completed())
                {
                    playerInfo.NextPhase();
                }

                //Rose's Quest
                if (playerInfo.GetPhase() == 10 && name.Equals("Rose") && inventory.quest3Completed())
                {
                    playerInfo.NextPhase();
                }




                //Ανάλογα με το phase του παιχνιδιού, φέρνει τον αντίστοιχο πίνακα με τις προτάσεις.
                switch (playerInfo.GetPhase())
                {
                    case 0: dialogueSystem.dialogueLines = sentences; break;
                    case 1: dialogueSystem.dialogueLines = sentences1; break;
                    case 2: dialogueSystem.dialogueLines = sentences2; break;
                    case 3: dialogueSystem.dialogueLines = sentences3; break;
                    case 4: dialogueSystem.dialogueLines = sentences4; break;
                    case 5: dialogueSystem.dialogueLines = sentences5; break;
                    case 6: dialogueSystem.dialogueLines = sentences6; break;
                    case 7: dialogueSystem.dialogueLines = sentences7; break;
                    case 8: dialogueSystem.dialogueLines = sentences8; break;
                    case 9: dialogueSystem.dialogueLines = sentences9; break;
                    case 10: dialogueSystem.dialogueLines = sentences10; break;
                    case 11: dialogueSystem.dialogueLines = sentences11; break;
                    case 12: dialogueSystem.dialogueLines = sentences12; break;
                    case 13: dialogueSystem.dialogueLines = sentences13; break;
                    default: dialogueSystem.dialogueLines = sentences13; break;
                }


                FindObjectOfType<DialogueSystem>().NPCName();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<DialogueSystem>().OutOfRange();
            this.gameObject.GetComponent<NPC>().enabled = false;
            pressedE = false;
        }
    }

}
