using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public int ID;
    public string type;
    public string description;
    public bool empty;
    public Sprite icon;
    public Transform slotIconGO;
    public Text details;

    void Start()
    {
        slotIconGO = transform.GetChild(0);
    }

    void OnBecomeVisible()
    {
        details.text = "";
    }

    public void UpdateSlot(string desc)
    {
        //Changes the transparency in the slot
        Image image = slotIconGO.GetComponent<Image>();
        Color c = image.color;
        c.a = 255;
        image.color = c;

        //Sets the panel's image
        slotIconGO.GetComponent<Image>().sprite = icon;

        description = desc;
    }

    public void ResetSlot()
    {
        //Changes the transparency in the slot
        Image image = slotIconGO.GetComponent<Image>();
        Color c = image.color;
        c.a = 0;
        image.color = c;

        description = "";
    }

    //Shows the details in the textbox
    public void ShowDetails()
    {
        details.text = description;
    }

}
