using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour {
    private float sensY;
    private float verMove;

    void Update() {
        //don't do anything while the game is paused
        if (Time.timeScale == 0) {
            return;
        }

        sensY = Options.YSens;
        //multiply sensitivity by what input we get, no mouse X as to not impede player movement
        verMove += sensY * Input.GetAxis("Mouse Y");

        //rotate by our constantly updated rotation values
        //clamp max vertical rotation so you can't 
        transform.eulerAngles = new Vector3(Mathf.Clamp(verMove, -60, 60), transform.eulerAngles.y, 0);
    }
}
