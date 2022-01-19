using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationChange : MonoBehaviour
{
    public Animator anim;
    public Text locationText;
    public static string currentLocation = "Forest";
    private Scene currentScene; //Active scene

    public GameObject panel;

    public Transform transform;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene();
    }

    //Checks if the player's location has changed
    void Update()
    {
        if (currentScene.name == "Village")
        {
            if ((transform.position.z < 40) && !currentLocation.Equals("Forest"))
            {
                currentLocation = "Forest";
                OpenPanel();
            } else if ((transform.position.x > 70) && !currentLocation.Equals("Farm"))
            {
                currentLocation = "Farm";
                OpenPanel();
            } else if ((transform.position.x < -75) && (transform.position.z < 121) && !currentLocation.Equals("Holy Woods"))
            {
                currentLocation = "Holy Woods";
                OpenPanel();
            } else if ((transform.position.x >= -80) && (transform.position.x < 70) && (transform.position.z >= 40) && (transform.position.z <= 121) && !currentLocation.Equals("Alexandria"))
            {
                currentLocation = "Alexandria";
                OpenPanel();
            } else if ((transform.position.x < -145) && (transform.position.z >= 121) && !currentLocation.Equals("Grove of Orcs"))
            {
                currentLocation = "Grove of Orcs";
                OpenPanel();
            }
        }
    }

    //When the location changes, show the new location's name in a panel
    void OpenPanel()
    {
        locationText.text = currentLocation;
        panel.SetActive(true);
        anim.SetBool("Open", true);
        Invoke("SlidePanelUp", 2f);
    }

    void SlidePanelUp()
    {
        anim.SetBool("Open", false);
        Invoke("ClosePanel", 3f);
    }

    void ClosePanel()
    {
        panel.SetActive(false);
    }
}