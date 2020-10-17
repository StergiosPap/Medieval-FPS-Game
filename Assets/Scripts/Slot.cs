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

    //Εμφανίζει την εικόνα του αντικειμένου που αποκτήθηκε.
    public void UpdateSlot(string desc)
    {
        //Αλλάζει το transparency του panel στο slot.
        Image image = slotIconGO.GetComponent<Image>();
        Color c = image.color;
        c.a = 255;
        image.color = c;

        //Θέτει την εικόνα του panel.
        slotIconGO.GetComponent<Image>().sprite = icon;

        description = desc;
    }

    public void ResetSlot()
    {
        //Αλλάζει το transparency του panel στο slot.
        Image image = slotIconGO.GetComponent<Image>();
        Color c = image.color;
        c.a = 0;
        image.color = c;

        description = "";
    }

    //Εμφανίζει στο textbox τις λεπτομέρειες του αντικειμένου που πατήθηκε.
    public void ShowDetails()
    {
        details.text = description;
    }

}
