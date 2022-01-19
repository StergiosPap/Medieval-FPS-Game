using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    //Player stats
    public int health = 50; //Current health points
    public int maxHealth = 50; //Maximum health points. Increases with each level.
    public int experience = 0; //Experience points
    public int level = 1;
    public int expGoal = 20; //Exp needed for next level
    public int phase = 0; //Story phase

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
        maxHealth += 5; //In each level, increase the player's max health points
        expGoal += 10; //Each level requires 10 exp more than the previous one

        health += 15; //Restores some life
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
            experience -= expGoal; //Reset before levelling up.
            LevelUp();
            notification += " Level Up!";
        }
        OpenPanel();
        Invoke("ClosePanel", 1.5f);
    }

    //Change player's HP (can reduce if the parameter is negative)
    public void GainHealth(int hp)
    {
        health += hp;
        //Clamping
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