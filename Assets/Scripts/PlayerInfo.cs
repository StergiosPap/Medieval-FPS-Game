using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    //Stats του παίκτη.
    public int health = 50; //Τρέχουσα ζωή
    public int maxHealth = 50; //Μέγιστη ζωή (αυξάνεται με κάθε επίπεδο)
    public int experience = 0; //Πόντοι εμπειρίας
    public int level = 1; //Επίπεδο
    public int expGoal = 20; //Πόντοι για αύξηση επιπέδου
    public int phase = 0; //Φάση του παιχνιδιού

    public GameObject notificationPanel;
    public Text notificationText;
    private string notification;

    public AudioSource levelUpSound;
    

    public void NextPhase()
    {
        phase++;
    }

    public void LevelUp()
    {
        level++;
        maxHealth += 5; //Κάθε επίπεδο θα έχει max 5 πόντους ζωής περισσότερους από το προηγούμενο.
        expGoal += 10; //Κάθε επίπεδο θα απαιτεί 10 περισσότερους πόντους εμπειρίας από το προηγούμενο.

        health += 15; //Αναπλήρωση λίγης ζωής.
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        levelUpSound.Play();
    }

    public void GainExp(int exp)
    {
        experience += exp;
        notification = "+" + exp.ToString() + " Exp!";
        if (experience >= expGoal)
        {
            experience -= expGoal; //Reset πριν γίνει το level up.
            LevelUp();
            notification += " Level Up!";
        }
        OpenPanel();
        Invoke("ClosePanel", 1.5f);
    }

    //Αλλαγή στη ζωή του παίκτη (μπορεί να την μειώσει αν η παράμετρος hp είναι αρνητική)
    public void GainHealth(int hp)
    {
        health += hp;
        //Περιορισμός της τιμής.
        if (health > maxHealth) { health = maxHealth; }
    }


    void OpenPanel()
    {
        notificationText.text = notification;
        notificationPanel.SetActive(true);
    }

    void ClosePanel()
    {
        notificationPanel.SetActive(false);
    }




    //Getters
    public int GetPhase()
    {
        return phase;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetExperience()
    {
        return experience;
    }

    public int GetExperienceGoal()
    {
        return expGoal;
    }
}