using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private bool insideBounds = false;
    private string destination;
    public GameObject helperGUI;
    public GameObject fadeOutCanvas;
    public static bool firstStart = true;

    void Start()
    {
        if (firstStart)
        {
            fadeOutCanvas.SetActive(true);
            firstStart = false;
        }
    }


    void Update()
    {
        if (insideBounds && Input.GetKeyDown(KeyCode.E)) //If "E" is pressed when the player is near the door, change sceen
        {
            SceneManager.LoadScene(destination);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "ChangeScene")
        {
            helperGUI.SetActive(true);
            insideBounds = true;
            switch (collider.name) //Change destination from village to farms and vice-versa
            {
                case "ToVillage":
                    destination = "Village";
                    break;
                case "ToFarms":
                    destination = "Farm";
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        //When the player moves away from the door, stop displaying the panel
        if (collider.tag == "ChangeScene")
        {
            helperGUI.SetActive(false);
            insideBounds = false;
        }
    }
}
