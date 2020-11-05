using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
    public Text bigText;
    public AudioSource audio;
    private string scName;

    private void Start() {
        //lock cursor within game window
        Cursor.lockState = CursorLockMode.Confined;

        //get audio component
        audio = GetComponent<AudioSource>();
        Cursor.visible = true;

        if (Options.Mute) {
            audio.volume = 0;
        } else {
            //change the volume to whatever volume is set
            audio.volume = Options.Volume;
        }

        //get scene name to auto set text
        Scene sc = SceneManager.GetActiveScene();
        scName = sc.name;

        if (scName == "Startup") {
            bigText.text = "A Hero's Journey";
        } else {
            bigText.text = Options.DeathMessage;
        }
    }

    public void LoadScene(string level) {
        //load sent level name
        SceneManager.LoadScene(level);
    }

    public void LoadLastScene() {
        //load last level name
        try {
            SceneManager.LoadScene(Options.LastScene);
        } catch {
            //default to the main menu if it doesn't work right
            SceneManager.LoadScene("Startup");
        }
    }

    public void ExitGame() {
        //exit the game
        Application.Quit();
    }
}
