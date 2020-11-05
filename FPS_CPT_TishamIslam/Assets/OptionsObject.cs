using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsObject : MonoBehaviour
{
    public GameObject options;
    private float distance; //NOTE: because of how rectangular transforms work, this is distance away from the edge of the screen. i.e.
    public float speed;
    public float openLimit;
    [SerializeField] private float originalLoc;
    private bool optionDisplayed;

    public AudioSource gameAudio;
    public Slider xSens;
    public Slider ySens;
    public Slider volume;
    public Toggle mute;


    private void Start()
    {
        //only hide options if this is the startup scene
        Scene sc = SceneManager.GetActiveScene();
        if (sc.name == "Startup")
        {
            options.SetActive(false);
        }

        //find the original location, then set the distance halfway towards the left of the screen
        originalLoc = transform.position.x;
        distance = transform.position.x / 2;

        //open limit can't be one or higher, nor 0 or lower
        if (openLimit >= 1 || openLimit <= 0)
        {
            openLimit = .5f;
        }
    }

    void Update()
    {
        if (optionDisplayed)
        {
            //move the buttons over
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, distance, Time.deltaTime * speed),
                transform.position.y, transform.position.z);

            //if the buttons go far enough (in this case 2/3 of the way there )...    note they'll always go left
            //the set distance is half the original position, remember that the original distance is not 0, but half of the width of the screen size
            //therefore, we need to go to orgLoc - distance if we set it manually, + whatever fraction of distance we want to go past to activate the actual menu items
            if (transform.position.x <= originalLoc - distance * openLimit)
            {
                //set the options to show up
                options.SetActive(true);
            }
        }
        else
        {
            //move the buttons back
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, originalLoc, Time.deltaTime * speed),
                transform.position.y, transform.position.z);
        }
    }

    public void HideShowOptions()
    {
        //looks the same as written
        if (optionDisplayed)
        {
            optionDisplayed = false;
            options.SetActive(false);
        }
        else
        {
            optionDisplayed = true;
        }
    }

    public void UpdateSettings()
    {
        //update static vars, so that they are kept between scenes
        Options.XSens = xSens.value;
        Options.YSens = ySens.value;
        Options.Volume = volume.value;
        Options.Mute = mute.isOn;

        //if muted, keep it muted
        if (Options.Mute)
        {
            gameAudio.volume = 0;
        }
        else
        {
            //change the volume to whatever volume is set
            gameAudio.volume = Options.Volume;
        }
    }
}
