using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FPSMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 8f; //Ταχύτητα του παίκτη
    public float gravity = -9.81f; //Επιτάχυνση της βαρύτητας
    public float jumpHeight = 3f; //Μέγιστο ύψος άλματος

    Vector3 velocity;

    public bool isGrounded = true;
    public Inventory inventory;

    private bool isEquipped; 

    Animator anim;

    public bool isPaused = false;
    public GameObject pausedPanel; //Το πάνελ που ανοίγει όταν το παιχνίδι είναι σε παύση.
    public GameObject inventoryPanel; //Το πάνελ που δείχνει το inventory και τα quests.
    public GameObject notificationPanel; //Το notification panel στο κάτω μέρος της οθόνης.
    public Text notificationText;

    //Οι πληροφορίες του παίκτη (μπάρα ζωής και επίπεδο).
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

        //Κλειδώνει τον κέρσορα
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Scene currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        UpdateStatsPanel();
                
        //Έλεγχος για πάτημα Escape ώστε να ανοίξει/κλείσει το μενού.
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


        //Equip/Unequip το όπλο.
        if (!isPaused && !isGameOver && Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.hasWeapon()) //Αν έχει σπαθί στο inventory
            {
                isEquipped = !isEquipped;
                if (isEquipped) //Εμφανίζει ή εξαφανίζει το σπαθί από την οθόνη
                {
                    sword.layer = 0; //Default layer
                } else
                {
                    sword.layer = 9; //Player layer (που είναι αόρατο)
                }
            } else
            {
                OpenPanel();
                Invoke("ClosePanel", 1.5f);
            }
        }

        //Χρήση potion
        if (!isPaused && !isGameOver && Input.GetKeyDown(KeyCode.P))
        {
            inventory.UsePotion();
        }


        if (isEquipped && Input.GetMouseButtonDown(0))
        {
            swordCollider.enabled = true; //Ενεργοποιεί το collider που μπορεί να χτυπήσει τον εχθρό.
            anim.SetBool("isAttacking", true);
        }

        if (isEquipped && Input.GetMouseButtonUp(0))
        {
            anim.SetBool("isAttacking", false);
        }

        //Αν βρισκόμαστε σε pause και πατηθεί το Q γίνεται επιστροφή στο Main Menu.
        if (isPaused && Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Menu");
        }


        //Μετακίνηση παίκτη
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //Άλμα
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false;
            //Υπάρχει ένα χρονικό όριο (μισό δευτερόλεπτο) μέσα στο οποίο ο παίκτης δεν μπορεί να ξαναπηδήξει.
            Invoke("Switch", 0.5f);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    private void Switch()
    {
        isGrounded = true;
    }

    //Μέθοδος που εκτελέιται κατά την σύγκρουση με το έδαφος.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }


    //Ενημερώνει το πάνελ με τα stats
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


    //Χρειαζόμαστε τιμή μεταξύ 0 και 1 για το ProgressBar.
    float CalculateHealth(int health, int maxhealth)
    {
        return (float)health / (float)maxhealth;
    }


    //Βρίσκει το χρώμα του Health Bar ανάλογα με το ποσοστό ζωής.
    Color32 CalculateColor(int hp, int max_hp)
    {
        float health = (float)hp;
        float maxhealth = (float)max_hp;
        if (health / maxhealth >= 0.75) //Health 75-100%
        {
            return new Color32(0, 255, 0, 150); //Πράσινο
        }
        else if (health / maxhealth >= 0.5) //Health 50-75%
        {
            return new Color32(255, 255, 0, 150); //Κίτρινο
        }
        else if (health / maxhealth >= 0.2) //Health 20-50%
        {
            return new Color32(255, 128, 0, 150); //Πορτοκαλί
        }
        else //Health 0-20%
        {
            return new Color32(255, 0, 0, 150); //Κόκκινο
        }
    }


    //Χρήση βοηθητικού πάνελ που ενημερώνει ότι δεν υπάρχει σπαθί στο inventory.
    void OpenPanel()
    {
        notificationText.text = "You don't have a sword yet!";
        notificationPanel.SetActive(true);
    }

    void ClosePanel()
    {
        notificationPanel.SetActive(false);
    }

    
    void GameOver()
    {
        isGameOver = true;
        GOpanel.SetActive(true);
        Time.timeScale = 0;
    }



}
