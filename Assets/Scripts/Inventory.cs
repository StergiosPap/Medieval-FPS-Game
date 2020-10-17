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

    public Text details; //Εμφανίζει τις λεπτομέρειες του αντικειμένου που πατήθηκε.

    public GameObject statsPanel, helpPanel; //Τα πάνελ που βρίσκονται στο πάνω μέρος της οθόνης.

    //Λίστα που περιέχει τα ID των αντικειμένων του παίκτη. Static διότι πρέπει να μεταφέρεται από τη μία σκηνή στην άλλη.
    public static IList<int> inventoryItems = new List<int>();

    //0 = Sword, 1 = Potion, 2 = Apple, 3 = Book, 4 = Scroll, 5 = Bottle of water
    public Sprite[] itemIcons = new Sprite[6];

    //Πίνακας με τα descriptions όλων των αντικειμένων.
    public static string[] itemDescription = { "A handmade sword made by Derrek the blacksmith. Only the strongest warrior can unleash its true power. Press 'F' to equip/unequip.", 
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

    //Κλείνει το inventory
    void CloseInventory()
    {
        statsPanel.SetActive(true);
        helpPanel.SetActive(true);
        inventoryUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    //Ανοίγει το inventory
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

    //Προσθέτει το item στη λίστα (πχ όταν αποκτηθεί μετά από κάποιο quest).
    public void AddItem(int itemID)
    {
        inventoryItems.Add(itemID);
    }
    

    //Ενημερώνει το inventory με τις πληροφορίες της λίστας inventoryItems. Καλείται όταν ανοίξει το inventory.
    public void UpdateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            slot[i].GetComponent<Slot>().icon = itemIcons[inventoryItems[i]];
            slot[i].GetComponent<Slot>().UpdateSlot(itemDescription[inventoryItems[i]]);
        }

        //Κάνει reset όσα slots δεν περιέχουν αντικείμενο.
        for (int i = inventoryItems.Count; i<12; i++)
        {
            slot[i].GetComponent<Slot>().ResetSlot();
        }
    }

    //Επιστρέφει true αν υπάρχει σπαθί στο inventory.
    public bool hasWeapon()
    {
        return inventoryItems.Contains(0);
    }


    //Επιστρέφει true αν έχει ολοκληρωθεί το 2ο quest (μήλο και βιβλίο στο inventory).
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


    //Επιστρέφει true αν έχει ολοκληρωθεί το 3ο quest (πάπυρος και μπουκάλι νερό στο inventory).
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


    //Καλείται όταν ο χρήστης πατήσει το κουμπί 'P'.
    public void UsePotion()
    {
        if (inventoryItems.Contains(1)) //Υπάρχει potion στο inventory.
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
