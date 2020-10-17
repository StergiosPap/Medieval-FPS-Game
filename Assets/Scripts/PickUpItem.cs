using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject panel;
    public GameObject thisObject;
    public int itemID;
    private bool isNear;
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNear)
        {
            panel.SetActive(false);
            inventory.AddItem(itemID);
            thisObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            isNear = true;
            panel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            isNear = false;
            panel.SetActive(false);
        }
    }
}
