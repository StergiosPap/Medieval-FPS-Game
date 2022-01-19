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
        /* Displays the NPC's dialogue string.
         * In some cases the dialogue depends on the current "story phase".
         * Whenever this function finds the character "^", it progresses the story phase.
         * There are other characters which are not shown in the dialogue window
         * and perform a certain action instead. For example, add a certain item to the player's inventory.
         */
        if (!outOfRange)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";
            pressEHelper.gameObject.SetActive(false);
            while (currentCharacterIndex < stringLength)
            {
                if (string.Equals(stringToDisplay[currentCharacterIndex], '^')) //Increase phase
                {
                    playerInfo.NextPhase();
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '*')) //Add sword to inventory
                {
                    inventory.AddItem(0);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '#')) //Add health potion to inventory
                {
                    inventory.AddItem(1);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '%')) //Add book to inventory
                {
                    inventory.AddItem(3);
                }
                else if (string.Equals(stringToDisplay[currentCharacterIndex], '&')) //Add scroll to inventory
                {
                    inventory.AddItem(4);
                }
                else //Other character. Display as normal.
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

    //When the player gets out of NPC's range, stop displaying dialogue window.
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