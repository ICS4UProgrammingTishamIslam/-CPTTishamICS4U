using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    [Header("Level Loading")]
    [Tooltip("The next level's name")] public string nextLevel;

    [Header("General Objective")]
    [Tooltip("The text component for your objective, drag and drop")] public Text objectiveText;
    [Tooltip("Array of messages to be used in the objective text")] public string[] messages; //note that messages should be chronological
    //[Tooltip("")] public bool[] objectives;
    //[Tooltip("Total number of objectives, once reached the next level will load")]          public int totalObjectives;
    //[Tooltip("Counts the amount of objectives complete")] [SerializeField]                  private int objectiveCounter;

    [Header("Kill Objective")]
    [Tooltip("Enables/Disables the kill objective")] public bool killObjective;
    [Tooltip("Enables/Disables the survive objective")] public bool surviveObjective;
    [Tooltip("The amount of kills you need")] public int killAmount;
    [Tooltip("Counts the amount of kills you've got")] [SerializeField] private int killCounter;

    [Header("Defend Objective")]
    [Tooltip("Enables/Disables the defend objective")] public bool defendObjective;
    [Tooltip("The total health of the object to defend")] public int defendHealth;
    [Tooltip("The current health of the object to defend")] private int defendCurrent;
    [Tooltip("How long you have to defend the object")] public float defendTime;
    [Tooltip("How long there is left to defend the object")] public float defendRemaining;
    [Tooltip("Text to show the timer")] public Text defendText;

    [Header("Escape Objective")]
    [Tooltip("Enables/Disables the escape objective")] public bool escapeObjective;
    [Tooltip("How long you have to escape")] public float escapeTime;
    [Tooltip("How long there is left to escape")] public float escapeRemaining;
    [Tooltip("The distance you have to go to win")] public float finishDistance;

    [Header("Other Displays")]
    [Tooltip("Displays crosshair")] public GameObject crosshair;
    [Tooltip("Canvas that displays pause text and options")] public Canvas pauseCanvas;
    [Tooltip("Text to show various other messages, mostly death messages")] public Text deathText;

    new private AudioSource audio;
    private Player player;

    void Start() {
        Options.GunEnabled = true;
        //lock the cursor within the game window
        Cursor.lockState = CursorLockMode.Confined;

        //get audio component and the player gameobject, and hide conditional text
        audio = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        deathText.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);

        //set the objective to the first message
        objectiveText.text = messages[0];

        //get the current scene name to let you go back to the current level and not start over again if you die
        Scene sc = SceneManager.GetActiveScene();
        Options.LastScene = sc.name;

        //make sure time scale and the cursor is reset
        Time.timeScale = 1;
        Cursor.visible = false;

        //if there's a defend objective, set up some stuff for it
        if (defendObjective) {
            defendRemaining = defendTime;
            defendCurrent = defendHealth;
            defendText.text = messages[1] + defendCurrent + "/" + defendHealth;
        }

        //survive objective means that kill counting is something that must be used
        if (surviveObjective) {
            killObjective = true;
        }

        if (escapeObjective) {
            Options.GunEnabled = false;
            escapeRemaining = escapeTime;
        }
    }

    public void Unpause() {
        //alternative to pressing p to unpause
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }



    private void Update() {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1) {
            pauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        } else if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0) {
            pauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
        }

        //if neither time based objective is used
        if (!defendObjective && !escapeObjective) {
            return;
        }

        if (defendRemaining <= 0 && defendObjective) {
            deathText.gameObject.SetActive(true);
            deathText.text = "You've defended the castle successfully!";
            objectiveText.text = "Time Remaining: 0";

            Options.DeathMessage = "You've saved the kingdom from invading the castle! Thanks for playing!";

            InvokeRepeating("SlowTime", 0, .1f);
            StartCoroutine(LoadLevelAfterDelay(3, nextLevel));
            return;
        }

        //if you fail to run away
        if (escapeRemaining <= 0 && escapeObjective) {
            PlayerDied("Time's Up");
            return;
        }

        //doesn't matter if either value is subtracted from, as it just won't be used
        escapeRemaining -= Time.deltaTime;
        defendRemaining -= Time.deltaTime;

        //set objective text to be a countdown after 5 seconds have passed so you can read the objective
        if (defendRemaining < defendTime - 5 && defendObjective) {
            objectiveText.text = "Time Remaining: " + defendRemaining.ToString("F2");
        }

        if (escapeRemaining < escapeTime - 5 && escapeObjective) {
            objectiveText.text = "Time Remaining: " + escapeRemaining.ToString("F2");

            //check if player passed the finish line
            if (player.transform.position.x >= finishDistance) {
                //add success message 
                deathText.gameObject.SetActive(true);
                deathText.text = "You've run from the goblins successfully!";

                //slow time and go to the next scene
                InvokeRepeating("SlowTime", 0, .1f);
                StartCoroutine(LoadLevelAfterDelay(3, nextLevel));
                return;
            }
        }

    }


    public void PlayerDied(string cause) {
        deathText.gameObject.SetActive(true);
        player.maxMoveSpeed = 0;

        switch (cause) {
            case "Fall":
                deathText.text = "You've fallen. OOF!";
                Options.DeathMessage = "You splat when you hit the ground. Yikes!";
                break;
            case "Goblin":
                Options.DeathMessage = "You got blown to pieces by a tiny goblin. Pathetic!";
                break;
            case "Defence Failed":
                deathText.text = "The goblins broke through the gates!";
                Options.DeathMessage = "The castle has fallen, and the goblins have killed everyone. Good job, \"Hero\"";
                break;
            case "Time's up":
                deathText.text = "You've been swarmed by goblins!";
                Options.DeathMessage = "You've failed to escape the goblins in time! Run faster!";
                break;
        }

        if (surviveObjective) {
            Options.DeathMessage = "You've served us well by killing " + killCounter + " goblins. Thank you for your aid! Fight once more?";
        }

        InvokeRepeating("SlowTime", 0, .1f);
        StartCoroutine(LoadLevelAfterDelay(5, "DeathScene"));
    }

    public void SlowTime() {
        Time.timeScale -= Time.timeScale / 50;
    }

    IEnumerator LoadLevelAfterDelay(float delay, string level) {
        Options.GunEnabled = false;
        if (crosshair != null) 
        crosshair.SetActive(false);

        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(level);
    }

    public void ObjectiveKilled() {
        //if there is no kill objective and this is called for whatever reason, return
        if (!killObjective) {
            return;
        }

        killCounter++;
        objectiveText.text = "Enemies killed: " + killCounter + "/" + killAmount;
        if (surviveObjective) {
            objectiveText.text = "Enemies killed: " + killCounter;
            return;
        }

        if (killCounter >= killAmount) {
            //if I need multiple objectives
            //ObjectiveComplete();
            deathText.text = "The goblins have been defeated, but the castle's under attack!";
            deathText.gameObject.SetActive(true);

            InvokeRepeating("SlowTime", 0, .1f);
            StartCoroutine(LoadLevelAfterDelay(3, nextLevel));
        }
    }

    /*if I need multiple objectives
    public void ObjectiveComplete()
    {
        objectiveCounter++;
        if (objectiveCounter == totalObjectives)
        {
            InvokeRepeating("SlowTime", 0, .1f);
            StartCoroutine(LoadLevelAfterDelay(3, nextLevel));
        }
    }
    */

    public void DefenceHit() {
        defendCurrent--;
        defendText.text = messages[1] + defendCurrent + "/" + defendHealth;
        if (defendCurrent <= 0) {
            PlayerDied("Defence Failed");
        }
    }

    public void Restart() {
        //load last level name
        try {
            SceneManager.LoadScene(Options.LastScene);
        } catch {
            SceneManager.LoadScene("Startup");
        }
    }

    public void LoadScene(string level) {
        SceneManager.LoadScene(level);
    }

    public void ExitGame() {
        Application.Quit();
    }
}