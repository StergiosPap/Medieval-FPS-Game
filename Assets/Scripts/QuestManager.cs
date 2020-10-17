using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    public Text description;
    public Text questTitleText1;
    public Text questTitleText2;
    public Text questTitleText3;

    private string questTitle1 = "A new warrior!";
    private string questTitle2 = "The first sword"; 
    private string questTitle3 = "Health comes first!";
    private string questBaseDescription1 = "Bob and Eliza need your help to save the village! Fight some orcs in the forest and level up to 5 to become a warrior! ";
    private string questBaseDescription2 = "Derrek can give you your first sword for free! All you have to do is give him the book he lent to Bob and an apple from the farm. ";
    private string questBaseDescription3 = "Rose promised to give you some potions for your adventure. But first you need to give her a bottle of water from the well and the recipe from Alice. ";
    private string questDescription1, questDescription2, questDescription3;

    void Start()
    {
        
    }

    public void OnEnable()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        switch (playerInfo.GetPhase())
        {
            case 6:
                questTitleText1.text = questTitle1;
                questTitleText1.color = Color.white;
                questDescription1 = questBaseDescription1 + "(Active)";
                break;
            case 7:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.white;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Active)";
                break;
            case 8:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.white;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Active)";
                break;
            case 9:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.green;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Completed)";
                break;
            case 10:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText3.text = questTitle3;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.green;
                questTitleText3.color = Color.white;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Completed)";
                questDescription3 = questBaseDescription3 + "(Active)";
                break;
            case 11:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText3.text = questTitle3;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.green;
                questTitleText3.color = Color.white;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Completed)";
                questDescription3 = questBaseDescription3 + "(Active)";
                break;
            case 12:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText3.text = questTitle3;
                questTitleText1.color = Color.white;
                questTitleText2.color = Color.green;
                questTitleText3.color = Color.green;
                questDescription1 = questBaseDescription1 + "(Active)";
                questDescription2 = questBaseDescription2 + "(Completed)"; 
                questDescription3 = questBaseDescription3 + "(Completed)";
                break;
            case 13:
                questTitleText1.text = questTitle1;
                questTitleText2.text = questTitle2;
                questTitleText3.text = questTitle3;
                questTitleText1.color = Color.green;
                questTitleText2.color = Color.green;
                questTitleText3.color = Color.green;
                questDescription1 = questBaseDescription1 + "(Completed)";
                questDescription2 = questBaseDescription2 + "(Completed)";
                questDescription3 = questBaseDescription3 + "(Completed)";
                break;
            default:
                break;
        }
    }

    public void Quest1Clicked()
    {
        description.text = questDescription1;
    }

    public void Quest2Clicked()
    {
        description.text = questDescription2;
    }

    public void Quest3Clicked()
    {
        description.text = questDescription3;
    }
}
