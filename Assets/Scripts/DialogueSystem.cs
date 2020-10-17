using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueGUI;
    public Transform dialogueBoxGUI;
    public GameObject pressEHelper;

    public float letterDelay = 0.0005f;
    public float letterMultiplier = 3f;

    public string Names;

    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;

    public KeyCode DialogueInput = KeyCode.E;

    private bool pressedE;

    private PlayerInfo playerInfo;

    private Inventory inventory;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        inventory = FindObjectOfType<Inventory>();
        dialogueText.text = "";
        pressedE = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressedE = true;
        }
    }

    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        dialogueGUI.SetActive(true);
        if (dialogueActive)
        {
            dialogueGUI.SetActive(false);
        }
    }

    public void NPCName()
    {
        outOfRange = false;
        dialogueBoxGUI.gameObject.SetActive(true);
        nameText.text = Names;
        if (pressedE)
        {
            pressedE = false;
            dialogueGUI.SetActive(false);
            if (!dialogueActive)
            {
                dialogueActive = true;
                StartCoroutine(StartDialogue());
            }
        }
        StartDialogue();
    }

    private IEnumerator StartDialogue()
    {
        if (!outOfRange)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;
            while (currentDialogueIndex<dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }

            while (true)
            {
                if(Input.GetKeyDown(DialogueInput) && !dialogueEnded)
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }


    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (!outOfRange)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";
            pressEHelper.gameObject.SetActive(false);
            while (currentCharacterIndex < stringLength)
            {

                if (string.Equals(stringToDisplay[currentCharacterIndex], '^')) //Ανεβάζει phase
                {
                    playerInfo.NextPhase();
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '*')) //Προσθήκη σπαθιού στο inventory
                {
                    inventory.AddItem(0);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '#')) //Προσθήκη health potion στο inventory
                {
                    inventory.AddItem(1);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '%')) //Προσθήκη βιβλίου στο inventory
                {
                    inventory.AddItem(3);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '&')) //Προσθήκη παπύρου στο inventory
                {
                    inventory.AddItem(4);
                }
                else //Κανονικός χαρακτήρας
                {
                    dialogueText.text += stringToDisplay[currentCharacterIndex];
                }



                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetKey(DialogueInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);
                    } else
                    {
                        yield return new WaitForSeconds(letterDelay);
                    }
                } else
                {
                    pressEHelper.gameObject.SetActive(true);
                    dialogueEnded = false;
                    break;
                }
            }

            while (true)
            {
                if (Input.GetKeyDown(DialogueInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";

        }
    }

    public void DropDialogue()
    {
        dialogueGUI.SetActive(false);
        dialogueBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUI.SetActive(false);
            dialogueBoxGUI.gameObject.SetActive(false);
        }
    }

}
