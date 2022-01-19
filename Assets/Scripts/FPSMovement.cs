using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FPSMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 8f; //Player speed
    public float gravity = -9.81f; //Gravity acceleration
    public float jumpHeight = 3f; //Max jump height

    Vector3 velocity;

    public bool isGrounded = true;
    public Inventory inventory;

    private bool isEquipped; 

    Animator anim;

    public bool isPaused = false;
    public GameObject pausedPanel; //Panel that opens when the game is paused
    public GameObject inventoryPanel; //Panel that shows the inventory and quests
    public GameObject notificationPanel; //Notification panel in the bottom side of the screen
    public Text notificationText;

    //Player info (health points and level)
    public Text levelText;
    public Slider slider;
    public Image barColor;

    public PlayerInfo playerInfo;
    
    public GameObject sword;
    public Collider swordCollider;

    public GameObject GOpanel; //Game over panel
    private bool isGameOver;


    void Start()
    {
        inventoryPanel.SetActive(false);

        playerInfo = FindObjectOfType<PlayerInfo>();
        inventory = FindObjectOfType<Inventory>();

        anim = GetComponent<Animator>();
        isEquipped = false;

        isGameOver = false;

        //Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Scene currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        UpdateStatsPanel();
                
        //Check whether the Escape key is pressed in order to open/close the menu
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            isPaused = !isPaused;
            pausedPanel.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");
        }


        //Equip/Unequip the sword
        if (!isPaused && !isGameOver && Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.hasWeapon()) //If the inventory contains a sword
            {
                isEquipped = !isEquipped;
                if (isEquipped) //Show/Don't show the sword held by the player
                {
                    sword.layer = 0; //Default layer
                } else
                {
                    sword.layer = 9; //Player layer (invisible sword)
                }
            } else
            {
                OpenPanel();
                Invoke("ClosePanel", 1.5f);
            }
        }

        //Use potion
        if (!isPaused && !isGameOver && Input.GetKeyDown(KeyCode.P))
        {
            inventory.UsePotion();
        }


        if (isEquipped && Input.GetMouseButtonDown(0))
        {
            swordCollider.enabled = true; //Activates the collider that can hit the enemies
            anim.SetBool("isAttacking", true);
        }

        if (isEquipped && Input.GetMouseButtonUp(0))
        {
            anim.SetBool("isAttacking", false);
        }

        //If the game is paused and the "Q" key is pressed, return to Main Menu
        if (isPaused && Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Menu");
        }
        
        //Player movement
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false;
            //There is a short period in which the player cannot jump again
            Invoke("Switch", 0.5f);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    private void Switch()
    {
        isGrounded = true;
    }

    //Function that is called when the player collides with the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    
    //Updates the stats panel
    private void UpdateStatsPanel()
    {
        levelText.text = "Level " + playerInfo.GetLevel().ToString() + " (" + playerInfo.GetExperience().ToString() + "/" + playerInfo.GetExperienceGoal().ToString() + ")";
        slider.value = CalculateHealth(playerInfo.GetHealth(), playerInfo.GetMaxHealth());
        barColor.color = CalculateColor(playerInfo.GetHealth(), playerInfo.GetMaxHealth());

        if (playerInfo.GetHealth() <= 0)
        {
            GameOver();
        }
    }
    
    //Return a value between 0 and 1 for the ProgressBar
    float CalculateHealth(int health, int maxhealth)
    {
        return (float)health / (float)maxhealth;
    }

    //Finds the appropriate color for the Health Bar, depending on the remaining health percentage
    Color32 CalculateColor(int hp, int max_hp)
    {
        float health = (float)hp;
        float maxhealth = (float)max_hp;
        if (health / maxhealth >= 0.75) //Health 75-100%
        {
            return new Color32(0, 255, 0, 150); //Green
        }
        else if (health / maxhealth >= 0.5) //Health 50-75%
        {
            return new Color32(255, 255, 0, 150); //Yellow
        }
        else if (health / maxhealth >= 0.2) //Health 20-50%
        {
            return new Color32(255, 128, 0, 150); //Orange
        }
        else //Health 0-20%
        {
            return new Color32(255, 0, 0, 150); //Red
        }
    }
    
    //Opens a notification panel that informs the player that they don't have a sword in the inventory
    void OpenPanel()
    {
        notificationText.text = "You don't have a sword yet!";
        notificationPanel.SetActive(true);
    }

    void ClosePanel()
    {
        notificationPanel.SetActive(false);
    }

    //Show Game Over panel and freeze the in-game time
    void GameOver()
    {
        isGameOver = true;
        GOpanel.SetActive(true);
        Time.timeScale = 0;
    }

}
