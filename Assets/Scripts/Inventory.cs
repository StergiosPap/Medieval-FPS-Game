using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled = false;
    public GameObject inventoryUI;
    private int allSlots;
    private int enabledSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    public GameObject notificationPanel;
    public Text notificationText;

    private PlayerInfo playerInfo;

    public Text details; //Show the details of the item that was clicked

    public GameObject statsPanel, helpPanel; //Panels in the inventory screen

    //List that contains the IDs of the player's items. Static because we need to transfer the list from one scene to another.
    public static IList<int> inventoryItems = new List<int>();

    //0 = Sword, 1 = Potion, 2 = Apple, 3 = Book, 4 = Scroll, 5 = Bottle of water
    public Sprite[] itemIcons = new Sprite[6];

    //Description of every item
    public static string[] itemDescription = {
        "A handmade sword made by Derrek the blacksmith. Only the strongest warrior can unleash its true power. Press 'F' to equip/unequip.", 
        "A health potion made by Rose the apothecary. Press 'P' to use! (+20 Health)", 
        "A fresh apple from Alexandria's crops.", 
        "A book called 'The Art of craftsmanship'.", 
        "A scroll that contains the recipe for a health potion.", 
        "A bottle of water from a well in Holy Woods. Some say that it is blessed by the sacred lake next to it."
    };


    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();

        allSlots = 12;
        slot = new GameObject[allSlots];
        
        for (int i=0; i<allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventoryEnabled = !inventoryEnabled;
            if (!inventoryEnabled)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    void CloseInventory()
    {
        statsPanel.SetActive(true);
        helpPanel.SetActive(true);
        inventoryUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    void OpenInventory()
    {
        statsPanel.SetActive(false);
        helpPanel.SetActive(false);
        UpdateInventory();
        details.text = "";
        inventoryUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;
            Item item = itemPickedUp.GetComponent<Item>();
        }
    }

    //Adds an item in the list (for example when it is gained after a quest)
    public void AddItem(int itemID)
    {
        inventoryItems.Add(itemID);
    }
    
    //When the inventory opens, this function updates its contents in order to display them
    public void UpdateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            slot[i].GetComponent<Slot>().icon = itemIcons[inventoryItems[i]];
            slot[i].GetComponent<Slot>().UpdateSlot(itemDescription[inventoryItems[i]]);
        }

        //Reset every slot that doesn't contain an item
        for (int i = inventoryItems.Count; i<12; i++)
        {
            slot[i].GetComponent<Slot>().ResetSlot();
        }
    }

    //Check if there is a sword in the inventory
    public bool hasWeapon()
    {
        return inventoryItems.Contains(0);
    }

    //Check if the second quest is completed. If yes, remove the requested items from the inventory.
    public bool quest2Completed()
    {
        if (inventoryItems.Contains(2) && inventoryItems.Contains(3))
        {
            inventoryItems.Remove(2);
            inventoryItems.Remove(3);
            return true;
        }
        return false;
    }

    //Check if the third quest is completed. If yes, remove the requested items from the inventory.
    public bool quest3Completed()
    {
        if (inventoryItems.Contains(4) && inventoryItems.Contains(5))
        {
            inventoryItems.Remove(4);
            inventoryItems.Remove(5);
            return true;
        }
        return false;
    }
    
    //Gets called when the player presses the "P" key
    public void UsePotion()
    {
        if (inventoryItems.Contains(1)) //If there is a potion in the inventory
        {
            playerInfo.GainHealth(20);
            inventoryItems.Remove(1);
            notificationText.text = "Used health potion!";
        } else
        {
            notificationText.text = "You don't have any potions!";
        }

        OpenPanel();
        Invoke("ClosePanel", 1.5f);
    }


    void OpenPanel()
    {
        notificationPanel.SetActive(true);
    }

    void ClosePanel()
    {
        notificationPanel.SetActive(false);
    }

}
